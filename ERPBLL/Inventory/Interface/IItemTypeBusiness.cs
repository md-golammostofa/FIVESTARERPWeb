using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
   public interface IItemTypeBusiness
    {
        IEnumerable<ItemType> GetAllItemTypeByOrgId(long orgId);
        bool SaveItemType(ItemTypeDTO itemType, long userId, long orgId);
        ItemType GetItemTypeOneByOrgId(long id,long warehouseId, long orgId);
        bool IsDuplicateItemTypeName(string itemTypeName, long id, long orgId, long warehouseId);
        ItemType GetItemType(long id, long orgId);
    }
}
