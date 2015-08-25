namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_viewid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhachHangs", "Viewid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KhachHangs", "Viewid");
        }
    }
}
