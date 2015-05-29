public interface IRegistration
{

    void Register();

    void Unregister();

    string PhaseName { get; }
}
