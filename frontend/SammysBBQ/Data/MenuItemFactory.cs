
namespace SammysBBQ.Data
{
    public class MenuItemFactory : AbsSingleton<MenuItemFactory>
    {
        public List<MenuItemContent> DefaultBBQMenuItems()
        {
            MenuItemContent Brisket = new MenuItemContent()
            {
                ItemName = "Brisket",
                ItemImagePath = ImageSources.Brisket,
                Description = null
            };
            MenuItemContent PulledPork = new MenuItemContent()
            {
                ItemName = "Pulled Pork",
                ItemImagePath = ImageSources.PulledPorkSandwich,
                Description = null
            };
            MenuItemContent Lamb = new MenuItemContent()
            {
                ItemName = "Lamb",
                ItemImagePath = ImageSources.PulledLambCut1,
                Description = null
            };
            MenuItemContent Beef = new MenuItemContent()
            {
                ItemName = "Beef",
                ItemImagePath = ImageSources.SteakTacos,
                Description = null
            };
            MenuItemContent Chicken = new MenuItemContent()
            {
                ItemName = "Chicken",
                ItemImagePath = ImageSources.Chicken,
                Description = null
            };
            MenuItemContent BeefPorkRibs = new MenuItemContent()
            {
                ItemName = "Beef/Pork Ribs",
                ItemImagePath = ImageSources.StLouisRibs1,
                Description = null
            };
            MenuItemContent Fish = new MenuItemContent()
            {
                ItemName = "Fish (Rainbow Trout, Salmon)",
                ItemImagePath = ImageSources.MapleChipotleRainbowTrout,
                Description = null
            };

            return new List<MenuItemContent>
            {
                Brisket,
                PulledPork,
                Lamb,
                Beef,
                Chicken,
                BeefPorkRibs,
                Fish,
            };
        }
    }
}
