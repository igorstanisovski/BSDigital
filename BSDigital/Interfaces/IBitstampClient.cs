namespace BSDigital.Interfaces
{
    public interface IBitstampClient
    {
        Task ConnectAsync();
        Task DisconnectAsync();

        event Action<string> OnMessage;
    }
}
