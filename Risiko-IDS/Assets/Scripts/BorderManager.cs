using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class BorderManager 
{
	//necessaria perché unity ha una storia di non supporto alle tuple, niente di speciale in realtà.
	#region Confine 
	private struct Confine 
	{
		private readonly string[] _stati;

		public Confine(string stato1, string stato2)
		{
			_stati=new string[2];
			_stati[0]=stato1;
			_stati[1]=stato2;
		}

		public string this[int index]
		{
			get
			{
				if (index<0 || index>=2)
					throw new UnityException("no such element");
				else
					return _stati[index];
			}
		}

		public static bool operator == (Confine a, Confine b) //mi fa comodo una cifra! vedi linq sotto
		{
			return (a[0] == b[0] && a[1] == b[1]) || (a[1] == b[0] && a[0] == b[1]);
		}
		public static bool operator != (Confine a, Confine b) //unity dava warning senza questo (ci sta)
		{
			return ! (a==b);
		}

		public override int GetHashCode() //unity dava warning senza questo (poteva farsene una ragione eh)
		{
			return this[0].GetHashCode() ^ this[1].GetHashCode();
		}

		public override bool Equals (object obj) //unity dava warning senza questo (ma in questo caso condivido)
		{
			if (obj is Confine)
				return this == (Confine)obj;
			return false;
		}

	}
	#endregion


	private static readonly List<Confine> _borders = new List<Confine>();

	public BorderManager ()
	{
		string configFilePath=Settings.BorderFile;
		StreamReader sr = new StreamReader(configFilePath);
		string line;
		while ((line=sr.ReadLine())!=null)
		{
			string[] tokens = line.Split(',').Select(s=> s.Trim()).ToArray();
			_borders.Add(new Confine(tokens[0], tokens[1]));
		}
		sr.Close ();

		//Debug.Log (areNeighbours("North_America_03", "South_America_04"));
	}

	public bool areNeighbours (string a, string b) 
	{
		if (a.Equals(b)) // che fare se chiedono se uno stato confina con lui stesso? nel file non ho messo nessun confine del tipo A,A
			return false;

		Confine x = new Confine(a,b);
		IEnumerable<Confine> res=	from c in _borders
									where c==x
		 							select c;
		return res.Count()>0;
	}



	public bool areNeighbours(GameObject a, GameObject b)
	{
		string s1=a.name;
		string s2=b.name;

		return areNeighbours(s1,s2);
	}

	public bool areNeighbours(StatoController c1, StatoController c2)
	{
		string s1 = c1.gameObject.name;
		string s2= c2.gameObject.name;

		return areNeighbours(s1, s2);
	}

}
