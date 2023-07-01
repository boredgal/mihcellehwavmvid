namespace Mihcelle.Hwavmvid.Modules.Blackjack
{

    public class Programstartup : Mihcelle.Hwavmvid.Programinterface
    {

        public void Configure(IServiceCollection services)
        {

            services.AddScoped<Mihcelle.Hwavmvid.Modules.Blackjack.Applicationdbcontext, Mihcelle.Hwavmvid.Modules.Blackjack.Applicationdbcontext>();
            services.AddScoped<Mihcelle.Hwavmvid.Modules.Blackjack.BlackjackService, Mihcelle.Hwavmvid.Modules.Blackjack.BlackjackService>();

        }

        public void Configureapp(WebApplication application)
        {

        }

    }
}
