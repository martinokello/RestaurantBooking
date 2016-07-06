using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestaurantBooking.Startup))]
namespace RestaurantBooking
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
