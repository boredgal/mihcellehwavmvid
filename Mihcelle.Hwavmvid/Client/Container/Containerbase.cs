using Microsoft.AspNetCore.Components;
using Mihcelle.Hwavmvid.Shared.Models;
using System.Net.Http.Json;

namespace Mihcelle.Hwavmvid.Client.Container
{
    public class Containerbase : ComponentBase, IDisposable
    {

        [Inject] public IHttpClientFactory ihttpclientfactory { get; set; }
        [Inject] public Applicationprovider applicationprovider { get; set; }
        [Inject] public NavigationManager navigationmanager { get; set; }

        protected override async Task OnInitializedAsync()
        {

            this.applicationprovider._oncontextpagechanged += async () => await this.Contextpagechanged();
            await base.OnInitializedAsync();
        }

        protected async Task Contextpagechanged()
        {

            try
            {

                this.applicationprovider._contextcontainer = null;
                this.applicationprovider._contextcontainercolumns = null;
                this.StateHasChanged();

                await InvokeAsync(async () =>
                {
                    var client = this.ihttpclientfactory?.CreateClient("Mihcelle.Hwavmvid.ServerApi.Unauthenticated");
                    this.applicationprovider._contextcontainer = await client.GetFromJsonAsync<Applicationcontainer?>(string.Concat("api/container/", this.applicationprovider?._contextpage.Id));
                    this.StateHasChanged();
                });

                if (this.applicationprovider._contextcontainer != null)
                {

                    await InvokeAsync(async () =>
                    {
                        var client = this.ihttpclientfactory?.CreateClient("Mihcelle.Hwavmvid.ServerApi.Unauthenticated");
                        this.applicationprovider._contextcontainercolumns = await client.GetFromJsonAsync<List<Applicationcontainercolumn>>(string.Concat("api/containercolumns/", this.applicationprovider?._contextcontainer.Id));
                        this.StateHasChanged();
                    });

                    await Task.Delay(1400).ContinueWith(async (task) =>
                    {
                        if (this.applicationprovider._contextcontainercolumns != null && this.applicationprovider._contextcontainercolumns.Any())
                        {
                            await this.applicationprovider.Initpackagemoduledraganddrop();
                        }
                    });
                    
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        public void Dispose()
        {
            this.applicationprovider._oncontextpagechanged -= async () => await this.Contextpagechanged();
        }

    }
}
