namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatedWerkbrief : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WerkbriefHours",
                c => new
                    {
                        WerkbriefHoursID = c.Int(nullable: false, identity: true),
                        WerkbriefID = c.Int(nullable: false),
                        Week = c.String(nullable: false),
                        ProductID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        Monday = c.String(),
                        Tuesday = c.String(),
                        Wednesday = c.String(),
                        Thursday = c.String(),
                        Friday = c.String(),
                        Saturday = c.String(),
                        Sunday = c.String(),
                        TotalHours = c.String(),
                    })
                .PrimaryKey(t => t.WerkbriefHoursID)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.Werkbrief", t => t.WerkbriefID, cascadeDelete: true)
                .Index(t => t.WerkbriefID)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.WerkbriefHoursTemp",
                c => new
                    {
                        WerkbriefHoursIDTemp = c.Int(nullable: false, identity: true),
                        WerkbriefIDTemp = c.Int(nullable: false),
                        Week = c.String(nullable: false),
                        ProductID = c.Int(nullable: false),
                        ProjectID = c.Int(nullable: false),
                        Monday = c.String(),
                        Tuesday = c.String(),
                        Wednesday = c.String(),
                        Thursday = c.String(),
                        Friday = c.String(),
                        Saturday = c.String(),
                        Sunday = c.String(),
                        TotalHours = c.String(),
                    })
                .PrimaryKey(t => t.WerkbriefHoursIDTemp)
                .ForeignKey("dbo.Projects", t => t.ProjectID, cascadeDelete: true)
                .ForeignKey("dbo.WerkbriefTemp", t => t.WerkbriefIDTemp, cascadeDelete: true)
                .Index(t => t.WerkbriefIDTemp)
                .Index(t => t.ProjectID);
            
            CreateTable(
                "dbo.Werkbrief",
                c => new
                    {
                        WerkbriefID = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(),
                        WerkbriefDate = c.String(),
                        WerkbriefEndDate = c.String(),
                        Period = c.String(),
                        TotalHours = c.Int(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        UserName = c.String(maxLength: 500),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WerkbriefID);
            
            CreateTable(
                "dbo.WerkbriefTemp",
                c => new
                    {
                        WerkbriefIDTemp = c.Int(nullable: false, identity: true),
                        CompanyID = c.Int(),
                        WerkbriefDate = c.String(),
                        WerkbriefEndDate = c.String(),
                        Period = c.String(),
                        TotalHours = c.Int(),
                        DateCreated = c.DateTime(),
                        DateModified = c.DateTime(),
                        UserName = c.String(maxLength: 500),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WerkbriefIDTemp);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WerkbriefHoursTemp", "WerkbriefIDTemp", "dbo.WerkbriefTemp");
            DropForeignKey("dbo.WerkbriefHours", "WerkbriefID", "dbo.Werkbrief");
            DropForeignKey("dbo.WerkbriefHoursTemp", "ProjectID", "dbo.Projects");
            DropForeignKey("dbo.WerkbriefHours", "ProjectID", "dbo.Projects");
            DropIndex("dbo.WerkbriefHoursTemp", new[] { "ProjectID" });
            DropIndex("dbo.WerkbriefHoursTemp", new[] { "WerkbriefIDTemp" });
            DropIndex("dbo.WerkbriefHours", new[] { "ProjectID" });
            DropIndex("dbo.WerkbriefHours", new[] { "WerkbriefID" });
            DropTable("dbo.WerkbriefTemp");
            DropTable("dbo.Werkbrief");
            DropTable("dbo.WerkbriefHoursTemp");
            DropTable("dbo.WerkbriefHours");
        }
    }
}
