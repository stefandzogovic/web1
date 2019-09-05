namespace Web_projekat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class jsontimes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Apartments", "Times", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Apartments", "Times");
        }
    }
}
