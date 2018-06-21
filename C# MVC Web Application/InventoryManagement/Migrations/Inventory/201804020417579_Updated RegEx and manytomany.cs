namespace InventoryManagement.Migrations.Inventory
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedRegExandmanytomany : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.EmployeeToFacilities", "EmployeeID", "dbo.Employees");
            DropForeignKey("dbo.EmployeeToFacilities", "FacilityId", "dbo.Facilities");
            DropIndex("dbo.EmployeeToFacilities", new[] { "EmployeeID" });
            DropIndex("dbo.EmployeeToFacilities", new[] { "FacilityId" });
            CreateTable(
                "dbo.FacilityEmployees",
                c => new
                    {
                        Facility_FacilityId = c.Int(nullable: false),
                        Employee_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Facility_FacilityId, t.Employee_Id })
                .ForeignKey("dbo.Facilities", t => t.Facility_FacilityId, cascadeDelete: true)
                .ForeignKey("dbo.Employees", t => t.Employee_Id, cascadeDelete: true)
                .Index(t => t.Facility_FacilityId)
                .Index(t => t.Employee_Id);
            
            AlterColumn("dbo.Employees", "UserName", c => c.String(nullable: false));
            AlterColumn("dbo.Employees", "Email", c => c.String(nullable: false));
            DropTable("dbo.EmployeeToFacilities");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.EmployeeToFacilities",
                c => new
                    {
                        EmployeeToFacilityId = c.Int(nullable: false, identity: true),
                        EmployeeID = c.Int(nullable: false),
                        FacilityId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeToFacilityId);
            
            DropForeignKey("dbo.FacilityEmployees", "Employee_Id", "dbo.Employees");
            DropForeignKey("dbo.FacilityEmployees", "Facility_FacilityId", "dbo.Facilities");
            DropIndex("dbo.FacilityEmployees", new[] { "Employee_Id" });
            DropIndex("dbo.FacilityEmployees", new[] { "Facility_FacilityId" });
            AlterColumn("dbo.Employees", "Email", c => c.String());
            AlterColumn("dbo.Employees", "UserName", c => c.String());
            DropTable("dbo.FacilityEmployees");
            CreateIndex("dbo.EmployeeToFacilities", "FacilityId");
            CreateIndex("dbo.EmployeeToFacilities", "EmployeeID");
            AddForeignKey("dbo.EmployeeToFacilities", "FacilityId", "dbo.Facilities", "FacilityId", cascadeDelete: true);
            AddForeignKey("dbo.EmployeeToFacilities", "EmployeeID", "dbo.Employees", "Id", cascadeDelete: true);
        }
    }
}
