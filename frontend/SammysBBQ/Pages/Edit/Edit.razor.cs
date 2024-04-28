using System.Text.Json;
using Microsoft.AspNetCore.Components;
using SammysBBQ.Data;
using SammysBBQ.Pages.Menu.Components;

namespace SammysBBQ.Pages.Edit
{
    using StrDataType = List<Dictionary<List<string>,string>>;
    using MenuDataType = List<Dictionary<List<string>, List<MenuItemContent>>>;

    public partial class Edit
    {

        [Inject] protected NavigationManager NavManager { get; set; } = default!;

        private JsonDocument? Content = null;
        private StrDataType StrEditableData = new();
        private StrDataType ImgEditableData = new();
        private MenuDataType MenuEditableData = new();

        private List<string> AvailableImages = new();

        protected override async Task OnInitializedAsync()
        {
            Content = await ApiDataFactory.Instance.Get(new List<string>() { "root" });

            AvailableImages = ImageFactory.Instance.AllImages();

            if (Content != null) ParseContent(Content);
        }

        void ParseContent(JsonDocument doc)
        {
            foreach(JsonProperty node in doc.RootElement.EnumerateObject())
            {
                ParseContent(new List<string> { node.Name }, node);
            }
        }

        void ParseContent(List<string> breadcrumb, JsonProperty node)
        {

            if (node.Name.Equals("menus"))
            {
                ParseMenuData(breadcrumb, node);
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
                        ParseContent(tb, inode);
                    }
                }
                catch(Exception exc)
                {
                }
            }
        }

        void ParseMenuData(List<string> breadcrumb, JsonProperty node)
        {
            int index = 0;
            foreach(JsonElement menuNode in node.Value.EnumerateArray())
            {
                var ttb = new List<string>(breadcrumb)
                {
                    index.ToString(),
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
                    index.ToString(),
                    "items"
                };
                var menuDataToAdd = new Dictionary<List<string>, List<MenuItemContent>> { { tmb, menuItemsToAdd } };
                MenuEditableData.Add(menuDataToAdd);

                ++index;
            }
        }

        #region Add Menu

        private bool AddMenuIsClicked = false;
        private string NewMenuTitle { get; set; } = "";

        Task OnClickedConfirmAddMenu()
        {
            AddMenuIsClicked = true;
            return Task.CompletedTask;
        }

        async Task OnClickedAddMenu()
        {
            int index = MenuEditableData.Count();

            // add menu title to data
            var titleBreadcrumb = new List<string>
            {
                "menu",
                "menus",
                index.ToString(),
                "title"
            };
            var titleDataToAdd = new Dictionary<List<string>, string> { { titleBreadcrumb, NewMenuTitle } };
            StrEditableData.Add(titleDataToAdd);

            await UpdateData(titleBreadcrumb, NewMenuTitle);


            // add empty menu to data
            var breadcrumb = new List<string>
            {
                "menu",
                "menus",
                index.ToString(),
                "items"
            };
            var emptyMenuItems = new List<MenuItemContent> { new MenuItemContent
            {
                ItemName = "",
                ItemImagePath = "",
                Description = "",
            } };
            var menuDataToAdd = new Dictionary<List<string>, List<MenuItemContent>> { { breadcrumb, emptyMenuItems } };
            MenuEditableData.Add(menuDataToAdd);

            await UpdateData(breadcrumb, emptyMenuItems);

            AddMenuIsClicked = false;
            StateHasChanged();
            NavManager.NavigateTo("/edit", true);
        }

        #endregion
        async Task UpdateData(List<string> Breadcrumb, List<MenuItemContent> Data)
        {
            List<string> b = new List<string>(Breadcrumb);
            b.Insert(0, "root");
            foreach (MenuItemContent content in Data)
            {
                if (content.ItemImagePath.Equals("None"))
                    content.ItemImagePath = content.ItemImagePath.Replace("None", "");
            }
            await ApiDataFactory.Instance.Set(Data, b);
        }

        async Task UpdateData(List<string> Breadcrumb, string Data)
        {
            List<string> b = new List<string>(Breadcrumb);
            b.Insert(0, "root");
            await ApiDataFactory.Instance.Set(Data, b);
        }

    }

}