using ERPDAL.ProductionDAL;
using ERPDAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.ProductionDAL
{
    public class ProductionUnitOfWork : IProductionUnitOfWork
    {
        private readonly ProductionDbContext _dbcontext;
        public ProductionUnitOfWork() {
            _dbcontext = new ProductionDbContext();
        }
        public DbContext Db { get { return _dbcontext; } }

        public void Dispose()
        {
            
        }
    }
}
