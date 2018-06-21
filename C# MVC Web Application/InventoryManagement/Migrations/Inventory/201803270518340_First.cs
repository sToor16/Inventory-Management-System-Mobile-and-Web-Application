namespace InventoryManagement.Migrations.Inventory
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class First : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IsAdmin = c.Boolean(nullable: false),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmployeeToFacilities",
                c => new
                    {
                        EmployeeToFacilityId = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        FacilityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeToFacilityId)
                .ForeignKey("dbo.Employees", t => t.EmployeeID, cascadeDelete: true)
                .ForeignKey("dbo.Facilities", t => t.FacilityId, cascadeDelete: true)
                .Index(t => t.EmployeeID)
                .Index(t => t.FacilityId);
            
            CreateTable(
                "dbo.Facilities",
                c => new
                    {
                        FacilityId = c.Int(nullable: false, identity: true),
                        FacilityName = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Landmark = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(maxLength: 6),
                    })
                .PrimaryKey(t => t.FacilityId);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        ResourceId = c.Int(nullable: false, identity: true),
                        ResourceName = c.String(nullable: false),
                        Quantity = c.String(nullable: false),
                        Description = c.String(),
                        Size = c.String(),
                        Color = c.String(),
                        FacilityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ResourceId)
                .ForeignKey("dbo.Facilities", t => t.FacilityId, cascadeDelete: true)
                .Index(t => t.FacilityId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Resources", "FacilityId", "dbo.Facilities");
            DropForeignKey("dbo.EmployeeToFacilities", "FacilityId", "dbo.Facilities");
            DropForeignKey("dbo.EmployeeToFacilities", "EmployeeID", "dbo.Employees");
            DropIndex("dbo.Resources", new[] { "FacilityId" });
            DropIndex("dbo.EmployeeToFacilities", new[] { "FacilityId" });
            DropIndex("dbo.EmployeeToFacilities", new[] { "EmployeeID" });
            DropTable("dbo.Resources");
            DropTable("dbo.Facilities");
            DropTable("dbo.EmployeeToFacilities");
            DropTable("dbo.Employees");
        }
    }
}
