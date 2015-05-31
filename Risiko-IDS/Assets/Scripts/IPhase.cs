public interface IPhase
{

    void Register();

    void Unregister();

    string PhaseName { get; }
}
