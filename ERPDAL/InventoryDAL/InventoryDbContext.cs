using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBO.Inventory.DomainModels;

namespace ERPDAL.InventoryDAL
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext():base("Inventory")
        {

        }
        public DbSet<Warehouse> tblWarehouses { get; set; }
        public DbSet<ItemType> tblItemTypes { get; set; }
        public DbSet<Unit> tblUnits { get; set; }
        public DbSet<Item> tblItems { get; set; }
        public DbSet<WarehouseStockInfo> tblWarehouseStockInfo { get; set; }
        public DbSet<WarehouseStockDetail> tblWarehouseStockDetails { get; set; }
        
    }
}
