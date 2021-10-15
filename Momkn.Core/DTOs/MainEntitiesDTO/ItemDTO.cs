using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Momkn.Core.DTOs.MainEntitiesDTO
{
    public class ItemDTO
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int StepID { get; set; }
    }
}
