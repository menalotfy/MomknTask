using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Momkn.Core.Enitities.MainEntity;

namespace Momkn.Core.Interfaces.MainInterface
{
    public interface IItemRepository : IRepository<Item>
    {
        List<Item> GetAllItemsStep(int stepID);
    }
}
