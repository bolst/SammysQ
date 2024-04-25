
using System.Diagnostics;

namespace SammysBBQ.Data
{
    public class ImageFactory : AbsSingleton<ImageFactory>
    {
        public List<Tuple<string, string>> SpotlightData()
        {
            return new List<Tuple<string, string>>()
            {
                new Tuple<string, string>("img/spotlight/ppork_sandwich.jpeg", "Flavor Fiesta"),
                new Tuple < string, string >("img/spotlight/bacon_crackers.jpeg", "Appetizing Delights"),
                new Tuple < string, string >("img/spotlight/chicken_sausage_app.jpeg", "Mouthwatering Platters"),
                new Tuple < string, string >("img/spotlight/steak_tacos.jpeg", "Delicious Food"),
            };
        }

        public string Logo(bool bkg = true)
        {
            string retval = "img/logo";

            retval += bkg ? ".jpg" : "-nobkg.png";

            return retval;
        }

        public string Square(string s)
        {
            string square = s.Replace(".jpg", "-square.jpg");
            if (File.Exists($"wwwroot/{square}"))
            {
                return square;
            }
            else
            {
                return s;
            }
        }
    }
}
