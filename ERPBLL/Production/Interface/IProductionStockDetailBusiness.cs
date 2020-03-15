using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
   public interface IProductionStockDetailBusiness
    {
        IEnumerable<ProductionStockDetail> GelAllProductionStockDetailByOrgId(long orgId);
        bool SaveProductionStockIn(List<ProductionStockDetailDTO> productionStockDetailDTOs, long userId, long orgId);
        bool SaveProductionStockInByProductionRequistion(long reqId, string status, long orgId, long userId);
        bool SaveProductionStockOut(List<ProductionStockDetailDTO> productionStockDetailDTOs, long userId, long orgId, string flag);
    }
}
