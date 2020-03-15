using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
   public interface IRequsitionDetailBusiness
    {
        IEnumerable<RequsitionDetail> GetAllReqDetailByOrgId(long orgId);
        bool SaveReqDetails(RequsitionDetailDTO detailDTO, long userId, long orgId);
        IEnumerable<RequsitionDetail> GetRequsitionDetailByReqId(long id, long orgId);
    }
}
