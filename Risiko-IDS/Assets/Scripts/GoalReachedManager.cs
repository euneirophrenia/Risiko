using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

/* Ok, questa classe non è di certo la più leggibile di tutte, quindi prepongo un commentone esplicativo.
 * Per prima cosa, l'idea è di usare la reflection per:
 * 		A) Autodedurre i tipi di SecretGoal esistenti
 * 		B) Istanziarli correttamente
 * A questo scopo, ogni classe che implementi SecretGoal, dovrà o essere capace di inizializzarsi da sola,
 * o accettare 1 parametro ed esporre un attributo di tipo "ConstructorArgumentsInfo", con il quale specificare alcune cose:
 * 		- il tipo del parametro (espresso come stringa e indipendente dalla classe, magari potremmo fattorizzare in un enum)
 * 	Nota:espresso come stringa e non come tipo per aumentare la semanitca. Un "array di stringhe" può così avere diversi significati
 * 		- eventuale max/min (opzionali) che il parametro deve rispettare (nel caso fosse numerico, ad esempio)
 * 		- se il parametro deve essere unico oppure no (perché ad esempio "Conquista N territori" non ha bisogno di N univoco).
 * 
 * Per fare un corretto match delle richieste, il Manager si ricorda una mappa Tipo-"funzione che genera un valore buono per il tipo".
 * Ho preparato alcune funzioni per ottenere:
 * 		-un intero (che serve per il "Conquista N territori")
 * 		-un nome di player (che serve per il "Elimina quel giocatore")
 * 		-array di stringhe nomi di continenti, in modo che l'array sia univoco (per l'obiettivo "Conquista certi territori")
 * 
 * Si espone un metodo "Check()" per controllare se qualche obiettivo è stato raggiunto, nel caso si scatena un evento passando l'istanza del
 * giocatore vincitore. Chi si registrerà, aggancerà un metodo che mostri qualche cosa, tipo un popup o quello che gli pare.
 * In realtà, si passa un IEnumerable<Giocatore>, perché può esistere il caso in cui 2 giocatori vincano contemporaneamente.
 * (Es:
 * 			-playerA => obiettivo : Elimina C
 * 			-playerB => obiettivo: conquista questi 2 continenti;
 * 			-plyerB raggiunge il proprio obiettivo e nel farlo elimina C (che come ultimo stato in suo possesso aveva lo stato mancante a B).
 * 			-playerA e playerB vincono a pari merito, direi..
 * )
 * 
 * Per estendere (= aggiungere un obiettivo segreto) occorre semplicemente:
 * 		-creare la classe con il metodo per fare il check (NB: nello stesso assembly del manager)
 * 		-dichiarare di cosa si ha bisogno mediante attributo
 * 		-predisporre una funzione apposita in questo manager nel caso non esista già, per fornire valori casuali opportuni
 * 
 * */

public class GoalReachedManager : IManager
{
	private readonly Type[] _tipi;
	private readonly List<SecretGoal> _obiettivi;

	private delegate T random<T>(bool uniqe, int min, int max);

	public delegate void GetWinner(IEnumerable<Giocatore> g);
	public event GetWinner GoalReached;

	private readonly Dictionary<string, random<object>> _randomGetter;
	private readonly Dictionary<string, List<object>> _usedValues;

	public GoalReachedManager()
	{
		_tipi=  (	from tipo in this.GetType().Assembly.GetTypes() 
					where typeof(SecretGoal).IsAssignableFrom(tipo) && !tipo.IsInterface
					select tipo
		         ).ToArray();

		_obiettivi= new List<SecretGoal>();

		_usedValues=new Dictionary<string, List<object>>();
		_randomGetter=new Dictionary<string, random<object>>();

		#region Inizializzazione della mappa Tipo-funzione
		_randomGetter["int"]=this.randomInt;
		_randomGetter["player"]=this.randomPlayerName;
		_randomGetter["continents"]=this.randomContinents;

		#endregion

	}
	

