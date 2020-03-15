using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERPBO.Inventory.DomainModels;

namespace ERPDAL.InventoryDAL
{
    public class WarehouseRepository:InventoryBaseRepository<Warehouse>
    {
        public WarehouseRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class ItemTypeRepository: InventoryBaseRepository<ItemType>
    {
        public ItemTypeRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class UnitRepository : InventoryBaseRepository<Unit>
    {
        public UnitRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class ItemRepository : InventoryBaseRepository<Item>
    {
        public ItemRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
    public class WarehouseStockInfoRepository : InventoryBaseRepository<WarehouseStockInfo>
    {
        public WarehouseStockInfoRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }

    public class WarehouseStockDetailRepository : InventoryBaseRepository<WarehouseStockDetail>
    {
        public WarehouseStockDetailRepository(IInventoryUnitOfWork inventoryUnitOfWork) : base(inventoryUnitOfWork) { }
    }
}
