namespace SuParty.Service
{
    public class ServiceMapping
    {
        public string Interface { get; set; } = string.Empty;
        public string Implementation { get; set; } = string.Empty;
        public string Lifetime { get; set; } = "Scoped"; // 預設 Scoped
    }
}
