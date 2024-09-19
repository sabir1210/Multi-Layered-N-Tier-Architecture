using NTier.DataObject.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.DataObject.DTOs.Goal
{
    public class GoalDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }

    public class CreateGoalDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        // Other properties
        public Guid UserId { get; set; }
    }

    public class UpdateGoalDTO
    {
        public string Name { get; set; }
        // Add other properties as needed
    }
}
