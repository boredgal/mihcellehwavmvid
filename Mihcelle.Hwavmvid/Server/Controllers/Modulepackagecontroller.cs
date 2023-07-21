using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mihcelle.Hwavmvid.Modules.Htmleditor;
using Mihcelle.Hwavmvid.Shared.Models;

namespace Mihcelle.Hwavmvid.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Modulepackagecontroller : ControllerBase
    {

        public Applicationdbcontext applicationdbcontext { get; set; }
        public IServiceProvider servicescopefactory { get; set; }


        public Modulepackagecontroller(Applicationdbcontext applicationdbcontext, IServiceProvider servicescopefactory)
        {
            this.applicationdbcontext = applicationdbcontext;
            this.servicescopefactory = servicescopefactory;
        }

        [AllowAnonymous]
        [HttpGet("packageextensions")]
        public async Task<List<Applicationmodulepackage>> packageextensions()
        {
            var items = await this.applicationdbcontext.Applicationmodulepackages.Where(item => !string.IsNullOrEmpty(item.Name)).ToListAsync();
            return items.OrderBy(item => item.Name).ToList();
        }

        [AllowAnonymous]
        [HttpGet("frameworkpackages")]
        public async Task<List<Applicationmodulepackage>?> frameworkpackages()
        {

            try
            {
                List<Applicationmodulepackage> packages = new List<Applicationmodulepackage>();
                var installeritems = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(assemblytypes => (typeof(IModuleinsideframeworkinstallerinterface)).IsAssignableFrom(assemblytypes));
                
                foreach (var item in installeritems)
                {
                    if (item.IsClass)
                    {
                        var moduleframeworkinstaller = (IModuleinsideframeworkinstallerinterface?)this.servicescopefactory.GetService(item);
                        if (moduleframeworkinstaller != null)
                        {
                            var package = moduleframeworkinstaller.applicationmodulepackage;
                            packages.Add(package);
                        }
                    }
                }

                var orderedpackages = packages.OrderBy(item => item.Name).ToList();
                return orderedpackages;
            }
            catch (Exception exception) { return null; }

        }

    }
}
