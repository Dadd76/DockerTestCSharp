using BlazingPizza;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class OrderUpdatesHub : Hub
{
    public async Task SendOrderUpdate(OrderWithStatus order)
    {
        await Clients.All.SendAsync("ReceiveOrderUpdate", order);
    }
}