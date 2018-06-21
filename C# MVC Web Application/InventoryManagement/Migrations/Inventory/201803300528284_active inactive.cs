namespace InventoryManagement.Migrations.Inventory
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class activeinactive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Facilities", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Resources", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Resources", "IsActive");
            DropColumn("dbo.Facilities", "IsActive");
            DropColumn("dbo.Employees", "IsActive");
        }
    }
}
