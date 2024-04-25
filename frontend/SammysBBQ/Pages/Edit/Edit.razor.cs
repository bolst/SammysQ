using System.Text.Json;
using SammysBBQ.Data;

namespace SammysBBQ.Pages.Edit
{
    using StrDataType = List<Dictionary<List<string>,string>>;
    using MenuDataType = List<Dictionary<List<string>, List<MenuItemContent>>>;

    public partial class Edit
    {


        private JsonDocument? Content = null;
        private StrDataType StrEditableData = new();
        private StrDataType ImgEditableData = new();
        private MenuDataType MenuEditableData = new();

        private List<string> AvailableImages = new();

        protected override async Task OnInitializedAsync()
        {
            Content = await ApiDataFactory.Instance.Get(new List<string>() { "root" });

            AvailableImages = ImageFactory.Instance.AllImages();

            if (Content != null) EnumerateContent(Content);
        }

        void EnumerateContent(JsonDocument doc)
        {
            foreach(JsonProperty node in doc.RootElement.EnumerateObject())
            {
                EnumerateContent(new List<string> { node.Name }, node);
            }
        }

        void EnumerateContent(List<string> breadcrumb, JsonProperty node)
        {

            if (node.Name.Equals("menus"))
            {
                EnumerateMenus(breadcrumb, node);
            }

            JsonElement n;
            if (node.Value.TryGetProperty("data", out n))
            {
                // breadcrumb.Add("data");

                // if node is string
                if (node.Value.GetProperty("type").ToString().Equals("string"))
                {
                    var dataToAdd = new Dictionary<List<string>, string> { { breadcrumb, n.ToString() } };
                    StrEditableData.Add(dataToAdd);
                }
                // if node is a path to an image
                if (node.Value.GetProperty("type").ToString().Equals("imagepath"))
                {
                    var dataToAdd = new Dictionary<List<string>, string> { { breadcrumb, n.ToString() } };
                    ImgEditableData.Add(dataToAdd);
                }
                // if node is array
                else if (node.Value.GetProperty("type").ToString().Equals("list"))
                {
                    return;
                    List<MenuItemContent> listDataToAdd = new List<MenuItemContent> { };
                    foreach (JsonElement menuItem in n.EnumerateArray())
                    {
                        listDataToAdd.Add(new MenuItemContent
                        {
                            ItemName = menuItem.GetProperty("ItemName").ToString(),
                            ItemImagePath = menuItem.GetProperty("ItemImagePath").ToString(),
                            Description = menuItem.GetProperty("Description").ToString()
                        });
                    }
                    MenuEditableData.Add(new Dictionary<List<string>, List<MenuItemContent>> { { breadcrumb, listDataToAdd } });
                }
            }
            else
            {
                try
                {
                    foreach (JsonProperty inode in node.Value.EnumerateObject())
                    {
                        List<string> tb = new List<string>(breadcrumb)
                        {
                            inode.Name
                        };
                        EnumerateContent(tb, inode);
                    }
                }
                catch(Exception exc)
                {
                }
            }
        }

        void EnumerateMenus(List<string> breadcrumb, JsonProperty node)
        {
            foreach(JsonElement menuNode in node.Value.EnumerateArray())
            {
                var ttb = new List<string>(breadcrumb)
                {
                    "title"
                };
                var titleDataToAdd = new Dictionary<List<string>, string> { { ttb, menuNode.GetProperty("title").GetProperty("data").ToString() } };
                StrEditableData.Add(titleDataToAdd);

                List<MenuItemContent> menuItemsToAdd = new();
                foreach(JsonElement menuItem in menuNode.GetProperty("items").GetProperty("data").EnumerateArray())
                {
                    menuItemsToAdd.Add(new MenuItemContent
                    {
                        ItemName = menuItem.GetProperty("ItemName").ToString(),
                        ItemImagePath = menuItem.GetProperty("ItemImagePath").ToString(),
                        Description = menuItem.GetProperty("Description").ToString(),
                    });
                }
                var tmb = new List<string>(breadcrumb)
                {
                    "items"
                };
                var menuDataToAdd = new Dictionary<List<string>, List<MenuItemContent>> { { tmb, menuItemsToAdd } };
                MenuEditableData.Add(menuDataToAdd);
            }
        }

    }
}