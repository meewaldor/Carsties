using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers
{
    public class BidPlacedConsumer : IConsumer<BidPlace>
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        public BidPlacedConsumer(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }
        public async Task Consume(ConsumeContext<BidPlace> context)
        {
            Console.WriteLine("==> bid placed message received");
            await _hubContext.Clients.All.SendAsync("BidPlaced", context.Message);
        }
    }
}
