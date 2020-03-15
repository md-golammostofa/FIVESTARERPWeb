using ERPBO.Production.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPDAL.ProductionDAL
{
    public class RequsitionInfoRepository : ProductionBaseRepository<RequsitionInfo>
    {
        public RequsitionInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class RequsitionDetailRepository : ProductionBaseRepository<RequsitionDetail>
    {
        public RequsitionDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
    public class ProductionLineRepository : ProductionBaseRepository<ProductionLine>
    {
        public ProductionLineRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class ProductionStockInfoRepository : ProductionBaseRepository<ProductionStockInfo>
    {
        public ProductionStockInfoRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }

    public class ProductionStockDetailRepository : ProductionBaseRepository<ProductionStockDetail>
    {
        public ProductionStockDetailRepository(IProductionUnitOfWork productionUnitOfWork) : base(productionUnitOfWork) { }
    }
}
