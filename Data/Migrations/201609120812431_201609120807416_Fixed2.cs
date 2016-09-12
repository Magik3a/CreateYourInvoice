namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _201609120807416_Fixed2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Werkbrief", "WerkbriefHTML", c => c.String());
            AlterColumn("dbo.Werkbrief", "TotalHours", c => c.String());
            CreateIndex("dbo.Werkbrief", "CompanyID");
            AddForeignKey("dbo.Werkbrief", "CompanyID", "dbo.Companies", "CompanyID");
        }

        public override void Down()
        {
            DropColumn("dbo.Werkbrief", "WerkbriefHTML");
            AlterColumn("dbo.Werkbrief", "TotalHours", c => c.Int());
            DropForeignKey("dbo.Werkbrief", "CompanyID", "dbo.Companies");
            DropIndex("dbo.Werkbrief", new[] { "CompanyID" });
        }
    }
}
