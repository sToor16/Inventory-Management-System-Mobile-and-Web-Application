namespace InventoryManagement.Migrations.Inventory
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class defaultvaluetrueforbooleancheck : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "defaultTrueCheck", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "defaultTrueCheck");
        }
    }
}
