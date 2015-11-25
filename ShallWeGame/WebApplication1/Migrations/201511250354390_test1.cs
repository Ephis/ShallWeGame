namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GameRequests", "gameToPlay_id", "dbo.Games");
            DropIndex("dbo.GameRequests", new[] { "gameToPlay_id" });
            AddColumn("dbo.GameRequests", "gameToPlayId", c => c.Int(nullable: false));
            DropColumn("dbo.GameRequests", "gameToPlay_id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GameRequests", "gameToPlay_id", c => c.Int());
            DropColumn("dbo.GameRequests", "gameToPlayId");
            CreateIndex("dbo.GameRequests", "gameToPlay_id");
            AddForeignKey("dbo.GameRequests", "gameToPlay_id", "dbo.Games", "id");
        }
    }
}
