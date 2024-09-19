using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTier.DataObject.Entities
{
    public class User
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        [AllowNull]
        public ICollection<Goal>? Goals { get; set; } = null;
    }
}
