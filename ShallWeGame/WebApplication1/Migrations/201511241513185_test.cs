namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.FreindRequests", newName: "FriendRequests");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.FriendRequests", newName: "FreindRequests");
        }
    }
}
