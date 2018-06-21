namespace InventoryManagement.Migrations.Inventory
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changingquantitytoint : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Resources", "Quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Resources", "Quantity", c => c.String(nullable: false));
        }
    }
}
