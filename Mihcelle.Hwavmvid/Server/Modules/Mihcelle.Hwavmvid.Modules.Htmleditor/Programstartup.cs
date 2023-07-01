namespace Mihcelle.Hwavmvid.Modules.Htmleditor
{

    public class Programstartup : Mihcelle.Hwavmvid.Programinterface
    {

        public void Configure(IServiceCollection services)
        {

            services.AddScoped<Mihcelle.Hwavmvid.Modules.Htmleditor.Applicationdbcontext, Mihcelle.Hwavmvid.Modules.Htmleditor.Applicationdbcontext>();

        }

        public void Configureapp(WebApplication application)
        {

        }

    }
}
