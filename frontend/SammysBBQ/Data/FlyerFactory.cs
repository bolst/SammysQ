
namespace SammysBBQ.Data
{
    public class FlyerFactory : AbsSingleton<FlyerFactory>
    {
        public string CateringFlyer() { return "img/catering-flyer.jpg"; }
        public string MainFlyer() { return "img/sammys-flyer.jpg"; }
    }
}
