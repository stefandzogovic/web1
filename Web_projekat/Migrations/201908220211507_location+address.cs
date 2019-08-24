namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class locationaddress : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments");
            DropPrimaryKey("dbo.Apartments");
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        LocationId = c.Int(nullable: false),
                        latitude = c.Double(nullable: false),
                        longitude = c.Double(nullable: false),
                        ApartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.LocationId)
                .ForeignKey("dbo.Addresses", t => t.LocationId)
                .Index(t => t.LocationId);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        street = c.String(),
                        number = c.Int(nullable: false),
                        city = c.String(),
                        postal_code = c.Long(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AddressId);
            
            AlterColumn("dbo.Apartments", "ApartmentId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Apartments", "ApartmentId");
            CreateIndex("dbo.Apartments", "ApartmentId");
            AddForeignKey("dbo.Apartments", "ApartmentId", "dbo.Locations", "LocationId");
            AddForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments", "ApartmentId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments");
            DropForeignKey("dbo.Apartments", "ApartmentId", "dbo.Locations");
            DropForeignKey("dbo.Locations", "LocationId", "dbo.Addresses");
            DropIndex("dbo.Locations", new[] { "LocationId" });
            DropIndex("dbo.Apartments", new[] { "ApartmentId" });
            DropPrimaryKey("dbo.Apartments");
            AlterColumn("dbo.Apartments", "ApartmentId", c => c.Int(nullable: false, identity: true));
            DropTable("dbo.Addresses");
            DropTable("dbo.Locations");
            AddPrimaryKey("dbo.Apartments", "ApartmentId");
            AddForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments", "ApartmentId", cascadeDelete: true);
        }
    }
}
