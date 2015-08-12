﻿// Default deny for the win

public class SelectDefault : SelectManager
{
	private static SelectDefault _instance;

	public static SelectDefault GetInstance()
	{
		if (_instance==null)
			_instance=new SelectDefault();
		return _instance;
	}

	protected override bool IsAValidFirst(StatoController s)
	{
		return false;
	}
	
	protected override bool IsAValidSecond(StatoController s1, StatoController s2)
	{
		return false;
	}


}



