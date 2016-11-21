namespace imgbruh.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedindexes : DbMigration
    {
        public override void Up()
        {
            RenameIndex(table: "dbo.AspNetUsers", name: "UserNameIndex", newName: "IX_Username");
            AlterColumn("dbo.Imgs", "CodeName", c => c.String(nullable: false, maxLength: 75));
            CreateIndex("dbo.Imgs", "CodeName", unique: true, name: "IX_Codename");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Imgs", "IX_Codename");
            AlterColumn("dbo.Imgs", "CodeName", c => c.String());
            RenameIndex(table: "dbo.AspNetUsers", name: "IX_Username", newName: "UserNameIndex");
        }
    }
}
