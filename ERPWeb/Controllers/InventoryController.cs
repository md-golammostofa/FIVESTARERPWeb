using ERPBLL.Inventory.Interface;
using ERPBO.ControlPanel.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DTOModels;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.ViewModels;
using ERPWeb.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ERPBO.Production.DTOModel;
using ERPBLL.Production.Interface;
using ERPBO.Production.ViewModels;
using ERPBLL.Common;
using System.Data.Entity;
using ERPBO.Production.DomainModels;

namespace ERPWeb.Controllers
{
    public class InventoryController : BaseController
    {
        // GET: Inventory
        private IWarehouseBusiness _warehouseBusiness;
        private IItemTypeBusiness _itemTypeBusiness;
        private IUnitBusiness _unitBusiness;
        private IItemBusiness _itemBusiness;
        private IWarehouseStockInfoBusiness _warehouseStockInfoBusiness;
        private IWarehouseStockDetailBusiness _warehouseStockDetailBusiness;
        private IProductionLineBusiness _productionLineBusiness;
        private IRequsitionInfoBusiness _requsitionInfoBusiness;
        private IRequsitionDetailBusiness _requsitionDetailBusiness;

        private readonly long UserId = 1;
        private readonly long OrgId = 1;

        public InventoryController(IWarehouseBusiness warehouseBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IItemBusiness itemBusiness, IWarehouseStockInfoBusiness warehouseStockInfoBusiness, IWarehouseStockDetailBusiness warehouseStockDetailBusiness, IProductionLineBusiness productionLineBusiness, IRequsitionInfoBusiness requsitionInfoBusiness, IRequsitionDetailBusiness requsitionDetailBusiness)
        {
            this._warehouseBusiness = warehouseBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            this._unitBusiness = unitBusiness;
            this._itemBusiness = itemBusiness;
            this._warehouseStockInfoBusiness = warehouseStockInfoBusiness;
            this._warehouseStockDetailBusiness = warehouseStockDetailBusiness;
            this._productionLineBusiness = productionLineBusiness;
            this._requsitionInfoBusiness = requsitionInfoBusiness;
            this._requsitionDetailBusiness = requsitionDetailBusiness;
        }
        // GET: Account

