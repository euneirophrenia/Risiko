using System;


/* So che esistono già descrittori dei costruttori, ma in questo caso mi serviva aumentare la semantica delle informazioni.
 * Quindi creo l'attributo che incapsuli tutte le informazioni che mi possono servire
 */

[AttributeUsage(AttributeTargets.Constructor, AllowMultiple=false)]

public class ConstructorArgumentsInfo : Attribute
{

	private string _tipo;
	private int _min, _max;
	private bool _unique;


	public ConstructorArgumentsInfo(string tipo)
	{
		this._tipo=tipo;
		this._unique=true;
		this._min=Int32.MinValue;
		this._max=Int32.MaxValue;
	}

	public string Tipo
	{
		get
		{
			return this._tipo;
		}
	}

	public int Min
	{
		get
		{
			return this._min;
		}
		set
		{
			this._min=value;
		}
	}

	public int Max
	{
		get
		{
			return this._max;
		}
		set
		{
			this._max=value;
		}
	}

	public bool IsUnique
	{
		set
		{
			this._unique=value;
		}
		get
		{
			return this._unique;
		}
	}

}
