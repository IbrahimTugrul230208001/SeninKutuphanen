namespace learningASP.NET_CORE.Services
{
    public interface IAIServices
    {
       Task GetMessageStreamAsync(string prompt, string connectionId, CancellationToken? cancellationToken = default!);

    }
}
