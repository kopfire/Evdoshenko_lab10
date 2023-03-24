using Microsoft.EntityFrameworkCore;

namespace Evdoshenko_lab10.Models
{
    public class Progress
    {

        public int Id { get; set; }

        public string Semester { get; set; }
        public string Discipline { get; set; }
        public string Grade { get; set; }

        public int? UserId { get; set; }
        public User User { get; set; }

    }
}