namespace ERPDAL.ProductionContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductionAllEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblProductionLines",
                c => new
                    {
                        LineId = c.Long(nullable: false, identity: true),
                        LineNumber = c.String(maxLength: 100),
                        LineIncharge = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LineId);
            
            CreateTable(
                "dbo.tblProductionStockDetail",
                c => new
                    {
                        StockDetailId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Int(nullable: false),
                        ExpireDate = c.DateTime(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        StockStatus = c.String(maxLength: 150),
                        RefferenceNumber = c.String(maxLength: 150),
                        ProductionStockInfo_ProductionStockInfoId = c.Long(),
                    })
                .PrimaryKey(t => t.StockDetailId)
                .ForeignKey("dbo.tblProductionStockInfo", t => t.ProductionStockInfo_ProductionStockInfoId)
                .Index(t => t.ProductionStockInfo_ProductionStockInfoId);
            
            CreateTable(
                "dbo.tblProductionStockInfo",
                c => new
                    {
                        ProductionStockInfoId = c.Long(nullable: false, identity: true),
                        WarehouseId = c.Long(),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        StockInQty = c.Int(),
                        StockOutQty = c.Int(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProductionStockInfoId);
            
            CreateTable(
                "dbo.tblRequsitionDetails",
                c => new
                    {
                        ReqDetailId = c.Long(nullable: false, identity: true),
                        ItemTypeId = c.Long(),
                        ItemId = c.Long(),
                        UnitId = c.Long(),
                        Quantity = c.Long(),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        ReqInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ReqDetailId)
                .ForeignKey("dbo.tblRequsitionInfo", t => t.ReqInfoId, cascadeDelete: true)
                .Index(t => t.ReqInfoId);
            
            CreateTable(
                "dbo.tblRequsitionInfo",
                c => new
                    {
                        ReqInfoId = c.Long(nullable: false, identity: true),
                        ReqInfoCode = c.String(maxLength: 100),
                        StateStatus = c.String(maxLength: 100),
                        Remarks = c.String(maxLength: 150),
                        OrganizationId = c.Long(nullable: false),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                        WarehouseId = c.Long(nullable: false),
                        LineId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ReqInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tblRequsitionDetails", "ReqInfoId", "dbo.tblRequsitionInfo");
            DropForeignKey("dbo.tblProductionStockDetail", "ProductionStockInfo_ProductionStockInfoId", "dbo.tblProductionStockInfo");
            DropIndex("dbo.tblRequsitionDetails", new[] { "ReqInfoId" });
            DropIndex("dbo.tblProductionStockDetail", new[] { "ProductionStockInfo_ProductionStockInfoId" });
            DropTable("dbo.tblRequsitionInfo");
            DropTable("dbo.tblRequsitionDetails");
            DropTable("dbo.tblProductionStockInfo");
            DropTable("dbo.tblProductionStockDetail");
            DropTable("dbo.tblProductionLines");
        }
    }
}
