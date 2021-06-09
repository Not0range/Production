namespace Production.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrderAndOther : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.Long(nullable: false),
                        Title = c.String(nullable: false),
                        Characteristics = c.String(nullable: false),
                        Documentation = c.String(nullable: false),
                        OrderDate = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        Title = c.String(nullable: false),
                        Count = c.Int(nullable: false),
                        Parts = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);
            
            AddColumn("dbo.Accounts", "Password", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Parts", "OrderID", "dbo.Orders");
            DropIndex("dbo.Parts", new[] { "OrderID" });
            DropColumn("dbo.Accounts", "Password");
            DropTable("dbo.Parts");
            DropTable("dbo.Orders");
        }
    }
}
