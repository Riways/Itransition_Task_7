using Microsoft.AspNetCore.Authorization.Infrastructure;
using Task_7.Data;

namespace Task_7.Models
{
    public class GameStateModel
    {
        public FieldState[][] GameState { get; set; }

        public GameStateModel() {
            GameState = new FieldState[3][]{
                new FieldState[] { FieldState.Empty, FieldState.Empty, FieldState.Empty },
                new FieldState[] { FieldState.Empty, FieldState.Empty, FieldState.Empty },
                new FieldState[] { FieldState.Empty, FieldState.Empty, FieldState.Empty } };
        }
        
    }
}
