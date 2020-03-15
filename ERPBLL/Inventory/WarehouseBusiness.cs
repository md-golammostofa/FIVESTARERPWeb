using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModels;
using ERPDAL.InventoryDAL;
using ERPDAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ERPBLL.Inventory
{
    public class WarehouseBusiness : IWarehouseBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly WarehouseRepository warehouseRepository; // table
        public WarehouseBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            warehouseRepository = new WarehouseRepository(this._inventoryDb);
        }

        public bool SaveWarehouse(WarehouseDTO warehouseDTO, long userId, long orgId)
        {
            Warehouse warehouse = new Warehouse();
            if (warehouseDTO.Id == 0)
            {
                warehouse.WarehouseName = warehouseDTO.WarehouseName;
                warehouse.Remarks = warehouseDTO.Remarks;
                warehouse.IsActive = warehouseDTO.IsActive;
                warehouse.EUserId = userId;
                warehouse.EntryDate = DateTime.Now;
                warehouse.OrganizationId = orgId;
                warehouseRepository.Insert(warehouse);
            }
            else
            {
                warehouse = GetWarehouseOneByOrgId(warehouseDTO.Id, orgId);
                warehouse.Remarks = warehouseDTO.Remarks;
                warehouse.IsActive = warehouseDTO.IsActive;
                warehouse.UpUserId = userId;
                warehouse.UpdateDate = DateTime.Now;
                warehouseRepository.Update(warehouse);
            }
            return warehouseRepository.Save();
        }

        public IEnumerable<Warehouse> GetAllWarehouseByOrgId(long orgId)
        {
            return warehouseRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }

        public Warehouse GetWarehouseOneByOrgId(long id, long orgId)
        {
            return warehouseRepository.GetOneByOrg(ware => ware.Id == id && ware.OrganizationId == orgId);
        }
        public async Task<bool> SaveAsync(WarehouseDTO warehouseDomain, long userId, long orgId)
        {
            //    Warehouse warehouse = new Warehouse();
            //    if (warehouse.Id == 0)
            //    {
            //        warehouse.WarehouseName = warehouseDomain.WarehouseName;
            //        warehouse.Remarks = warehouseDomain.Remarks;
            //        warehouse.IsActive = warehouseDomain.IsActive;
            //        warehouse.EUserId = userId;
            //        warehouse.EntryDate = DateTime.Now;
            //        warehouseRepository.Insert(warehouse);
            //    }
            //    else
            //    {
            //        warehouse.Remarks = warehouseDomain.Remarks;
            //        warehouse.IsActive = warehouseDomain.IsActive;
            //        warehouse.UpUserId = userId;
            //        warehouse.UpdateDate = DateTime.Now;
            //        warehouseRepository.Update(warehouse);
            //    }
            //    return await warehouseRepository.SaveAsync();
            //}

            return await _inventoryDb.Db.SaveChangesAsync() > 0;
        }

        public IEnumerable<dynamic> GetComplexData(long orgId)
        {
            string query = string.Format("");
            return SqlQuery(query)
;
        }

        public IEnumerable<dynamic> SqlQuery(string query)
        {
          return warehouseRepository.SqlQuery(query).ToList();
        }

        public bool IsDuplicateWarehouseName(string warehouseName, long id, long orgId)
        {
            return warehouseRepository.GetOneByOrg(ware => ware.WarehouseName == warehouseName && ware.Id != id && ware.OrganizationId == orgId) != null ? true : false;
        }
    }
}
