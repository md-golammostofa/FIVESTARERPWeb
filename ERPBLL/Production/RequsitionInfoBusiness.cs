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

namespace ERPBLL.Production
{
   public class RequsitionInfoBusiness: IRequsitionInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly RequsitionInfoRepository requsitionInfoRepository; // table
        private readonly IItemBusiness itemBusiness; // interface
        private readonly IInventoryUnitOfWork _inventoryDb; // database

        private readonly IWarehouseStockInfoBusiness _warehouseStockInfoBusiness;
        public RequsitionInfoBusiness(IProductionUnitOfWork productionDb, IInventoryUnitOfWork inventoryDb)
        {
            this._productionDb = productionDb;
            requsitionInfoRepository = new RequsitionInfoRepository(this._productionDb);
            this._inventoryDb = inventoryDb;
            _warehouseStockInfoBusiness = new WarehouseStockInfoBusiness(this._inventoryDb);
            itemBusiness = new ItemBusiness(this._inventoryDb);
        }

        public IEnumerable<RequsitionInfo> GetAllReqInfoByOrgId(long orgId)
        {
            return requsitionInfoRepository.GetAll(unit => unit.OrganizationId == orgId).ToList();
        }

        public RequsitionInfo GetRequisitionById(long reqId, long orgId)
        {
            return requsitionInfoRepository.GetOneByOrg(req=>req.ReqInfoId ==reqId && req.OrganizationId == orgId);
        }

        public bool SaveRequisition(ReqInfoDTO reqInfoDTO, long userId, long orgId)
        {
            RequsitionInfo requsitionInfo = new RequsitionInfo();
            requsitionInfo.WarehouseId = reqInfoDTO.WarehouseId.Value;
            requsitionInfo.LineId = reqInfoDTO.LineId.Value;
            requsitionInfo.OrganizationId = orgId;
            requsitionInfo.StateStatus = "Pending";
            requsitionInfo.ReqInfoCode =("REQ-"+ DateTime.Now.ToString("yy") + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd") + DateTime.Now.ToString("hh") + DateTime.Now.ToString("mm") + DateTime.Now.ToString("ss"));

            requsitionInfo.EntryDate = DateTime.Now;
            requsitionInfo.EUserId = userId;
            List<RequsitionDetail> requsitionDetails = new List<RequsitionDetail>();

            foreach (var item in reqInfoDTO.ReqDetails)
            {
                RequsitionDetail requsitionDetail = new RequsitionDetail();
                requsitionDetail.ItemTypeId = item.ItemTypeId;
                requsitionDetail.ItemId = item.ItemId;
                requsitionDetail.Quantity = item.Quantity;
                requsitionDetail.UnitId = itemBusiness.GetItemOneByOrgId(item.ItemId.Value, orgId).UnitId;
                requsitionDetail.Remarks = item.Remarks;
                requsitionDetail.EUserId = userId;
                requsitionDetail.EntryDate = DateTime.Now;
                requsitionDetail.OrganizationId = orgId;
                requsitionDetails.Add(requsitionDetail);
            }
            requsitionInfo.RequsitionDetails = requsitionDetails;
            requsitionInfoRepository.Insert(requsitionInfo);
            return requsitionInfoRepository.Save();
        }
        public bool SaveRequisitionStatus(long reqId, string status, long orgId)
        {
           var reqInfo = requsitionInfoRepository.GetOneByOrg(req => req.ReqInfoId == reqId && req.OrganizationId == orgId);
            if(reqInfo != null)
            {
                reqInfo.StateStatus = status;
                requsitionInfoRepository.Update(reqInfo);
            }
            return requsitionInfoRepository.Save();
        }
    }
}
