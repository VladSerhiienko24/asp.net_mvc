namespace Tours_1._0.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Hostels",
                c => new
                    {
                        HostelID = c.Int(nullable: false, identity: true),
                        HostelName = c.String(maxLength: 30),
                        HostelMark = c.Int(nullable: false),
                        Website = c.String(maxLength: 60),
                    })
                .PrimaryKey(t => t.HostelID);
            
            CreateTable(
                "dbo.ResponseHostels",
                c => new
                    {
                        ResponseID = c.Int(nullable: false, identity: true),
                        HostelID = c.Int(),
                        UserId = c.String(maxLength: 128),
                        ResponseName = c.String(maxLength: 70),
                        Mark = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Hostels", t => t.HostelID)
                .Index(t => t.HostelID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(maxLength: 30),
                        LastName = c.String(maxLength: 30),
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
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        TourID = c.Int(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateOrder = c.String(),
                        StatusOrderID = c.Int(),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.StatusOrders", t => t.StatusOrderID)
                .ForeignKey("dbo.Tours", t => t.TourID)
                .Index(t => t.UserId)
                .Index(t => t.TourID)
                .Index(t => t.StatusOrderID);
            
            CreateTable(
                "dbo.StatusOrders",
                c => new
                    {
                        StatusOrderID = c.Int(nullable: false, identity: true),
                        StatusOrderName = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.StatusOrderID);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Tours",
                c => new
                    {
                        TourID = c.Int(nullable: false, identity: true),
                        TourName = c.String(maxLength: 30),
                        TypeTourID = c.Int(),
                        TourSights = c.String(maxLength: 100),
                        DateStart = c.String(maxLength: 25),
                        DateEnd = c.String(maxLength: 25),
                        HostelID = c.Int(),
                        StatusHot = c.Boolean(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.TourID)
                .ForeignKey("dbo.Hostels", t => t.HostelID)
                .ForeignKey("dbo.TypeTours", t => t.TypeTourID)
                .Index(t => t.TypeTourID)
                .Index(t => t.HostelID);
            
            CreateTable(
                "dbo.ResponseTours",
                c => new
                    {
                        ResponseID = c.Int(nullable: false, identity: true),
                        TourID = c.Int(),
                        UserId = c.String(maxLength: 128),
                        ResponseName = c.String(maxLength: 70),
                        Mark = c.Int(nullable: false),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ResponseID)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.Tours", t => t.TourID)
                .Index(t => t.TourID)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.TypeTours",
                c => new
                    {
                        TypeTourID = c.Int(nullable: false, identity: true),
                        TypeTourName = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.TypeTourID);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ResponseHostels", "HostelID", "dbo.Hostels");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ResponseHostels", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Tours", "TypeTourID", "dbo.TypeTours");
            DropForeignKey("dbo.ResponseTours", "TourID", "dbo.Tours");
            DropForeignKey("dbo.ResponseTours", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "TourID", "dbo.Tours");
            DropForeignKey("dbo.Tours", "HostelID", "dbo.Hostels");
            DropForeignKey("dbo.Tickets", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "StatusOrderID", "dbo.StatusOrders");
            DropForeignKey("dbo.Orders", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.ResponseTours", new[] { "UserId" });
            DropIndex("dbo.ResponseTours", new[] { "TourID" });
            DropIndex("dbo.Tours", new[] { "HostelID" });
            DropIndex("dbo.Tours", new[] { "TypeTourID" });
            DropIndex("dbo.Tickets", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "StatusOrderID" });
            DropIndex("dbo.Orders", new[] { "TourID" });
            DropIndex("dbo.Orders", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.ResponseHostels", new[] { "UserId" });
            DropIndex("dbo.ResponseHostels", new[] { "HostelID" });
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.TypeTours");
            DropTable("dbo.ResponseTours");
            DropTable("dbo.Tours");
            DropTable("dbo.Tickets");
            DropTable("dbo.StatusOrders");
            DropTable("dbo.Orders");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.ResponseHostels");
            DropTable("dbo.Hostels");
        }
    }
}
