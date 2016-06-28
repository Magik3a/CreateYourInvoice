namespace akcetDB.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _201606271704526_AutomaticMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProductInvoice", "ProductPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProductInvoice", "Quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProductInvoice", "TotalPrice", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ProductInvoiceTemp", "ProductID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductInvoiceTemp", "DdsID", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductInvoiceTemp", "ProductPrice", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ProductInvoiceTemp", "Quanity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductInvoiceTemp", "Quanity", c => c.Int());
            AlterColumn("dbo.ProductInvoiceTemp", "ProductPrice", c => c.Int());
            AlterColumn("dbo.ProductInvoiceTemp", "DdsID", c => c.Int());
            AlterColumn("dbo.ProductInvoiceTemp", "ProductID", c => c.Int());
            AlterColumn("dbo.ProductInvoice", "TotalPrice", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductInvoice", "Quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductInvoice", "ProductPrice", c => c.Int(nullable: false));
  
        }
    }
}
