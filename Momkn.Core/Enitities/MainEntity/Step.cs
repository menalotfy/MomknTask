using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Momkn.Core.Identity;

namespace Momkn.Core.Enitities.MainEntity
{
    public class Step : BaseEntity
    {
        public Step()
        {
            Items = new HashSet<Item>();
         
   
        }
        public string Name { get; set; }
        public int Number { get; set; }
 
        public ICollection<Item> Items { get; set; }

    }
}
