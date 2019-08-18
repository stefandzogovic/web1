namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class photo_class : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        PhotoId = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        ImageBytes = c.Binary(),
                        ApartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PhotoId)
                .ForeignKey("dbo.Apartments", t => t.ApartmentId, cascadeDelete: true)
                .Index(t => t.ApartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "ApartmentId", "dbo.Apartments");
            DropIndex("dbo.Photos", new[] { "ApartmentId" });
            DropTable("dbo.Photos");
        }
    }
}
