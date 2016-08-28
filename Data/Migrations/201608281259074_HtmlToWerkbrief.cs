namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HtmlToWerkbrief : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Werkbrief", "WerkbriefHTML", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Werkbrief", "WerkbriefHTML");
        }
    }
}
