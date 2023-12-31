﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Oqtane.ChatHubs.Models;

namespace Mihcelle.Hwavmvid.Modules.ChatHubs.Commands
{
    public abstract class AdminCommand : BaseCommand
    {
        public override async Task Execute(CommandServicesContext commandServicesContext, CommandCallerContext commandCallerContext, string[] args, ChatHubUser caller)
        {
            if(!commandServicesContext.ChatHub.Context.User.HasClaim(ClaimTypes.Role, Mihcelle.Hwavmvid.Shared.Constants.Authentication.Anonymousrole))
            {
                throw new HubException("You do not have any permissions to run this command");
            }

            await ExecuteAdminOperation(commandServicesContext, commandCallerContext, args, caller);
        }

        public abstract Task ExecuteAdminOperation(CommandServicesContext context, CommandCallerContext callerContext, string[] args, ChatHubUser caller);
    }
}
