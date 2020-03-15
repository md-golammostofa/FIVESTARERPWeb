using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
    public interface IWarehouseBusiness
    {
        IEnumerable<Warehouse> GetAllWarehouseByOrgId(long orgId);
        bool SaveWarehouse(WarehouseDTO warehouse,long userId, long orgId);
        Warehouse GetWarehouseOneByOrgId(long id, long orgId);
        Task<bool> SaveAsync(WarehouseDTO warehouse, long UserId, long orgId);
        IEnumerable<dynamic> SqlQuery(string query);
        IEnumerable<dynamic> GetComplexData(long orgId);

        bool IsDuplicateWarehouseName(string warehouseName, long id, long orgId);
    }
}
