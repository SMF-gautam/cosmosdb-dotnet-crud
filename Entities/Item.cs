using Newtonsoft.Json;

namespace CosmosDB.Entities
{
    public class Item
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
