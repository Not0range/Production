namespace Production.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShippingDateNotNull : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "ShippingDate", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "ShippingDate", c => c.DateTime(storeType: "date"));
        }
    }
}
