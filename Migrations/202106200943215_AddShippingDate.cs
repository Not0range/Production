namespace Production.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddShippingDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "ShippingDate", c => c.DateTime(storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "ShippingDate");
        }
    }
}
