namespace InventoryManagement.Migrations.Inventory
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class previousValuechangedtoStringforreportpage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Resources", "PreviousValue", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Resources", "PreviousValue", c => c.Int(nullable: false));
        }
    }
}
