namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControllerFirstTry : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameRequests", "playersNeeded", c => c.Int(nullable: false));
            AddColumn("dbo.Invites", "priority", c => c.Int(nullable: false));
            DropColumn("dbo.GameRequests", "playersNeed");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GameRequests", "playersNeed", c => c.Int(nullable: false));
            DropColumn("dbo.Invites", "priority");
            DropColumn("dbo.GameRequests", "playersNeeded");
        }
    }
}
