using System;
using System.ComponentModel.DataAnnotations;

namespace Recipes.Domain.Entities.Generic
{
    public abstract class AuditableEntity
    {
        public int Id { get; set; }

        [Required]
        public string CreatedByUserId { get; set; }

        public string LastModifiedByUserId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
    }
}
