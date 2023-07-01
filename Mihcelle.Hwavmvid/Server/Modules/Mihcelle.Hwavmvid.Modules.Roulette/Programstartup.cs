using Mihcelle.Hwavmvid.Modules.Roulette;

namespace Mihcelle.Hwavmvid.Modules.Roulette
{

    public class Programstartup : Mihcelle.Hwavmvid.Programinterface
    {

        public void Configure(IServiceCollection services)
        {

            services.AddScoped<Applicationdbcontext, Applicationdbcontext>();

        }

        public void Configureapp(WebApplication application)
        {

        }

    }
}
