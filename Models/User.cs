

using System.IO;

namespace Evdoshenko_lab10.Models
{
    public class User
    {

        public int Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Role { get; set; }

        public string Salt { get; set; }

        public ICollection<Progress> Progresses { get; set; }
        public User()
        {
            Progresses = new List<Progress>();
        }
    }
}
