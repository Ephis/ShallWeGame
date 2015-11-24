namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class freinds : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Accounts", "Account_id", "dbo.Accounts");
            DropIndex("dbo.Accounts", new[] { "Account_id" });
            CreateTable(
                "dbo.Freinds",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        reciver_id = c.Int(),
                        sender_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Accounts", t => t.reciver_id)
                .ForeignKey("dbo.Accounts", t => t.sender_id)
                .Index(t => t.reciver_id)
                .Index(t => t.sender_id);
            
            DropColumn("dbo.Accounts", "Account_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "Account_id", c => c.Int());
            DropForeignKey("dbo.Freinds", "sender_id", "dbo.Accounts");
            DropForeignKey("dbo.Freinds", "reciver_id", "dbo.Accounts");
            DropIndex("dbo.Freinds", new[] { "sender_id" });
            DropIndex("dbo.Freinds", new[] { "reciver_id" });
            DropTable("dbo.Freinds");
            CreateIndex("dbo.Accounts", "Account_id");
            AddForeignKey("dbo.Accounts", "Account_id", "dbo.Accounts", "id");
        }
    }
}
