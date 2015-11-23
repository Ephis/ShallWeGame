namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        userId = c.String(),
                        Account_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Accounts", t => t.Account_id)
                .Index(t => t.Account_id);
            
            CreateTable(
                "dbo.FreindRequests",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        requestStatus = c.Int(nullable: false),
                        requestMadeAt = c.DateTime(nullable: false),
                        reciver_id = c.Int(),
                        sender_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Accounts", t => t.reciver_id)
                .ForeignKey("dbo.Accounts", t => t.sender_id)
                .Index(t => t.reciver_id)
                .Index(t => t.sender_id);
            
            CreateTable(
                "dbo.GameRequests",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        requestCreatedAt = c.DateTime(nullable: false),
                        timeItBegins = c.DateTime(nullable: false),
                        timeItEnds = c.DateTime(nullable: false),
                        playersNeed = c.Int(nullable: false),
                        gameToPlay_id = c.Int(),
                        owner_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Games", t => t.gameToPlay_id)
                .ForeignKey("dbo.Accounts", t => t.owner_id)
                .Index(t => t.gameToPlay_id)
                .Index(t => t.owner_id);
            
            CreateTable(
                "dbo.Games",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Invites",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        inviteStatus = c.Int(nullable: false),
                        madeAt = c.DateTime(nullable: false),
                        reciver_id = c.Int(),
                        GameRequest_id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.Accounts", t => t.reciver_id)
                .ForeignKey("dbo.GameRequests", t => t.GameRequest_id)
                .Index(t => t.reciver_id)
                .Index(t => t.GameRequest_id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        firstLogin = c.Boolean(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.GameRequests", "owner_id", "dbo.Accounts");
            DropForeignKey("dbo.Invites", "GameRequest_id", "dbo.GameRequests");
            DropForeignKey("dbo.Invites", "reciver_id", "dbo.Accounts");
            DropForeignKey("dbo.GameRequests", "gameToPlay_id", "dbo.Games");
            DropForeignKey("dbo.FreindRequests", "sender_id", "dbo.Accounts");
            DropForeignKey("dbo.FreindRequests", "reciver_id", "dbo.Accounts");
            DropForeignKey("dbo.Accounts", "Account_id", "dbo.Accounts");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Invites", new[] { "GameRequest_id" });
            DropIndex("dbo.Invites", new[] { "reciver_id" });
            DropIndex("dbo.GameRequests", new[] { "owner_id" });
            DropIndex("dbo.GameRequests", new[] { "gameToPlay_id" });
            DropIndex("dbo.FreindRequests", new[] { "sender_id" });
            DropIndex("dbo.FreindRequests", new[] { "reciver_id" });
            DropIndex("dbo.Accounts", new[] { "Account_id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Invites");
            DropTable("dbo.Games");
            DropTable("dbo.GameRequests");
            DropTable("dbo.FreindRequests");
            DropTable("dbo.Accounts");
        }
    }
}
