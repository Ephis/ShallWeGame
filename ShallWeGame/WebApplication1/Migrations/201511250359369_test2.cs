namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameRequests", "gameToPlay_id", c => c.Int());
            CreateIndex("dbo.GameRequests", "gameToPlay_id");
            AddForeignKey("dbo.GameRequests", "gameToPlay_id", "dbo.Games", "id");
            DropColumn("dbo.GameRequests", "gameToPlayId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GameRequests", "gameToPlayId", c => c.Int(nullable: false));
            DropForeignKey("dbo.GameRequests", "gameToPlay_id", "dbo.Games");
            DropIndex("dbo.GameRequests", new[] { "gameToPlay_id" });
            DropColumn("dbo.GameRequests", "gameToPlay_id");
        }
    }
}
