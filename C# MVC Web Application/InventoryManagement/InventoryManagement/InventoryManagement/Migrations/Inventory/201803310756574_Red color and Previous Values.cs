namespace InventoryManagement.Migrations.Inventory
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RedcolorandPreviousValues : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Resources", "Changed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resources", "PreviousValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "PreviousValue");
            DropColumn("dbo.Resources", "Changed");
        }
    }
}
