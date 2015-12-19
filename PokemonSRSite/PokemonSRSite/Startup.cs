using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PokemonSRSite.Startup))]
namespace PokemonSRSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
