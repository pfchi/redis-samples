using Microsoft.AspNetCore.SignalR;

namespace webapp
{
    /// <summary>
    /// https://learn.microsoft.com/zh-tw/aspnet/core/tutorials/signalr?view=aspnetcore-8.0&tabs=visual-studio
    /// </summary>
    public class ChatRoomSignalrHub:Hub
    {
        public override async Task OnConnectedAsync()
        {
            var ap = Environment.GetEnvironmentVariable("COMPUTERNAME") ?? Environment.GetEnvironmentVariable("APNAME");
            var welcome = $"Welcome to Chatroom @ {ap}";
            await Clients.All.SendAsync("Connected", welcome);
            await base.OnConnectedAsync();
        }
        public async Task SendMessage(string nickname, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", nickname, message);
        }
    }
}
