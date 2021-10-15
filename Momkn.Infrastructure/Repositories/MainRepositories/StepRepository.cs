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
 
    public class StepRepository : Repository<Step>, IStepRepository
    {
        private readonly MomknDbContext _dbContext;
        public StepRepository(MomknDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

    }
}
