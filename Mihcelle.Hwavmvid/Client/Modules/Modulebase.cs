using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Mihcelle.Hwavmvid.Client.Modules.Admin;
using Mihcelle.Hwavmvid.Client.Pages;
using Mihcelle.Hwavmvid.Modal;
using Mihcelle.Hwavmvid.Shared.Constants;
using Mihcelle.Hwavmvid.Shared.Models;
using System.Net.Http.Json;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Mihcelle.Hwavmvid.Client.Modules
{
    public class Modulebase : ComponentBase
    {


        [Inject] public IServiceProvider iserviceprovider { get; set; }
        [Inject] public IHttpClientFactory ihttpclientfactory { get; set; }
        [Inject] public NavigationManager navigationmanager { get; set; }
        [Inject] public Applicationprovider applicationprovider { get; set; }
        [Inject] public Modalservice modalservice { get; set; }

        [Parameter] public string Moduleid { get; set; }
        [Parameter] public Type Componenttype { get; set; }
        [Parameter] public string Containertype { get; set; }
        [Parameter] public Type Componentsettingstype { get; set; }

        protected Moduleservice<Modulepreferences> moduleservice { get; set; }
        protected Dictionary<string, object> servpara { get; set; }

        protected override Task OnInitializedAsync()
        {
            
            Modulepreferences modulepreferences = new Modulepreferences();
            this.moduleservice = new Moduleservice<Modulepreferences>();
            this.moduleservice.Preferences = new Modulepreferences();
            this.moduleservice.Preferences.ModuleId = this.Moduleid;

            this.servpara = new Dictionary<string, object>();
            servpara.Add("Moduleparams", this.moduleservice);

            return base.OnInitializedAsync();
        }

        public async Task Setcontainertype()
        {

            string targetcontainertype = string.Empty;

            if (this.Containertype == Applicationcontainertype.Boxed)
                targetcontainertype = "container-fluid";

            if (this.Containertype == Applicationcontainertype.Fullwidth)
                targetcontainertype = "container";

            var client = this.ihttpclientfactory?.CreateClient("Mihcelle.Hwavmvid.ServerApi.Unauthenticated");
            await client.GetAsync(string.Concat("api/container/", this.applicationprovider._contextcontainer.Id, "/", targetcontainertype));
            this.navigationmanager.NavigateTo(this.navigationmanager.Uri, true);
        }

        public string adminmodalelementid { get; set; } = "admin_modal_element_id_for_settings";
        public async Task Openmodulesettings()
        {

            await this.modalservice.ShowModal(this.adminmodalelementid);
            await InvokeAsync(() =>
            {
                this.StateHasChanged();
            });
        }

        public async Task Deletemodule(string moduleid)
        {
            var client = this.ihttpclientfactory?.CreateClient("Mihcelle.Hwavmvid.ServerApi.Unauthenticated");
            await client.DeleteAsync(string.Concat("api/module/", moduleid));
            this.navigationmanager.NavigateTo(this.navigationmanager.Uri, true);
        }

    }
}
