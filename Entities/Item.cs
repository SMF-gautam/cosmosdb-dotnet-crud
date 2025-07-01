using System.Text.Json.Serialization;

namespace CosmosDB.Entities
{
    public class Item
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
