using ERPBLL.Inventory;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production;
using ERPBLL.Production.Interface;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace ERPWeb
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            // e.g. container.RegisterType<ITestService, TestService>();

            // Inventory Database
            container.RegisterType<IWarehouseStockDetailBusiness, WarehouseStockDetailBusiness>();
            container.RegisterType<IWarehouseStockInfoBusiness, WarehouseStockInfoBusiness>();
            container.RegisterType<IItemBusiness, ItemBusiness>();
            container.RegisterType<IUnitBusiness, UnitBusiness>();
            container.RegisterType<IItemTypeBusiness, ItemTypeBusiness>();
            container.RegisterType<IWarehouseBusiness, WarehouseBusiness>();
            container.RegisterType<IInventoryUnitOfWork, InventoryUnitOfWork>(); // database

            // Production Database
            container.RegisterType<IProductionLineBusiness, ProductionLineBusiness>();
            container.RegisterType<IRequsitionDetailBusiness, RequsitionDetailBusiness>();
            container.RegisterType<IRequsitionInfoBusiness, RequsitionInfoBusiness>();
            container.RegisterType<IProductionStockDetailBusiness, ProductionStockDetailBusiness>();
            container.RegisterType<IProductionStockInfoBusiness, ProductionStockInfoBusiness>();
            container.RegisterType<IProductionUnitOfWork, ProductionUnitOfWork>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}