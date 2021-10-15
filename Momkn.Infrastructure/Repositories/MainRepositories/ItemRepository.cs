using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Momkn.Core.Enitities.MainEntity;

using Momkn.Core.Interfaces.MainInterface;
using Momkn.Infrastructure.Data;

namespace Momkn.Infrastructure.Repositories.MainRepositories
{
 
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private readonly MomknDbContext _dbContext;
        public ItemRepository(MomknDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Item> GetAllItemsStep(int stepID)
        {
            return _dbContext.Items.Where(s => s.StepID == stepID).ToList();
        }
    }
}
