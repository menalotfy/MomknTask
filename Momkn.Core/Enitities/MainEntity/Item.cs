using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Momkn.Core.Identity;

namespace Momkn.Core.Enitities.MainEntity
{
    public class Item : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int StepID { get; set; }
        public Step Step { get; set; }


    }
}
