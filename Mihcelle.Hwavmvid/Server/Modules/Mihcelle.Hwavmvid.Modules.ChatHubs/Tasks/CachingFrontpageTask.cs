﻿using System;
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
    public class CachingFrontpageTask : Hostedservicebase, IHostedservicebase
    {


        private IServiceScopeFactory iservicescopefacotry { get; set; }
        private Mihcelle.Hwavmvid.Modules.ChatHubs.Applicationdbcontext moduleapplicationdbcontext { get; set; }
        private ChatHubService chathubservice { get; set; }
        public int Interval { get; set; } = 17000;

        public CachingFrontpageTask(IServiceScopeFactory servicescopefactory) : base(servicescopefactory)
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

            var scope = this.servicescopefactory.CreateScope();
            this.moduleapplicationdbcontext = scope.ServiceProvider.GetService<Mihcelle.Hwavmvid.Modules.ChatHubs.Applicationdbcontext>();
            this.chathubservice = scope.ServiceProvider.GetService<ChatHubService>();

            var chathubmoduleassemblyname = "Mihcelle.Hwavmvid.Modules.ChatHubs.Index, Mihcelle.Hwavmvid.Client";
            var chathubmoduleitems = await frameworkapplicationdbcontext.Applicationmodules.Where(item => item.Assemblytype == chathubmoduleassemblyname).ToListAsync();
            foreach (var module in chathubmoduleitems)
            {
                var items = chathubservice.GetRooms(1, Oqtane.ChatHubs.Constants.ChatHubConstants.FrontPageItems, module.Id, true).GetAwaiter().GetResult();
            }
        }
    }
}