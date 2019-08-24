namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fix : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Apartments", "ApartmentId", "dbo.Locations");
            DropForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Amenities", "ApartmentId", "dbo.Apartments");
            DropIndex("dbo.Apartments", new[] { "ApartmentId" });
            DropPrimaryKey("dbo.Apartments");
            AlterColumn("dbo.Apartments", "ApartmentId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Apartments", "ApartmentId");
            CreateIndex("dbo.Locations", "ApartmentId");
            AddForeignKey("dbo.Locations", "ApartmentId", "dbo.Apartments", "ApartmentId", cascadeDelete: true);
            AddForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments", "ApartmentId", cascadeDelete: true);
            AddForeignKey("dbo.Amenities", "ApartmentId", "dbo.Apartments", "ApartmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Amenities", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Locations", "ApartmentId", "dbo.Apartments");
            DropIndex("dbo.Locations", new[] { "ApartmentId" });
            DropPrimaryKey("dbo.Apartments");
            AlterColumn("dbo.Apartments", "ApartmentId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Apartments", "ApartmentId");
            CreateIndex("dbo.Apartments", "ApartmentId");
            AddForeignKey("dbo.Amenities", "ApartmentId", "dbo.Apartments", "ApartmentId", cascadeDelete: true);
            AddForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments", "ApartmentId", cascadeDelete: true);
            AddForeignKey("dbo.Apartments", "ApartmentId", "dbo.Locations", "LocationId");
        }
    }
}
