public class PhaseManager : IManager
{
    private Giocatore currentPlayer;

    public PhaseManager()
    {

    }

    public Giocatore CurrentPlayer
    {
        get
        {
            return this.currentPlayer;
        }
    }
}