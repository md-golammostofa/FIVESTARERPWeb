using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
   public interface IRequsitionInfoBusiness
    {
        IEnumerable<RequsitionInfo> GetAllReqInfoByOrgId(long orgId);
        bool SaveRequisition(ReqInfoDTO requsitionInfoDTO, long userId, long orgId);
        RequsitionInfo GetRequisitionById(long reqId, long orgId);
        bool SaveRequisitionStatus(long reqId, string status, long orgId);
    }
}
