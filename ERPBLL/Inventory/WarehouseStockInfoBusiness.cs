using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DomainModels;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
   public class WarehouseStockInfoBusiness: IWarehouseStockInfoBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly WarehouseStockInfoRepository warehouseStockInfoRepository; // table
        public WarehouseStockInfoBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            warehouseStockInfoRepository = new WarehouseStockInfoRepository(this._inventoryDb);
        }

        public IEnumerable<WarehouseStockInfo> GetAllWarehouseStockInfoByOrgId(long orgId)
        {
            return warehouseStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
    }
}
