namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTOtalHours : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Werkbrief", "TotalHours", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Werkbrief", "TotalHours", c => c.Int());
        }
    }
}
