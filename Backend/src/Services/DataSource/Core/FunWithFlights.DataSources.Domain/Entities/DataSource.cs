namespace FunWithFlights.DataSources.Domain.Entities
{
    public sealed class DataSource(string name, string? description, string url)
    {
        public int Id { get; private set; }
        public string Name { get; private set; } = name;
        public string? Description { get; private set; } = description;
        public string Url { get; private set; } = url;

        public void ChangeUrl(string newUrl)
        {
            if (string.IsNullOrWhiteSpace(newUrl))
            {
                throw new ArgumentException($"'{nameof(newUrl)}' cannot be null or whitespace.", nameof(newUrl));
            }

            if (!Url.Equals(newUrl, StringComparison.OrdinalIgnoreCase))
            {
                Url = newUrl;
                
                // publish event
            }
        }

        // TODO: implement other update methods once they need
    }
}
