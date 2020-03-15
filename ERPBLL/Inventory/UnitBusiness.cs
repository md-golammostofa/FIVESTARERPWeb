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
   public class UnitBusiness: IUnitBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly UnitRepository unitRepository; // table
        public UnitBusiness(IInventoryUnitOfWork inventoryDb)
        {
            this._inventoryDb = inventoryDb;
            unitRepository = new UnitRepository(this._inventoryDb);
        }

        public IEnumerable<Unit> GetAllUnitByOrgId(long orgId)
        {
            return unitRepository.GetAll(unit => unit.OrganizationId == orgId).ToList();
        }

        public Unit GetUnitOneByOrgId(long id, long orgId)
        {
            return unitRepository.GetOneByOrg(unit => unit.UnitId == id && unit.OrganizationId == orgId);
        }

        public bool IsDuplicateUnitName(string unitName, long id, long orgId)
        {
            return unitRepository.GetOneByOrg(unit => unit.UnitName == unitName && unit.UnitId != id && unit.OrganizationId == orgId) != null ? true : false;
        }

        public bool SaveUnit(UnitDomainDTO unitDTO, long userId, long orgId)
        {
            Unit unit = new Unit();
            if (unitDTO.UnitId == 0)
            {
                unit.UnitName = unitDTO.UnitName;
                unit.UnitSymbol = unitDTO.UnitSymbol;
                unit.Remarks = unitDTO.Remarks;
                unit.EUserId = userId;
                unit.EntryDate = DateTime.Now;
                unit.OrganizationId = orgId;
                unitRepository.Insert(unit);
            }
            else
            {
                unit = GetUnitOneByOrgId(unitDTO.UnitId, orgId);
                unit.UnitName = unitDTO.UnitName;
                unit.UnitSymbol = unitDTO.UnitSymbol;
                unit.Remarks = unitDTO.Remarks;
                unit.UpUserId = userId;
                unit.UpdateDate = DateTime.Now;
                unit.OrganizationId = orgId;
                unitRepository.Update(unit);
            }
            return unitRepository.Save();

        }
    }
}
