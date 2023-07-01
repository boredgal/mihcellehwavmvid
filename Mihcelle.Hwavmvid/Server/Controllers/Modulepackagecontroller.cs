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
        public Modulepackagecontroller(Applicationdbcontext applicationdbcontext)
        {
            this.applicationdbcontext = applicationdbcontext;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<List<Applicationmodulepackage>> Get()
        {
            var items = await this.applicationdbcontext.Applicationmodulepackages.Where(item => !string.IsNullOrEmpty(item.Name)).ToListAsync();
            return items.OrderBy(item => item.Name).ToList();
        }

    }
}
