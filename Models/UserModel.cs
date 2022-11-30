using Task_7.Data;

namespace Task_7.Models
{
    public class UserModel
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public int Room { get; set; }
        public Role Role { get; set; }
        public int Roll { get; set; } = -1;

        public UserModel(string connectionId, string name, int room, Role role)
        {
            ConnectionId = connectionId;
            Name = name;
            Room = room;
            Role = role;
        }
    }
}
