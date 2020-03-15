using ERPBO.Production.DomainModels;
using ERPBO.Production.DTOModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production.Interface
{
   public interface IProductionLineBusiness
    {
        IEnumerable<ProductionLine> GetAllProductionLineByOrgId(long orgId);
        bool SaveUnit(ProductionLineDTO lineDTO, long userId, long orgId);
        bool IsDuplicateLineNumber(string lineNumber, long id, long orgId);
        ProductionLine GetProductionLineOneByOrgId(long id, long orgId);
    }
}
