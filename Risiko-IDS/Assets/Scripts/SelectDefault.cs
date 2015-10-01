// Default deny for the win
public abstract partial class SelectManager
{
	private class SelectDefault : SelectManager
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
		
		protected override bool IsAValidSecond(StatoController s2)
		{
			return false;
		}
	}
}