        #region Warehouse - Table
        [HttpGet]
        public ActionResult GetWarehouseList()
        {
            IEnumerable<WarehouseDTO> warehousesDomains = _warehouseBusiness.GetAllWarehouseByOrgId(1).Select(ware => new WarehouseDTO
            {
                Id = ware.Id,
                WarehouseName = ware.WarehouseName,
                Remarks = ware.Remarks,
                StateStatus = (ware.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = ware.OrganizationId
            }).ToList();
            List<WarehouseViewModel> warehouseViewModels = new List<WarehouseViewModel>();
            AutoMapper.Mapper.Map(warehousesDomains, warehouseViewModels);
            return View(warehouseViewModels);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult SaveWarehouse(WarehouseViewModel viewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    WarehouseDTO dto = new WarehouseDTO();
                    AutoMapper.Mapper.Map(viewModel, dto);
                    isSuccess = _warehouseBusiness.SaveWarehouse(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        #endregion

        #region ItemType - Table
        public ActionResult GetItemTypeList()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

            IEnumerable<ItemTypeDTO> itemTypesDomains = _itemTypeBusiness.GetAllItemTypeByOrgId(OrgId).Select(item => new ItemTypeDTO
            {
                ItemId = item.ItemId,
                WarehouseId = item.WarehouseId,
                ItemName = item.ItemName,
                Remarks = item.Remarks,
                StateStatus = (item.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = item.OrganizationId,
                WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(item.WarehouseId, OrgId).WarehouseName)
            }).ToList();
            List<ItemTypeViewModel> itemTypeViewModels = new List<ItemTypeViewModel>();
            AutoMapper.Mapper.Map(itemTypesDomains, itemTypeViewModels);
            return View(itemTypeViewModels);
        }

        public ActionResult SaveItemType(ItemTypeViewModel itemTypeViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ItemTypeDTO dto = new ItemTypeDTO();
                    AutoMapper.Mapper.Map(itemTypeViewModel, dto);
                    isSuccess = _itemTypeBusiness.SaveItemType(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Unit - Table
        public ActionResult GetAllUnitList()
        {
            IEnumerable<UnitDomainDTO> unitDomains = _unitBusiness.GetAllUnitByOrgId(1).Select(unit => new UnitDomainDTO
            {
                UnitId = unit.UnitId,
                UnitName = unit.UnitName,
                UnitSymbol = unit.UnitSymbol,
                Remarks = unit.Remarks,
                OrganizationId = unit.OrganizationId
            }).ToList();
            List<UnitViewModel> unitViewModels = new List<UnitViewModel>();
            AutoMapper.Mapper.Map(unitDomains, unitViewModels);
            return View(unitViewModels);
        }

        public ActionResult SaveUnit(UnitViewModel unitViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    UnitDomainDTO dto = new UnitDomainDTO();
                    AutoMapper.Mapper.Map(unitViewModel, dto);
                    isSuccess = _unitBusiness.SaveUnit(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Item - Table
        public ActionResult GetItemList()
        {
            ViewBag.ddlItemTypeName = _itemTypeBusiness.GetAllItemTypeByOrgId(OrgId).Select(itemtype => new SelectListItem { Text = itemtype.ItemName, Value = itemtype.ItemId.ToString() }).ToList();

            ViewBag.ddlUnitName = _unitBusiness.GetAllUnitByOrgId(OrgId).Select(unit => new SelectListItem { Text = unit.UnitName, Value = unit.UnitId.ToString() }).ToList();

            IEnumerable<ItemDomainDTO> itemDomains = _itemBusiness.GetAllItemByOrgId(1).Select(item => new ItemDomainDTO
            {
                ItemId = item.ItemId,
                ItemName = item.ItemName,
                Remarks = item.Remarks,
                StateStatus = (item.IsActive == true ? "Active" : "Inactive"),
                OrganizationId = item.OrganizationId,
                ItemTypeId = item.ItemTypeId,
                ItemTypeName = _itemTypeBusiness.GetItemType(item.ItemTypeId, OrgId).ItemName,
                UnitId = item.UnitId,
                UnitName = _unitBusiness.GetUnitOneByOrgId(item.UnitId, OrgId).UnitName
            }).ToList();
            List<ItemViewModel> itemViewModels = new List<ItemViewModel>();
            AutoMapper.Mapper.Map(itemDomains, itemViewModels);
            return View(itemViewModels);
        }
        public ActionResult SaveItem(ItemViewModel itemViewModel)
        {
            bool isSuccess = false;
            if (ModelState.IsValid)
            {
                try
                {
                    ItemDomainDTO dto = new ItemDomainDTO();
                    AutoMapper.Mapper.Map(itemViewModel, dto);
                    isSuccess = _itemBusiness.SaveItem(dto, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }

        [HttpPost, ValidateJsonAntiForgeryToken]
        public ActionResult GetItemById(long id)
        {
            ItemDomainDTO itemDTO = _itemBusiness.GetItemById(id, OrgId);
            itemDTO.UnitName = _unitBusiness.GetUnitOneByOrgId(itemDTO.UnitId, OrgId).UnitName;
            itemDTO.ItemTypeName = _itemTypeBusiness.GetItemType(itemDTO.ItemTypeId, OrgId).ItemName;
            ItemViewModel itemViewModel = new ItemViewModel();
            AutoMapper.Mapper.Map(itemDTO, itemViewModel);
            return Json(itemViewModel);
        }
        #endregion

        #region Warehouse Stock Info -Table

        [HttpGet]
        public ActionResult GetWarehouseStockInfoList()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        [HttpGet]
        public ActionResult GetWarehouseStockInfoPartialList(long? WarehouseId, long? ItemTypeId, long? ItemId)
        {
            IEnumerable<WarehouseStockInfoDTO> warehouseStockInfoDTO = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(OrgId).Select(info => new WarehouseStockInfoDTO
            {
                StockInfoId = info.StockInfoId,
                WarehouseId = info.WarehouseId,
                Warehouse = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId.Value, OrgId).WarehouseName),
                ItemTypeId = info.ItemTypeId,
                ItemType = (_itemTypeBusiness.GetItemType(info.ItemTypeId.Value, OrgId).ItemName),
                ItemId = info.ItemId,
                Item = (_itemBusiness.GetItemOneByOrgId(info.ItemId.Value, OrgId).ItemName),
                UnitId = info.UnitId,
                Unit = (_unitBusiness.GetUnitOneByOrgId(info.UnitId.Value, OrgId).UnitSymbol),
                StockInQty = info.StockInQty,
                StockOutQty = info.StockOutQty,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
            }).AsEnumerable();

            warehouseStockInfoDTO = warehouseStockInfoDTO.Where(ws => (WarehouseId == null || WarehouseId == 0 || ws.WarehouseId == WarehouseId) && (ItemTypeId == null || ItemTypeId == 0 || ws.ItemTypeId == ItemTypeId) && (ItemId == null || ItemId == 0 || ws.ItemId == ItemId)).ToList();

            List<WarehouseStockInfoViewModel> warehouseStockInfoViews = new List<WarehouseStockInfoViewModel>();
            AutoMapper.Mapper.Map(warehouseStockInfoDTO, warehouseStockInfoViews);
            return PartialView("_WarehouseStockInfoList", warehouseStockInfoViews);
        }

        public ActionResult CreateStock()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem
            {
                Text = ware.WarehouseName,
                Value = ware.Id.ToString()
            }).ToList();
            return View();
        }

        [HttpPost]
        public ActionResult SaveWarehouseStockIn(List<WarehouseStockDetailViewModel> models)
        {
            bool isSuccess = false;
            if (ModelState.IsValid && models.Count > 0)
            {
                try
                {
                    List<WarehouseStockDetailDTO> dtos = new List<WarehouseStockDetailDTO>();
                    AutoMapper.Mapper.Map(models, dtos);
                    isSuccess = _warehouseStockDetailBusiness.SaveWarehouseStockIn(dtos, UserId, OrgId);
                }
                catch (Exception ex)
                {
                    isSuccess = false;
                }
            }
            return Json(isSuccess);
        }
        #endregion

        #region Production Requisition -Table
        [HttpGet]
        public ActionResult GetReqInfoList()
        {
            ViewBag.ddlWarehouse = _warehouseBusiness.GetAllWarehouseByOrgId(OrgId).Select(ware => new SelectListItem { Text = ware.WarehouseName, Value = ware.Id.ToString() }).ToList();

            ViewBag.ddlLineNumber = _productionLineBusiness.GetAllProductionLineByOrgId(OrgId).Select(line => new SelectListItem { Text = line.LineNumber, Value = line.LineId.ToString() }).ToList();

            ViewBag.ddlStateStatus = Utility.ListOfReqStatus().Where(status => status.value == "Pending" || status.value == "Accepted" || status.value == "Decline").Select(st => new SelectListItem
            {
                Text = st.text,
                Value = st.value
            }).ToList();
            return View();
        }

        // Used By  GetReqInfoList
        public ActionResult GetReqInfoParitalList(string reqCode, long? warehouseId, string status, long? line, string fromDate, string toDate)
        {
            IEnumerable<RequsitionInfoDTO> requsitionInfoDTO = _requsitionInfoBusiness.GetAllReqInfoByOrgId(OrgId).Where(req => (req.StateStatus == "Pending" || req.StateStatus == "Accepted" || req.StateStatus == "Decline")
                &&
                (reqCode == null || reqCode.Trim() == "" || req.ReqInfoCode.Contains(reqCode))
                &&
                (warehouseId == null || warehouseId <= 0 || req.WarehouseId == warehouseId)
                &&
                (status == null || status.Trim() == "" || req.StateStatus == status.Trim())
                &&
                (line == null || line <= 0 || req.LineId == line)
                &&
                (
                    (fromDate == null && toDate == null)
                    ||
                     (fromDate == "" && toDate == "")
                    ||
                    (fromDate.Trim() != "" && toDate.Trim() != "" &&

                        req.EntryDate.Value.Date >= Convert.ToDateTime(fromDate).Date &&
                        req.EntryDate.Value.Date <= Convert.ToDateTime(toDate).Date)
                    ||
                    (fromDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(fromDate).Date)
                    ||
                    (toDate.Trim() != "" && req.EntryDate.Value.Date == Convert.ToDateTime(toDate).Date)
                )
            ).Select(info => new RequsitionInfoDTO
            {
                ReqInfoId = info.ReqInfoId,
                ReqInfoCode = info.ReqInfoCode,
                LineId = info.LineId,
                LineNumber = (_productionLineBusiness.GetProductionLineOneByOrgId(info.LineId, OrgId).LineNumber),
                StateStatus = info.StateStatus,
                Remarks = info.Remarks,
                OrganizationId = info.OrganizationId,
                EntryDate = info.EntryDate,
                WarehouseId = info.WarehouseId,
                WarehouseName = (_warehouseBusiness.GetWarehouseOneByOrgId(info.WarehouseId, OrgId).WarehouseName),
                Qty = _requsitionDetailBusiness.GetRequsitionDetailByReqId(info.ReqInfoId, OrgId).Select(s => s.ItemId).Distinct().Count(),
            }).ToList();

            List<RequsitionInfoViewModel> requsitionInfoViewModels = new List<RequsitionInfoViewModel>();
            AutoMapper.Mapper.Map(requsitionInfoDTO, requsitionInfoViewModels);
            return PartialView(requsitionInfoViewModels);
        }
        public ActionResult GetRequsitionDetails(long? reqId)
        {
            IEnumerable<RequsitionDetailDTO> requsitionDetailDTO = _requsitionDetailBusiness.GetAllReqDetailByOrgId(OrgId).Where(rqd => reqId == null || reqId == 0 || rqd.ReqInfoId == reqId).Select(d => new RequsitionDetailDTO
            {
                ReqDetailId = d.ReqDetailId,
                ItemTypeId = d.ItemTypeId.Value,
                ItemTypeName = (_itemTypeBusiness.GetItemType(d.ItemTypeId.Value, OrgId).ItemName),
                ItemId = d.ItemId.Value,
                ItemName = (_itemBusiness.GetItemOneByOrgId(d.ItemId.Value, OrgId).ItemName),
                Quantity = d.Quantity.Value,
                UnitName = (_unitBusiness.GetUnitOneByOrgId(d.UnitId.Value, OrgId).UnitSymbol)
            }).ToList();
            List<RequsitionDetailViewModel> requsitionDetailViewModels = new List<RequsitionDetailViewModel>();
            AutoMapper.Mapper.Map(requsitionDetailDTO, requsitionDetailViewModels);

            ViewBag.RequisitionStatus = _requsitionInfoBusiness.GetRequisitionById(reqId.Value, OrgId).StateStatus;
            
            return PartialView("_GetRequsitionDetails", requsitionDetailViewModels);
        }

        [HttpPost,ValidateJsonAntiForgeryToken]
        public ActionResult SaveRequisitionStatus(long reqId, string status)
        {
            bool IsSuccess = false;
            if (reqId > 0 && !string.IsNullOrEmpty(status))
            {
                if(status == "Decline" || status == "Recheck")
                {
                    IsSuccess = _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, OrgId);
                }
                else
                {
                    IsSuccess = _warehouseStockDetailBusiness.SaveWarehouseStockOutByProductionRequistion(reqId, status, OrgId,UserId);
                }
            }
            return Json(IsSuccess);
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}