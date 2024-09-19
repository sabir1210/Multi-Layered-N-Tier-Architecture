using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.DataObject.Entities
{
    public class MyTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Details { get; set; }
        public bool IsCompleted { get; set; }
        public Guid GoalId { get; set; }
        public Goal Goal { get; set; }  // Navigation property
        // Other properties
        public ICollection<TaskCompleteRecord>? TaskCompleteRecords { get; set; } = null;
    }
}
