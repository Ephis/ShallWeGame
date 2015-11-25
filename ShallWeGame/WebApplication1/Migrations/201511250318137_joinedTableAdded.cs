namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class joinedTableAdded : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Invites", "GameRequest_id", "dbo.GameRequests");
            DropIndex("dbo.Invites", new[] { "GameRequest_id" });
            CreateTable(
                "dbo.InviteRequests",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        gameRequest_id = c.Int(),
                        invite_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.GameRequests", t => t.gameRequest_id)
                .ForeignKey("dbo.Invites", t => t.invite_id)
                .Index(t => t.gameRequest_id)
                .Index(t => t.invite_id);
            
            DropColumn("dbo.Invites", "GameRequest_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Invites", "GameRequest_id", c => c.Int());
            DropForeignKey("dbo.InviteRequests", "invite_id", "dbo.Invites");
            DropForeignKey("dbo.InviteRequests", "gameRequest_id", "dbo.GameRequests");
            DropIndex("dbo.InviteRequests", new[] { "invite_id" });
            DropIndex("dbo.InviteRequests", new[] { "gameRequest_id" });
            DropTable("dbo.InviteRequests");
            CreateIndex("dbo.Invites", "GameRequest_id");
            AddForeignKey("dbo.Invites", "GameRequest_id", "dbo.GameRequests", "id");
        }
    }
}
