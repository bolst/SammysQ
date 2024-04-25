namespace SammysBBQ.Data
{

    public class MenuContent
    {
        public MenuContentRoot title { get; set; }
        public MenuItemContentRoot items { get; set; }
    }

    public class MenuItemContent
    {
        public string ItemName { get; set; }
        public string? ItemImagePath { get; set; }
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