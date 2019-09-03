namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isdeleted : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Amenities", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Apartments", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Photos", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Users", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Locations", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.Addresses", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Addresses", "IsDeleted");
            DropColumn("dbo.Locations", "IsDeleted");
            DropColumn("dbo.Users", "IsDeleted");
            DropColumn("dbo.Photos", "IsDeleted");
            DropColumn("dbo.Apartments", "IsDeleted");
            DropColumn("dbo.Amenities", "IsDeleted");
        }
    }
}
