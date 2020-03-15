using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPWeb.Filters;
using System.Linq;
using System.Web.Mvc;
using ERPBO.Inventory.DTOModel;

namespace ERPWeb.Controllers
{

    public class CommonController : BaseController
    {
        IWarehouseBusiness _warehouseBusiness;
        IItemTypeBusiness _itemTypeBusiness;
        IUnitBusiness _unitBusiness;
        IItemBusiness _itemBusiness;
        IRequsitionInfoBusiness _requsitionInfoBusiness;
        IRequsitionDetailBusiness _requsitionDetailBusiness;
        IProductionLineBusiness _productionLineBusiness;

        private readonly long UserId = 1;
        private readonly long OrgId = 1;
        public CommonController(IWarehouseBusiness warehouseBusiness,IItemTypeBusiness itemTypeBusiness,IUnitBusiness unitBusiness,IItemBusiness itemBusiness, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness, IProductionLineBusiness productionLineBusiness)
        {
            this._warehouseBusiness = warehouseBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            this._unitBusiness = unitBusiness;
            this._itemBusiness = itemBusiness;
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
            this._productionLineBusiness = productionLineBusiness;
        }

        #region Validation Action Methods
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateWarehouseName(string warehouseName, long id)
        {
            bool isExist = _warehouseBusiness.IsDuplicateWarehouseName(warehouseName, id, OrgId);
            return Json(isExist);
        }
        #endregion
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateItemTypeName(string itemTypeName, long id, long warehouseId)
        {
            bool isExist = _itemTypeBusiness.IsDuplicateItemTypeName(itemTypeName, id, OrgId, warehouseId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateUnitName(string unitName, long id)
        {
            bool isExist = _unitBusiness.IsDuplicateUnitName(unitName, id, OrgId);
            return Json(isExist);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateItemName(string itemName, long id)
        {
            bool isExist = _itemBusiness.IsDuplicateItemName(itemName, id, OrgId);
            return Json(isExist);
        }

        [HttpPost]
        public ActionResult GetItemTypeForDDL(long warehouseId)
        {
            var itemTypes =_itemTypeBusiness.GetAllItemTypeByOrgId(OrgId).AsEnumerable();
            var dropDown = itemTypes.Where(i => i.WarehouseId == warehouseId).Select(i=> new Dropdown { text = i.ItemName,value= i.ItemId.ToString() }).ToList();
            return Json(dropDown);
        }

        [HttpPost]
        public ActionResult GetItemForDDL(long itemTypeId)
        {
            var items = _itemBusiness.GetAllItemByOrgId(OrgId).AsEnumerable();
            var dropDown = items.Where(i => i.ItemTypeId == itemTypeId).Select(i => new Dropdown { text = i.ItemName, value = i.ItemId.ToString() }).ToList();
            return Json(dropDown);
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult GetUnitByItemId(long itemId)
        {
            var unitId = _itemBusiness.GetItemOneByOrgId(itemId, OrgId).UnitId;
            var unit = _unitBusiness.GetUnitOneByOrgId(unitId, OrgId);
            UnitDomainDTO unitDTO = new UnitDomainDTO();
            unitDTO.UnitId = unit.UnitId;
            unitDTO.UnitName = unit.UnitName;
            unitDTO.UnitSymbol = unit.UnitSymbol;
            return Json(unitDTO);
        }
        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult IsDuplicateLineNumber(string lineNumber,long id)
        {
            bool isExist =_productionLineBusiness.IsDuplicateLineNumber(lineNumber, id, OrgId);
            return Json(isExist);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}