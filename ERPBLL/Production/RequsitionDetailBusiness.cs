using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ERPBLL.Production
{
   public class RequsitionDetailBusiness: IRequsitionDetailBusiness
   {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly RequsitionDetailRepository requsitionDetailRepository; // table
        public RequsitionDetailBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            requsitionDetailRepository = new RequsitionDetailRepository(this._productionDb);
        }

        public IEnumerable<RequsitionDetail> GetAllReqDetailByOrgId(long orgId)
        {
            return requsitionDetailRepository.GetAll(unit => unit.OrganizationId == orgId).ToList();
        }
        public bool SaveReqDetails(RequsitionDetailDTO detailDTO, long userId, long orgId)
        {
            
            RequsitionDetail requsitionDetail = new RequsitionDetail();
            if (detailDTO.ReqDetailId == 0)
            {
                requsitionDetail.ItemTypeId = detailDTO.ItemTypeId;
                requsitionDetail.ItemId = detailDTO.ItemId;
                requsitionDetail.Quantity = detailDTO.Quantity;
                requsitionDetail.UnitId = detailDTO.UnitId;
                requsitionDetail.Remarks = requsitionDetail.Remarks;
                requsitionDetail.EUserId = userId;
                requsitionDetail.EntryDate = DateTime.Now;
                requsitionDetail.OrganizationId = orgId;
                requsitionDetailRepository.Insert(requsitionDetail);
            }
            else
            {
                requsitionDetail.ItemTypeId = detailDTO.ItemTypeId;
                requsitionDetail.ItemId = detailDTO.ItemId;
                requsitionDetail.Quantity = detailDTO.Quantity;
                requsitionDetail.UnitId = detailDTO.UnitId;
                requsitionDetail.Remarks = requsitionDetail.Remarks;
                requsitionDetail.UpUserId = userId;
                requsitionDetail.UpdateDate = DateTime.Now;
                requsitionDetail.OrganizationId = orgId;
                requsitionDetailRepository.Update(requsitionDetail);
            }
            return requsitionDetailRepository.Save();
        }

        public IEnumerable<RequsitionDetail> GetRequsitionDetailByReqId(long id, long orgId)
        {
            return requsitionDetailRepository.GetAll(rd => rd.ReqInfoId == id && rd.OrganizationId == orgId).ToList();
        }

    }
}
