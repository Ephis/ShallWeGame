namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class JoinedTableRemoved : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.InviteRequests", "gameRequest_id", "dbo.GameRequests");
            DropForeignKey("dbo.InviteRequests", "invite_id", "dbo.Invites");
            DropIndex("dbo.InviteRequests", new[] { "gameRequest_id" });
            DropIndex("dbo.InviteRequests", new[] { "invite_id" });
            DropTable("dbo.InviteRequests");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.InviteRequests",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        gameRequest_id = c.Int(),
                        invite_id = c.Int(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateIndex("dbo.InviteRequests", "invite_id");
            CreateIndex("dbo.InviteRequests", "gameRequest_id");
            AddForeignKey("dbo.InviteRequests", "invite_id", "dbo.Invites", "id");
            AddForeignKey("dbo.InviteRequests", "gameRequest_id", "dbo.GameRequests", "id");
        }
    }
}
