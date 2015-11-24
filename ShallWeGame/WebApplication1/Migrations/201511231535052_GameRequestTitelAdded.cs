namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GameRequestTitelAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GameRequests", "titel", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.GameRequests", "titel");
        }
    }
}
