
using System.Data.Entity;
using ERPBO.Production.DomainModels;

namespace ERPDAL.ProductionDAL
{
    public class ProductionDbContext : DbContext
    {
        public ProductionDbContext():base("Production")
        {

        }
        public DbSet<RequsitionInfo> tblRequsitionInfo { get; set; }
        public DbSet<RequsitionDetail> tblRequsitionDetails { get; set; }
        public DbSet<ProductionLine> tblProductionLines { get; set; }
        public DbSet<ProductionStockInfo> tblProductionStockInfo { get; set; }
        public DbSet<ProductionStockDetail> tblProductionStockDetail { get; set; }
    }
}
