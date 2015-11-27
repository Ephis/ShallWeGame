namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RandomChange : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Freinds", newName: "Friends");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Friends", newName: "Freinds");
        }
    }
}
