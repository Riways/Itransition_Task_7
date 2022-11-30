using Task_7.Data;
using Task_7.Models;

namespace Task_7.Services
{
    public interface IGameService
    {
        public GameStateModel GetGameStateByRoom( int roomId);
        public List<Role> GetAvaiableRolesInRoom(int roomId );
        public FieldState ConvertRoleToFieldState(Role role);
        public UserModel? GetOpponent(UserModel client);
        public void ChangeGameStateInRoom(int roomId, GameStateModel gameState);
        public void ChangeAvaiableRolesInRoom(int roomId, Role role, bool state );
        public void InitRoom(int roomId );
        public void DropRoom(int roomId);
        public void RemoveUserFromGame( UserModel user);
        public void ResetRoomState(int roomId);
        public void AddUserToTheRoom( UserModel client);
        public bool IsWin(GameStateModel gameState);
        public bool IsDraw(GameStateModel gameState);
    }

    public class GameService: IGameService
    {
        private static readonly int AMOUNT_OF_ROOMS = 1;
        private static readonly int PLAYERS_IN_ROOM = 2;
        private static Dictionary<int, GameStateModel> _gameStateInRoom { get; set; } = new();
        private static Dictionary<int, Dictionary<Role, bool>> _availableRolesInRoom { get; set; } = new();
        private static Dictionary<int, List<UserModel>> _playersInRoom { get; set; } = new(); 

        public GameService() {
            for (var i = 0; i<AMOUNT_OF_ROOMS; i++)
            {
                InitRoom(i);
            }
        }

        public void InitRoom(int roomId)
        {
            _gameStateInRoom.Add(roomId, new GameStateModel());
            _availableRolesInRoom.Add(roomId,new Dictionary<Role, bool>());
            _availableRolesInRoom[roomId].Add(Role.X, true);
            _availableRolesInRoom[roomId].Add(Role.O, true);
            _availableRolesInRoom[roomId].Add(Role.Observer, true);
            _playersInRoom.Add(roomId, new List<UserModel>(PLAYERS_IN_ROOM));
        }

        public void DropRoom(int roomId)
        {
            _playersInRoom[roomId] = new List<UserModel>();
            ResetRoomState(roomId);
        }

        public void ResetRoomState(int roomId)
        {
            _gameStateInRoom[roomId] = new GameStateModel();
        }

        public void RemoveUserFromGame( UserModel user)
        {
            _playersInRoom[user.Room].Remove(user);
            ChangeAvaiableRolesInRoom(user.Room, user.Role, true);
        }

        public void AddUserToTheRoom(UserModel client)
        {
            _playersInRoom[client.Room].Add(client);
        }

        public UserModel? GetOpponent(UserModel client)
        {
            return  _playersInRoom[client.Room].Where(u => !u.ConnectionId.Equals(client.ConnectionId)).FirstOrDefault();
        }

        public List<Role> GetAvaiableRolesInRoom(int roomId)
        {
            Dictionary<Role, bool> rolesAndAvailability = _availableRolesInRoom[roomId];
            return rolesAndAvailability.Where((pair) => pair.Value).Select(pair => pair.Key).ToList();
        }

        public GameStateModel GetGameStateByRoom(int roomId)
        {
            return _gameStateInRoom[roomId];
        }

        public void ChangeAvaiableRolesInRoom(int roomId, Role role, bool state)
        {
            _availableRolesInRoom[roomId][role] = state;
        }

        public void ChangeGameStateInRoom(int roomId, GameStateModel gameState)
        {
            _gameStateInRoom[roomId] = gameState;
        }

        public bool IsWin(GameStateModel gameState)
        {
            FieldState winnerState = CheckDiagonal(gameState);
            if (winnerState != FieldState.Empty)
                return true;
            for (var i = 0; i < 3; i++)
            {
                FieldState firstFieldInRow = gameState.GameState[i][0];
                FieldState firstFieldInColumn = gameState.GameState[0][i];
                if (firstFieldInRow == FieldState.Empty && firstFieldInColumn ==FieldState.Empty)
                    continue;
                if (gameState.GameState[i].All((state) => firstFieldInRow.Equals(state)))
                {
                    winnerState = firstFieldInRow;
                    break;
                }
                FieldState secondFieldInColumn = gameState.GameState[1][i];
                FieldState thirdFieldInColumn = gameState.GameState[2][i];
                if (firstFieldInColumn == FieldState.Empty)
                    continue;
                if (secondFieldInColumn == firstFieldInColumn && thirdFieldInColumn == firstFieldInColumn) { 
                    winnerState = firstFieldInColumn;
                    break;
                }
            }
            if (winnerState != FieldState.Empty)
                return true;
            return false;
        }

        public bool IsDraw(GameStateModel gameState)
        {
            if (gameState.GameState.All(edge => edge.All(state => state != FieldState.Empty)))
            {
                return true;
            }
            return false;
        }

        public FieldState ConvertRoleToFieldState(Role role)
        {
            switch (role)
            {
                case Role.X:
                    return FieldState.X;
                case Role.O:
                    return FieldState.O;
                default:
                    return FieldState.Empty;
            }
        }

        private FieldState CheckDiagonal(GameStateModel gameState)
        {
            FieldState middleFieldValue = gameState.GameState[1][1];
            FieldState tlFieldValue = gameState.GameState[0][0];
            FieldState trFieldValue = gameState.GameState[0][2];
            FieldState blFieldValue = gameState.GameState[2][0];
            FieldState brFieldValue = gameState.GameState[2][2];
            if ((tlFieldValue == middleFieldValue && brFieldValue == middleFieldValue) ||
                (trFieldValue == middleFieldValue && blFieldValue == middleFieldValue))
                return middleFieldValue;
            return FieldState.Empty;
        }
    }
}
