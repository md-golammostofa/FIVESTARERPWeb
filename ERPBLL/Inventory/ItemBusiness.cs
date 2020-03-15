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
   public class ItemBusiness:IItemBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly ItemRepository itemRepository; // table
        public ItemBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            itemRepository = new ItemRepository(this._inventoryDb);
        }

        public IEnumerable<Item> GetAllItemByOrgId(long orgId)
        {
            return itemRepository.GetAll(item => item.OrganizationId == orgId).ToList();
        }

        public ItemDomainDTO GetItemById(long itemId, long orgId)
        {
            return itemRepository.GetAll(item => item.ItemId == itemId && item.OrganizationId == orgId).Select(it=> new ItemDomainDTO{
                ItemId = it.ItemId,
                ItemName = it.ItemName,
                ItemTypeId = it.ItemTypeId,
                UnitId = it.UnitId,
                IsActive = it.IsActive,
                Remarks = it.Remarks
            }).FirstOrDefault();
        }

        public Item GetItemOneByOrgId(long id, long orgId)
        {
            return itemRepository.GetOneByOrg(item => item.ItemId == id && item.OrganizationId == orgId);
        }

        public bool IsDuplicateItemName(string itemName, long id, long orgId)
        {
            return itemRepository.GetOneByOrg(item => item.ItemName == itemName && item.OrganizationId == orgId && item.ItemId != id) != null ? true : false;
        }

        public bool SaveItem(ItemDomainDTO itemDomain, long userId, long orgId)
        {
            Item items = new Item();
            if (itemDomain.ItemId == 0)
            {
                items.ItemName = itemDomain.ItemName;
                items.Remarks = itemDomain.Remarks;
                items.IsActive = itemDomain.IsActive;
                items.EUserId = userId;
                items.EntryDate = DateTime.Now;
                items.OrganizationId = orgId;
                items.ItemTypeId = itemDomain.ItemTypeId;
                items.UnitId = itemDomain.UnitId;
                itemRepository.Insert(items);
            }
            else
            {
                items = GetItemOneByOrgId(itemDomain.ItemId, orgId);
                items.ItemName = itemDomain.ItemName;
                items.Remarks = itemDomain.Remarks;
                items.IsActive = itemDomain.IsActive;
                items.UpUserId = itemDomain.UpUserId;
                items.UpdateDate = DateTime.Now;
                items.ItemTypeId = itemDomain.ItemTypeId;
                items.UnitId = itemDomain.UnitId;
                itemRepository.Update(items);
            }
            return itemRepository.Save();
        }
    }
}
