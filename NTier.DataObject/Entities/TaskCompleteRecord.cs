using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.DataObject.Entities
{
    public class TaskCompleteRecord
    {
        public Guid Id { get; set; }
        public int TaskId { get; set; }
        public DateTime CompletedDate { get; set; }
        public MyTask Task { get; set; }  // Navigation property
        // Other properties
    }
}
