using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.DataObject.Entities
{
    public class Goal
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // Other properties
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<MyTask>? MyTasks { get; set; } = null;
    }
}
