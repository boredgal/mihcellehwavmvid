using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Mihcelle.Hwavmvid.Server;
using Mihcelle.Hwavmvid.Shared.Models;

namespace Mihcelle.Hwavmvid.Modules.Htmleditor
{

    public class Applicationdbcontext : Mihcelle.Hwavmvid.Server.Data.Applicationdbcontext, IModuleinstallerinterface
    {

        public DbSet<Applicationhtmleditor> Applicationhtmleditors { get; set; }

        public Applicationdbcontext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            try { base.OnModelCreating(builder); } catch { }
        }

        public async Task Removemodule(string moduleid)
        {
            await this.Applicationhtmleditors.Where(item => item.Moduleid == moduleid).ExecuteDeleteAsync();
            await this.SaveChangesAsync();            
        }

        public async Task Install()
        {
            try
            {
                await this.Database.MigrateAsync();
            }
            catch (Exception message)
            {
                Console.WriteLine(message);
            }
        }

        public async Task Deinstall()
        {
            await this.Database.RollbackTransactionAsync();
        }
        
        public Applicationmodulepackage applicationmodulepackage 
        {
            get
            {
                var package = new Applicationmodulepackage()
                {
                    Name = "Htmleditor",
                    Version = "1.0.0",
                    Icon = "I",
                    Assemblytype = "Mihcelle.Hwavmvid.Modules.Htmleditor.Index, Mihcelle.Hwavmvid.Client",
                    Settingstype = "Mihcelle.Hwavmvid.Modules.Htmleditor.Settings, Mihcelle.Hwavmvid.Client",
                    Description = string.Empty,
                    Createdon = DateTime.Now,
                };

                return package;
            }
        }

    }
}
