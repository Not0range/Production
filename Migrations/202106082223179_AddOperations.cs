namespace Production.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOperations : DbMigration
    {
        public override void Up()
        {
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
            
            AlterColumn("dbo.Accounts", "Password", c => c.String(nullable: false, maxLength: 32));
            AlterColumn("dbo.Orders", "Title", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Orders", "Characteristics", c => c.String(nullable: false, maxLength: 300));
            AlterColumn("dbo.Orders", "Documentation", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Parts", "Title", c => c.String(nullable: false, maxLength: 100));
            CreateIndex("dbo.Orders", "Number");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartOperations", "PartID", "dbo.Parts");
            DropForeignKey("dbo.OrderOperations", "OrderID", "dbo.Orders");
            DropIndex("dbo.PartOperations", new[] { "PartID" });
            DropIndex("dbo.OrderOperations", new[] { "OrderID" });
            DropIndex("dbo.Orders", new[] { "Number" });
            AlterColumn("dbo.Parts", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Documentation", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Characteristics", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Title", c => c.String(nullable: false));
            AlterColumn("dbo.Accounts", "Password", c => c.String(nullable: false));
            DropTable("dbo.PartOperations");
            DropTable("dbo.OrderOperations");
        }
    }
}
