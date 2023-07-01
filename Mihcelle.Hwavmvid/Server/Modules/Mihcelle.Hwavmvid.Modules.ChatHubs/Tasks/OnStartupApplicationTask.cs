using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mihcelle.Hwavmvid.Modules.ChatHubs;
using Mihcelle.Hwavmvid.Modules.ChatHubs.Providers;
using Mihcelle.Hwavmvid.Server.Data;
using Mihcelle.Hwavmvid.Server.Tasks;
using Mihcelle.Hwavmvid.Shared.Models;

namespace Mihcelle.Hwavmvid.Modules.ChatHubs.Tasks
{
    public class OnStartupApplicationTask : Hostedservicebase, IHostedservicebase
    {


        private IServiceScopeFactory iservicescopefacotry { get; set; }
        private Mihcelle.Hwavmvid.Modules.ChatHubs.Applicationdbcontext moduleapplicationdbcontext { get; set; }
        private ChatHubService chathubservice { get; set; }
        public int Interval { get; set; } = 17000;
        private bool Taskexecuted { get; set; } = false;

        public OnStartupApplicationTask(IServiceScopeFactory servicescopefactory) : base(servicescopefactory)
        {
            this.iservicescopefacotry = servicescopefactory;
        }

        public override async Task StartAsync(CancellationToken provider)
        {
            await Task.CompletedTask;            
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        public async Task Runtaskimplementation(Hwavmvid.Server.Data.Applicationdbcontext frameworkapplicationdbcontext)
        {

            if (this.Taskexecuted == false)
            {
                this.Taskexecuted = true;
                var scope = this.servicescopefactory.CreateScope();
                this.moduleapplicationdbcontext = scope.ServiceProvider.GetService<Mihcelle.Hwavmvid.Modules.ChatHubs.Applicationdbcontext>();
                this.chathubservice = scope.ServiceProvider.GetService<ChatHubService>();
                this.chathubservice.ArchiveActiveDbItems().GetAwaiter().GetResult();
            }
        }

    }
}