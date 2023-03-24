using Microsoft.EntityFrameworkCore;

namespace Evdoshenko_lab10.Models
{
    public class Achievement
    {

        public int Id { get; set; }

        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

    }
}