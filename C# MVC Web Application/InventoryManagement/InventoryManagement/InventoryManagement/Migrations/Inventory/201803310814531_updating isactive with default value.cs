namespace InventoryManagement.Migrations.Inventory
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatingisactivewithdefaultvalue : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Resources", "defaultTrueCheck");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Resources", "defaultTrueCheck", c => c.Boolean(nullable: false));
        }
    }
}