    public SecretGoal GenerateGoal()
    {
		if (_tipi.Length<1)
			throw new TypeLoadException("Non sono state trovate classi che implementino SecretGoal. " +
				"Assicurarsi che siano in questo stesso assembly.");

		int _next=UnityEngine.Random.Range(0, _tipi.Length);
		Type[] constructorParam = {typeof(object)};
		Attribute[] attrs = (Attribute[]) _tipi[_next].GetConstructor(constructorParam).GetCustomAttributes(typeof(ConstructorArgumentsInfo), false);
		SecretGoal g;
		if (attrs.Length<1)
		{

			 g= (SecretGoal) Activator.CreateInstance(_tipi[_next]);
			_obiettivi.Add(g);
			return g;
		}
		ConstructorArgumentsInfo attr = (ConstructorArgumentsInfo) attrs[0];
		object[] param={_randomGetter[attr.Tipo](attr.IsUnique, attr.Min, attr.Max)};

	    g = (SecretGoal )Activator.CreateInstance(_tipi[_next], param);
		_obiettivi.Add(g);
		return g;
    }

	private bool FindArray<T>(T[] what, IEnumerable<object> where)
	{
		object[] control = new List<object>(where).ToArray();
		if (control.Length<1 || !control[0].GetType().IsAssignableFrom(typeof(object[])))
			return false;

		bool found;
		foreach (object[] x in where)
		{
			found=true;
			foreach (object element in x)
			{
				found&=what.Contains(element);
				if (!found)
					break;
			}
			if (found)
				return true;
		}
		return false;
	}

	public void RebindPlayer(ref string name)
	{
		string newname;
		do
		{
			_usedValues["player"].Remove(name);
			newname= this.randomPlayerName(true, 0, 0);
		}
		while (newname.Equals(name));

		name=newname;
	}

	public void Check()
	{
		bool gameover=false;
		List<Giocatore> winners=new List<Giocatore>();
		for (int i=0; i< _obiettivi.Count && !gameover; i++)
		{
			gameover|=_obiettivi[i].GoalReached();
			if (_obiettivi[i].GoalReached())
				winners.Add(_obiettivi[i].Player);
		}

		if (gameover && GoalReached != null)
			GoalReached(winners);

	}


	#region Random Getters
	private object randomInt(bool uniq, int min, int max)
	{
		if (! _usedValues.ContainsKey("int"))
			_usedValues["int"]=new List<object>();

		int res;
		do
		{
			res = UnityEngine.Random.Range(min, max);
		}
		while (uniq && _usedValues["int"].Contains(res));
		_usedValues["int"].Add(res);

		return res;
	}

	private string[] randomContinents(bool uniq, int min, int max)
	{
		string[] continents = MainManager.GetInstance().Continents.ToArray();

		List<string> res = new List<String>();

		if (! _usedValues.ContainsKey("continents"))
			_usedValues["continents"]=new List<object>();

		do
		{
			int n = UnityEngine.Random.Range(min, max);
			for (int i=0; i<n;i++)
			{
				string c;
				do
				{
					c=continents[UnityEngine.Random.Range(0, continents.Length)];
				}
				while (res.Contains(c));
				res.Add(c);
			}

		}
		while (uniq && FindArray<string>(res.ToArray(), _usedValues["continents"]));
		_usedValues["continents"].Add(res.ToArray());

		return res.ToArray();

	}

	private string randomPlayerName(bool uniq, int min, int max)
	{
//		string[] names = {"tizio", "caio", "pipponio"};
		string[] names =MainManager.GetInstance().PlayerNames.ToArray();

		if (!_usedValues.ContainsKey("player"))
			_usedValues["player"]= new List<object>();

		string res;
		do
		{
			res = names[UnityEngine.Random.Range(0, names.Length)];
		}
		while (uniq && _usedValues["player"].Contains(res));
		_usedValues["player"].Add (res);

		return res;
	}

	#endregion
	
}
