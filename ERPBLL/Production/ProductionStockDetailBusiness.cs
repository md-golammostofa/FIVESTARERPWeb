using ERPBLL.Inventory;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class ProductionStockDetailBusiness : IProductionStockDetailBusiness
    {
        private readonly ProductionStockDetailRepository _productionStockDetailRepository; //table
        private readonly ProductionStockInfoRepository _productionStockInfoRepository; // table
        private readonly IRequsitionInfoBusiness _requsitionInfoBusiness; // table
        private readonly IRequsitionDetailBusiness _requsitionDetailBusiness; // table
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness; // table
        private readonly IItemBusiness _itemBusiness;  // table

        private readonly IProductionUnitOfWork _productionDb; // database;
        private readonly IInventoryUnitOfWork _inventoryDb; // database
        public ProductionStockDetailBusiness(IProductionUnitOfWork productionDb , IInventoryUnitOfWork inventoryDb)
        {
            this._productionDb = productionDb;
            this._inventoryDb = inventoryDb;

            _productionStockDetailRepository = new ProductionStockDetailRepository(this._productionDb);
            _productionStockInfoRepository =  new ProductionStockInfoRepository(this._productionDb);
            _productionStockInfoBusiness = new ProductionStockInfoBusiness(this._productionDb);
            _requsitionInfoBusiness = new RequsitionInfoBusiness(this._productionDb, this._inventoryDb);
            _requsitionDetailBusiness = new RequsitionDetailBusiness(this._productionDb);
            _itemBusiness = new ItemBusiness(this._inventoryDb);
        }
        public IEnumerable<ProductionStockDetail> GelAllProductionStockDetailByOrgId(long orgId)
        {
            return _productionStockDetailRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
        public bool SaveProductionStockIn(List<ProductionStockDetailDTO> productionStockDetailDTOs, long userId, long orgId)
        {
            List<ProductionStockDetail> productionStockDetails = new List<ProductionStockDetail>();
            foreach (var item in productionStockDetailDTOs)
            {
                ProductionStockDetail stockDetail = new ProductionStockDetail();
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
                stockDetail.RefferenceNumber = item.RefferenceNumber;

                var productionInfo = _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(orgId).Where(o => o.ItemTypeId == item.ItemTypeId && o.ItemId == item.ItemId).FirstOrDefault();
                if (productionInfo != null)
                {
                    productionInfo.StockInQty += item.Quantity;
                    _productionStockInfoRepository.Update(productionInfo);
                }
                else
                {
                    ProductionStockInfo productionStockInfo = new ProductionStockInfo();
                    productionStockInfo.WarehouseId = item.WarehouseId;
                    productionStockInfo.ItemTypeId = item.ItemTypeId;
                    productionStockInfo.ItemId = item.ItemId;
                    productionStockInfo.UnitId = stockDetail.UnitId;
                    productionStockInfo.StockInQty = item.Quantity;
                    productionStockInfo.StockOutQty = 0;
                    productionStockInfo.OrganizationId = orgId;
                    productionStockInfo.EUserId = userId;
                    productionStockInfo.EntryDate = DateTime.Now;
                    _productionStockInfoRepository.Insert(productionStockInfo);
                }
                productionStockDetails.Add(stockDetail);
            }
            _productionStockDetailRepository.InsertAll(productionStockDetails);
            return _productionStockDetailRepository.Save();
        }
        public bool SaveProductionStockOut(List<ProductionStockDetailDTO> productionStockDetailDTOs, long userId, long orgId, string flag)
        {
            throw new NotImplementedException();
        }
        public bool SaveProductionStockInByProductionRequistion(long reqId, string status, long orgId, long userId)
        {
            var reqInfo = _requsitionInfoBusiness.GetRequisitionById(reqId, orgId);
            var reqDetail = _requsitionDetailBusiness.GetRequsitionDetailByReqId(reqId, orgId).ToList();
            if(reqInfo!= null && reqInfo.StateStatus =="Approved" && reqDetail.Count > 0)
            {
                List<ProductionStockDetailDTO> productionStockDetailDTOs = new List<ProductionStockDetailDTO>();
                foreach (var item in reqDetail)
                {
                    ProductionStockDetailDTO productionStockDetailDTO = new ProductionStockDetailDTO
                    {
                        WarehouseId = reqInfo.WarehouseId,
                        ItemTypeId = item.ItemTypeId,
                        ItemId = item.ItemId,
                        OrganizationId = orgId,
                        EUserId = userId,
                        UnitId = item.UnitId,
                        StockStatus = "Stock-In",
                        Remarks = item.Remarks,
                        Quantity = (int)item.Quantity.Value,
                        RefferenceNumber = reqInfo.ReqInfoCode
                    };
                    productionStockDetailDTOs.Add(productionStockDetailDTO);
                }
                if(SaveProductionStockIn(productionStockDetailDTOs, userId, orgId) == true)
                {
                   return _requsitionInfoBusiness.SaveRequisitionStatus(reqId, status, orgId);
                }
            }
            return false;
        }
    }
}
