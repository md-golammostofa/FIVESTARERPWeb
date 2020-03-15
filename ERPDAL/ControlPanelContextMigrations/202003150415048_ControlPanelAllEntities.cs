namespace ERPDAL.ControlPanelContextMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlPanelAllEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tblOrganizations",
                c => new
                    {
                        OrgId = c.Long(nullable: false, identity: true),
                        OrganizationName = c.String(maxLength: 150),
                        Address = c.String(maxLength: 150),
                        EUserId = c.Long(),
                        EntryDate = c.DateTime(),
                        UpUserId = c.Long(),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.OrgId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tblOrganizations");
        }
    }
}
