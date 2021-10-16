using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Momkn.Core.Enitities.MainEntity;

using Momkn.Core.Interfaces.MainInterface;
using Momkn.Infrastructure.Data;

namespace Momkn.Infrastructure.Repositories.MainRepositories
{
 
    public class StepRepository : Repository<Step>, IStepRepository
    {
        private readonly MomknDbContext _dbContext;
        public StepRepository(MomknDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
            
        }
        public List<Step> getAllSteps()
        {
            return _dbContext.Steps.Where(x=>x.IsDeleted!=true).Include(x => x.Items.Where(x=>x.IsDeleted!=true)).ToList();
         }

    }
}
