public abstract partial class SelectManager
{
	private class SelectMove : SelectManager
	{

		private static SelectMove _instance;
		
		public static SelectMove GetInstance()
		{
			if (_instance==null)
				_instance=new SelectMove();
			return _instance;
		}

		private SelectMove()
		{
			base.Init(); 
		}

		protected override bool IsAValidFirst(StatoController s)
		{

			return s.Player.Equals(this._currentPlayer) && s.TankNumber>1;
		}
		
		protected override bool IsAValidSecond(StatoController s2)
		{
			BorderManager border=BorderManager.GetInstance();
			return border.areNeighbours(_stateTemp, s2) && _stateTemp.Player.Equals(s2.Player);
		}
	}
}
