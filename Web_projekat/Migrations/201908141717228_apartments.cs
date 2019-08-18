namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class apartments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Apartments",
                c => new
                    {
                        ApartmentId = c.Int(nullable: false, identity: true),
                        type = c.Int(nullable: false),
                        number_of_rooms = c.Int(nullable: false),
                        number_of_guests = c.Int(nullable: false),
                        active = c.Boolean(nullable: false),
                        price_per_night = c.Double(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApartmentId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Apartments", "UserId", "dbo.Users");
            DropIndex("dbo.Apartments", new[] { "UserId" });
            DropTable("dbo.Apartments");
        }
    }
}
