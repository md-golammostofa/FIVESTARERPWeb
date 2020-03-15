using ERPBLL.Inventory.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DomainModels;
using ERPDAL.InventoryDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
   public class ItemTypeBusiness:IItemTypeBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly ItemTypeRepository itemTypeRepository; // table
        public ItemTypeBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            itemTypeRepository = new ItemTypeRepository(this._inventoryDb);
        }

        public IEnumerable<ItemType> GetAllItemTypeByOrgId(long orgId)
        {
            return itemTypeRepository.GetAll(item => item.OrganizationId == orgId).ToList();
        }

        

        public bool SaveItemType(ItemTypeDTO itemTypeDTO, long userId, long orgId)
        {
            ItemType itemType = new ItemType();
            if (itemTypeDTO.ItemId == 0)
            {
                itemType.ItemName = itemTypeDTO.ItemName;
                itemType.Remarks = itemTypeDTO.Remarks;
                itemType.IsActive = itemTypeDTO.IsActive;
                itemType.EUserId = userId;
                itemType.EntryDate = DateTime.Now;
                itemType.OrganizationId = orgId;
                itemType.WarehouseId = itemTypeDTO.WarehouseId;
                itemTypeRepository.Insert(itemType);
            }
            else
            {
                itemType = GetItemTypeOneByOrgId(itemTypeDTO.ItemId, itemTypeDTO.WarehouseId, orgId);
                itemType.ItemName = itemTypeDTO.ItemName;
                itemType.Remarks = itemTypeDTO.Remarks;
                itemType.IsActive = itemTypeDTO.IsActive;
                itemType.UpUserId = itemTypeDTO.UpUserId;
                itemType.UpdateDate = DateTime.Now;
                itemType.OrganizationId = orgId;
                itemTypeRepository.Update(itemType);
            }
            return itemTypeRepository.Save();
        }
        public bool IsDuplicateItemTypeName(string itemTypeName, long id, long orgId, long warehouseId)
        {
            return itemTypeRepository.GetOneByOrg(item => item.ItemName == itemTypeName && item.ItemId != id && item.OrganizationId == orgId && item.WarehouseId == warehouseId) != null ? true : false;
        }

        public ItemType GetItemTypeOneByOrgId(long id, long warehouseId, long orgId)
        {
            return itemTypeRepository.GetOneByOrg(item => item.ItemId == id && item.OrganizationId == orgId && item.WarehouseId==warehouseId);
        }

        public ItemType GetItemType(long id, long orgId)
        {
            return itemTypeRepository.GetOneByOrg(item => item.ItemId == id && item.OrganizationId == orgId);
        }
    }
}
