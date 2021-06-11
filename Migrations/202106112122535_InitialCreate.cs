namespace Production.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Guild = c.Int(nullable: false),
                        Password = c.String(nullable: false, maxLength: 32),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderOperations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Guild = c.Int(nullable: false),
                        Brigade = c.Int(nullable: false),
                        WorkPlace = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Number = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Characteristics = c.String(nullable: false, maxLength: 300),
                        Documentation = c.String(nullable: false, maxLength: 100),
                        OrderDate = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => t.ID)
                .Index(t => t.Number);
            
            CreateTable(
                "dbo.Parts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Count = c.Int(nullable: false),
                        Parts = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.PartOperations",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PartID = c.Int(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Guild = c.Int(nullable: false),
                        Brigade = c.Int(nullable: false),
                        WorkPlace = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Parts", t => t.PartID, cascadeDelete: true)
                .Index(t => t.PartID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Parts", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.PartOperations", "PartID", "dbo.Parts");
            DropForeignKey("dbo.OrderOperations", "OrderID", "dbo.Orders");
            DropIndex("dbo.PartOperations", new[] { "PartID" });
            DropIndex("dbo.Parts", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "Number" });
            DropIndex("dbo.OrderOperations", new[] { "OrderID" });
            DropTable("dbo.PartOperations");
            DropTable("dbo.Parts");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderOperations");
            DropTable("dbo.Accounts");
        }
    }
}
