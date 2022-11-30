using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using Task_7.Data;
using Task_7.Models;

namespace Task_7.Hubs
{
    public class GameHub : Hub
    {
        public async Task UserChangedGameState(UserModel user, GameStateModel newState)
        {
            await Clients.All.SendAsync("RecieveNewState", user, newState);
        }
        
        public async Task UserWinningProcedure(UserModel user)
        {
            await Clients.All.SendAsync("RecieveWinner", user);
        }

        public async Task DrawGameProcedure(UserModel user)
        {
            await Clients.All.SendAsync("ReciveDrawResult", user);
        }

        public async Task NewGameStarted(UserModel user)
        {
            await Clients.All.SendAsync("StartNewGame", user);
        }

        public async Task UserHasChosenTheRole(UserModel user)
        {
            await Clients.All.SendAsync("RecieveNewUserRole", user);
        }

        public async Task UserRolled(UserModel user, int roll)
        {
            await Clients.All.SendAsync("RecieveOpponentRoll", user, roll);
        } 

        public async Task UserLeaved(UserModel user)
        {
            await Clients.All.SendAsync("HandleLeavedUserEvent", user);
        }

        public async Task UserConnected(UserModel user)
        {
            await Clients.All.SendAsync("HandleConnectedUserEvent", user);
        }

    }
}
