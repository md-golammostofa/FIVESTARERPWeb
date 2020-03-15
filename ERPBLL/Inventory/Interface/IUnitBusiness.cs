using ERPBO.Inventory.DTOModel;
using ERPBO.Inventory.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory.Interface
{
   public interface IUnitBusiness
    {
        IEnumerable<Unit> GetAllUnitByOrgId(long orgId);
        bool SaveUnit(UnitDomainDTO unitDTO, long userId, long orgId);
        bool IsDuplicateUnitName(string unitName, long id, long orgId);
        Unit GetUnitOneByOrgId(long id, long orgId);
    }
}
