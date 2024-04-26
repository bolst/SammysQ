using System.Text.Json.Serialization;

namespace SammysBBQ.Data
{

    public class MenuContent
    {
        public MenuContentRoot title { get; set; }
        public MenuItemContentRoot items { get; set; }
    }

    public class MenuItemContent
    {
        [JsonPropertyName("ItemName")]
        public string ItemName { get; set; }
        [JsonPropertyName("ItemImagePath")]
        public string? ItemImagePath { get; set; }
        [JsonPropertyName("Description")]
        public string? Description { get; set; }
    }

    public class MenuContentRoot
    {
        public string data { get; set; }
    }

    public class MenuItemContentRoot
    {
        public List<MenuItemContent> data { get; set; }
    }

}