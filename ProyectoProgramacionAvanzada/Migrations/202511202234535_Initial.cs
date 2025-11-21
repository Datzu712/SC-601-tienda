namespace ProyectoProgramacionAvanzada.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.product",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 100, unicode: false),
                        price = c.Decimal(nullable: false, precision: 10, scale: 2),
                        stock = c.Int(nullable: false),
                        status = c.Boolean(nullable: false),
                        created_at = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.Product_Images",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        product_id = c.Int(nullable: false),
                        image_url = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.product", t => t.product_id, cascadeDelete: true)
                .Index(t => t.product_id);
            
            CreateTable(
                "dbo.Product_Reviews",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        content = c.String(nullable: false, unicode: false),
                        approved = c.Boolean(nullable: false),
                        product_id = c.Int(nullable: false),
                        author_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.user", t => t.author_id, cascadeDelete: true)
                .ForeignKey("dbo.product", t => t.product_id, cascadeDelete: true)
                .Index(t => t.product_id)
                .Index(t => t.author_id);
            
            CreateTable(
                "dbo.user",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        username = c.String(nullable: false, maxLength: 100, unicode: false),
                        hashed_password = c.String(nullable: false, maxLength: 500, unicode: false),
                        last_connection = c.DateTime(),
                        enabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.role",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.user_role",
                c => new
                    {
                        Role_id = c.Int(nullable: false),
                        User_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Role_id, t.User_id })
                .ForeignKey("dbo.role", t => t.Role_id, cascadeDelete: true)
                .ForeignKey("dbo.user", t => t.User_id, cascadeDelete: true)
                .Index(t => t.Role_id)
                .Index(t => t.User_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Product_Reviews", "product_id", "dbo.product");
            DropForeignKey("dbo.user_role", "User_id", "dbo.user");
            DropForeignKey("dbo.user_role", "Role_id", "dbo.role");
            DropForeignKey("dbo.Product_Reviews", "author_id", "dbo.user");
            DropForeignKey("dbo.Product_Images", "product_id", "dbo.product");
            DropIndex("dbo.user_role", new[] { "User_id" });
            DropIndex("dbo.user_role", new[] { "Role_id" });
            DropIndex("dbo.Product_Reviews", new[] { "author_id" });
            DropIndex("dbo.Product_Reviews", new[] { "product_id" });
            DropIndex("dbo.Product_Images", new[] { "product_id" });
            DropTable("dbo.user_role");
            DropTable("dbo.role");
            DropTable("dbo.user");
            DropTable("dbo.Product_Reviews");
            DropTable("dbo.Product_Images");
            DropTable("dbo.product");
        }
    }
}
