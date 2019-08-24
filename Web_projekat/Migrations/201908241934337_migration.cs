namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Amenities",
                c => new
                    {
                        AmenityId = c.Int(nullable: false, identity: true),
                        type = c.Int(nullable: false),
                        name = c.String(),
                        ApartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AmenityId)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId, cascadeDelete: true)
                .Index(t => t.ApartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Amenities", "ApartmentId", "dbo.Apartments");
            DropIndex("dbo.Amenities", new[] { "ApartmentId" });
            DropTable("dbo.Amenities");
        }
    }
}
