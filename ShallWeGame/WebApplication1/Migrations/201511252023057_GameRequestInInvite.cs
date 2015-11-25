namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameRequestInInvite : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Invites", "gameRequest_id", c => c.Int());
            CreateIndex("dbo.Invites", "gameRequest_id");
            AddForeignKey("dbo.Invites", "gameRequest_id", "dbo.GameRequests", "id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invites", "gameRequest_id", "dbo.GameRequests");
            DropIndex("dbo.Invites", new[] { "gameRequest_id" });
            DropColumn("dbo.Invites", "gameRequest_id");
        }
    }
}
