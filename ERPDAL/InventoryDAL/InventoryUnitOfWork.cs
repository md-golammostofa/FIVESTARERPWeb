using ERPDAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.InventoryDAL
{
    public class InventoryUnitOfWork : IInventoryUnitOfWork
    {
        private readonly InventoryDbContext _dbcontext;
        public InventoryUnitOfWork() {
            _dbcontext = new InventoryDbContext();
        }
        public DbContext Db { get { return _dbcontext; } }

        public void Dispose()
        {
            
        }
    }
}
