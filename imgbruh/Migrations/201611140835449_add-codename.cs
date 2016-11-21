namespace imgbruh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcodename : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Imgs", "CodeName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Imgs", "CodeName");
        }
    }
}
