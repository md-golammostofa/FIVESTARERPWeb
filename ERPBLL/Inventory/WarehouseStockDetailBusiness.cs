using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class WarehouseStockDetailBusiness : IWarehouseStockDetailBusiness
    {
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        private readonly IItemBusiness _itemBusiness;
        private readonly IWarehouseStockInfoBusiness _warehouseStockInfoBusiness;
        private readonly WarehouseStockDetailRepository warehouseStockDetailRepository; // table 
        private readonly WarehouseStockInfoRepository warehouseStockInfoRepository; // table 

        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness; // table
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness; // table
        private readonly IProductionUnitOfWork _productionDb; // database;

        public WarehouseStockDetailBusiness(IInventoryUnitOfWork inventoryDb, IItemBusiness itemBusiness, IWarehouseStockInfoBusiness warehouseStockInfoBusiness, IProductionUnitOfWork productionDb)
        {
            this._inventoryDb = inventoryDb;
            _itemBusiness = itemBusiness;
            warehouseStockDetailRepository = new WarehouseStockDetailRepository(this._inventoryDb);
            _warehouseStockInfoBusiness = warehouseStockInfoBusiness;
            warehouseStockInfoRepository = new WarehouseStockInfoRepository(this._inventoryDb);

            this._productionDb = productionDb;
            _requsitionInfoBusiness = new RequsitionInfoBusiness(this._productionDb, this._inventoryDb);
            _requsitionDetailBusiness = new RequsitionDetailBusiness(this._productionDb);
        }
        public IEnumerable<WarehouseStockDetail> GelAllWarehouseStockDetailByOrgId(long orgId)
        {
            return warehouseStockDetailRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
        public bool SaveWarehouseStockIn(List<WarehouseStockDetailDTO> warehouseStockDetailDTO, long userId, long orgId)
        {
            List<WarehouseStockDetail> warehouseStockDetails = new List<WarehouseStockDetail>();

            foreach (var item in warehouseStockDetailDTO)
            {
                WarehouseStockDetail stockDetail = new WarehouseStockDetail();
                stockDetail.WarehouseId = item.WarehouseId;
                stockDetail.ItemTypeId = item.ItemTypeId;
                stockDetail.ItemId = item.ItemId;
                stockDetail.Quantity = item.Quantity;
                stockDetail.OrganizationId = orgId;
                stockDetail.EUserId = userId;
                stockDetail.Remarks = item.Remarks;
                stockDetail.UnitId = _itemBusiness.GetItemById(item.ItemId.Value, orgId).UnitId;
                stockDetail.EntryDate = DateTime.Now;
                stockDetail.StockStatus = "Stock-In";

                var warehouseInfo = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId).FirstOrDefault();
                if (warehouseInfo != null)
                {
                    warehouseInfo.StockInQty += item.Quantity;
                    warehouseStockInfoRepository.Update(warehouseInfo);
                }
                else
                {
                    WarehouseStockInfo warehouseStockInfo = new WarehouseStockInfo();
                    warehouseStockInfo.WarehouseId = item.WarehouseId;
                    warehouseStockInfo.ItemTypeId = item.ItemTypeId;
                    warehouseStockInfo.ItemId = item.ItemId;
                    warehouseStockInfo.UnitId = stockDetail.UnitId;
                    warehouseStockInfo.StockInQty = item.Quantity;
                    warehouseStockInfo.StockOutQty = 0;
                    warehouseStockInfo.OrganizationId = orgId;
                    warehouseStockInfo.EUserId = userId;
                    warehouseStockInfo.EntryDate = DateTime.Now;
                    warehouseStockInfoRepository.Insert(warehouseStockInfo);
                }
                warehouseStockDetails.Add(stockDetail);
            }
            warehouseStockDetailRepository.InsertAll(warehouseStockDetails);
            return warehouseStockDetailRepository.Save();
        }
        public bool SaveWarehouseStockOut(List<WarehouseStockDetailDTO> warehouseStockDetailDTOs, long userId, long orgId, string flag)
        {
            List<WarehouseStockDetail> warehouseStockDetails = new List<WarehouseStockDetail>();
            bool isValidate = true;
            switch (flag)
            {
                case
                #region Production Requistion
         "Production Requistion":
                    var items = warehouseStockDetailDTOs.Select(s => s.ItemId).ToList();
                    var stock = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(s => items.Contains(s.ItemId.Value)).Select(s => s.ItemId).ToList();

                    if (items.Count() == stock.Count())
                    {
                        foreach (var item in warehouseStockDetailDTOs)
                        {
                            var warehouseInfo = _warehouseStockInfoBusiness.GetAllWarehouseStockInfoByOrgId(orgId).Where(s => s.ItemId == item.ItemId && (s.StockInQty - s.StockOutQty) >= item.Quantity).FirstOrDefault();
                            if (warehouseInfo != null)
                            {
                                warehouseInfo.StockOutQty += item.Quantity;
                                warehouseInfo.UpUserId = userId;
                                warehouseInfo.UpdateDate = DateTime.Now;

                                WarehouseStockDetail warehouseStockDetail = new WarehouseStockDetail()
                                {
                                    WarehouseId = item.WarehouseId,
                                    ItemTypeId = item.ItemTypeId,
                                    ItemId = item.ItemId,
                                    Quantity = item.Quantity,
                                    EUserId = userId,
                                    EntryDate = DateTime.Now,
                                    OrganizationId = orgId,
                                    Remarks = item.Remarks,
                                    StockStatus = item.StockStatus,
                                    RefferenceNumber = item.RefferenceNumber
                                };
                                warehouseStockInfoRepository.Update(warehouseInfo);
                                warehouseStockDetails.Add(warehouseStockDetail);
                            }
                            else
                            {
                                isValidate = false;
                            }
                        }
                    }
                    if (isValidate == true)
                    {
                        warehouseStockDetailRepository.InsertAll(warehouseStockDetails);
                        return warehouseStockDetailRepository.Save();
                    }
                    break;
                #endregion
                default:
                    break;
            }
            return false;
        }
        public bool SaveWarehouseStockOutByProductionRequistion(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = _requsitionInfoBusiness.GetRequisitionById(reqId, orgId);
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqId, orgId);
            if (reqInfo != null && reqDetail.Count() > 0)
            {
                List<WarehouseStockDetailDTO> stockDetailDTOs = new List<WarehouseStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    WarehouseStockDetailDTO stockDetailDTO = new WarehouseStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        ItemTypeId = item.ItemTypeId.Value,
                        ItemId = item.ItemId,
                        UnitId = item.UnitId.Value,
                        OrganizationId = item.OrganizationId,
                        Quantity = (int)item.Quantity.Value,
                        EUserId = userId,
                        EntryDate = DateTime.Now,
                        Remarks = "Stock Out By Production Requistion " + "(" + reqInfo.ReqInfoCode + ")",
                        RefferenceNumber = reqInfo.ReqInfoCode,
                        StockStatus = "Stock-Out"
                    };

                    stockDetailDTOs.Add(stockDetailDTO);
                }
                if (SaveWarehouseStockOut(stockDetailDTOs, userId, orgId, "Production Requistion") == true)
                {
                    return _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, orgId);
                }
            }
            return false;
        }
    }
}
