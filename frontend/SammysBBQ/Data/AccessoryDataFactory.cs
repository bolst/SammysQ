
using System.Text.Json;

namespace SammysBBQ.Data
{
    public class AccessoryDataFactory : AbsSingleton<AccessoryDataFactory>
    {
        public JsonElement DefaultCoalData()
        {
            string strData =
            @"{
            ""text"": {
                        ""data"": ""Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description Coal Description""
                    },
                    ""image 1"": {
                        ""data"": ""img/bush-coal.jpg""
                    },
                    ""image 2"": {
                        ""data"": ""img/bush-coal-closeup.jpg""
                    },
                    ""link title"": {
                        ""data"": ""Bog Line Bush Coal Premium Charcoal""
                    },
                    ""link destination"": {
                        ""data"": ""https://www.instagram.com/reel/C4jlxpAugNS/""
                    }
            }";

            JsonDocument doc = JsonDocument.Parse(strData);
            return doc.RootElement;
        }

        public JsonElement DefaultWoodData()
        {
            string strData =
            @"{
                  ""text"": {
                        ""data"": ""Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description Wood Description""
                    },
                    ""image 1"": {
                        ""data"": ""img/wood-bagged1.jpg""
                    },
                    ""image 2"": {
                        ""data"": ""img/wood-bagged2.jpg""
                    },
                    ""image 3"": {
                        ""data"": ""img/wood-closeup1.jpg""
                    },
                    ""image 4"": {
                        ""data"": ""img/wood-closeup2.jpg""
                    },
                    ""link title"": {
                        ""data"": ""Matt's Smokin' Firewood - Premium Smoking Wood""
                    },
                    ""link destination"": {
                        ""data"": ""https://www.mattswood.online/""
                    }
            }";

            JsonDocument doc = JsonDocument.Parse(strData);
            return doc.RootElement;
        }
    }
}
