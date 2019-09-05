namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isdeletedavaialableamenities : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AvailableAmenities", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AvailableAmenities", "IsDeleted");
        }
    }
}
