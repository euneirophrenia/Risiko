public class SelectAttack : SelectManager
{
	private static SelectAttack _instance;

	public static SelectAttack GetInstance()
	{
		if (_instance==null)
			_instance=new SelectAttack();
		return _instance;
	}

	private SelectAttack()
	{
		base.Init();
	}

	protected override bool IsAValidFirst(StatoController s)
	{
		return s.Player.Equals(PhaseManager.GetInstance().CurrentPlayer);
	}

	protected override bool IsAValidSecond(StatoController s2)
	{
		BorderManager border=BorderManager.GetInstance();
		return border.areNeighbours(_stateTemp, s2) && !_stateTemp.Player.Equals(s2.Player);
	}

}
