using MARN_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MARN_API.Hubs
{
    [Authorize]
    public class NotificationHub : Hub
    {

    }
}
