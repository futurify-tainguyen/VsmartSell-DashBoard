namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class notify_day : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhachHangs", "Notify", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.KhachHangs", "Notify");
        }
    }
}
