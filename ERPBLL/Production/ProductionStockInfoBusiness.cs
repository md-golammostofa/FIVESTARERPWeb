using ERPBLL.Production.Interface;
using ERPBO.Production.DomainModels;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class ProductionStockInfoBusiness : IProductionStockInfoBusiness
    {
        private readonly IProductionUnitOfWork _productionDb; // database
        private readonly ProductionStockInfoRepository productionStockInfoRepository; // table

        public ProductionStockInfoBusiness(IProductionUnitOfWork productionDb)
        {
            this._productionDb = productionDb;
            productionStockInfoRepository = new ProductionStockInfoRepository(this._productionDb);
        }
        public IEnumerable<ProductionStockInfo> GetAllProductionStockInfoByOrgId(long orgId)
        {
            return productionStockInfoRepository.GetAll(ware => ware.OrganizationId == orgId).ToList();
        }
    }
}
