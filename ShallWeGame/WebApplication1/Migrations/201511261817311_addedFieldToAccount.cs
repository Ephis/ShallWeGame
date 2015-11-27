namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedFieldToAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "isUsersAccount", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "isUsersAccount");
        }
    }
}
