namespace imgbruh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modifiedimgs : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imgs", "ContentType", c => c.String());
            AddColumn("dbo.Imgs", "FileName", c => c.String());
            AddColumn("dbo.Imgs", "LookupId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Imgs", "LookupId");
            DropColumn("dbo.Imgs", "FileName");
            DropColumn("dbo.Imgs", "ContentType");
        }
    }
}
