namespace learningASP.NET_CORE.ViewModels
{
    public record ChatRequestVM(string Prompt, string ConnectionId)
    {
        public string Prompt { get; set; }
        public string ConnectionId { get; set; }
    }
}
