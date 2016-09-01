namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCompanyToWerkbrief : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Werkbrief", "CompanyID");
            AddForeignKey("dbo.Werkbrief", "CompanyID", "dbo.Companies", "CompanyID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Werkbrief", "CompanyID", "dbo.Companies");
            DropIndex("dbo.Werkbrief", new[] { "CompanyID" });
        }
    }
}
