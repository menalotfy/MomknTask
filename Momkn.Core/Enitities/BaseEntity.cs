using System;
using System.Collections.Generic;
using System.Text;
using Momkn.Core.Identity;

namespace Momkn.Core.Enitities
{
    public abstract class BaseEntity
    {
        public int ID { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string CreatedByNameId { get; set; }
        public string ModifiedByNameId { get; set; }
        public ApplicationUser CreatedByName { get; set; }
        public ApplicationUser ModifiedByName { get; set; }

        public bool ShouldSerializeIsDeleted()
        {
            return false;
        }
        public bool ShouldSerializeModifiedDate()
        {
            return false;
        }
        public bool ShouldSerializeCreatedByNameId()
        {
            return false;
        }
        public bool ShouldSerializeModifiedByNameId()
        {
            return false;
        }
    }
}
