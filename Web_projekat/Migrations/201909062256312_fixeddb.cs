namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixeddb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        street = c.String(),
                        number = c.Int(nullable: false),
                        city = c.String(),
                        postal_code = c.Long(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        LocationId = c.Int(),
                        Location_LocationId1 = c.Int(),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.Locations", t => t.LocationId)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId1)
                .Index(t => t.LocationId)
                .Index(t => t.Location_LocationId1);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false, identity: true),
                        latitude = c.Double(nullable: false),
                        longitude = c.Double(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        ApartmentId = c.Int(),
                        Apartment_ApartmentId1 = c.Int(),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId)
                .ForeignKey("dbo.Apartments", t => t.Apartment_ApartmentId1)
                .Index(t => t.ApartmentId)
                .Index(t => t.Apartment_ApartmentId1);
            
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
                        Times = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ApartmentId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Amenities",
                c => new
                    {
                        AmenityId = c.Int(nullable: false, identity: true),
                        type = c.Int(nullable: false),
                        name = c.String(),
                        ApartmentId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AmenityId)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId, cascadeDelete: true)
                .Index(t => t.ApartmentId);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ImageBytes = c.Binary(),
                        ApartmentId = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoId)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId, cascadeDelete: true)
                .Index(t => t.ApartmentId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        username = c.String(),
                        password = c.String(),
                        name = c.String(),
                        surname = c.String(),
                        email = c.String(),
                        sex = c.Int(nullable: false),
                        role = c.Int(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.AvailableAmenities",
                c => new
                    {
                        AvailableAmenitiesId = c.Int(nullable: false, identity: true),
                        type = c.Int(nullable: false),
                        name = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.AvailableAmenitiesId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Addresses", "Location_LocationId1", "dbo.Locations");
            DropForeignKey("dbo.Locations", "Apartment_ApartmentId1", "dbo.Apartments");
            DropForeignKey("dbo.Apartments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Locations", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Amenities", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Addresses", "LocationId", "dbo.Locations");
            DropIndex("dbo.Photos", new[] { "ApartmentId" });
            DropIndex("dbo.Amenities", new[] { "ApartmentId" });
            DropIndex("dbo.Apartments", new[] { "UserId" });
            DropIndex("dbo.Locations", new[] { "Apartment_ApartmentId1" });
            DropIndex("dbo.Locations", new[] { "ApartmentId" });
            DropIndex("dbo.Addresses", new[] { "Location_LocationId1" });
            DropIndex("dbo.Addresses", new[] { "LocationId" });
            DropTable("dbo.AvailableAmenities");
            DropTable("dbo.Users");
            DropTable("dbo.Photos");
            DropTable("dbo.Amenities");
            DropTable("dbo.Apartments");
            DropTable("dbo.Locations");
            DropTable("dbo.Addresses");
        }
    }
}
