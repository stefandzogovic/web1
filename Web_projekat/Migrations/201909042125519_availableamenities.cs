namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class availableamenities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AvailableAmenities",
                c => new
                    {
                        AvailableAmenitiesId = c.Int(nullable: false, identity: true),
                        type = c.Int(nullable: false),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.AvailableAmenitiesId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AvailableAmenities");
        }
    }
}
