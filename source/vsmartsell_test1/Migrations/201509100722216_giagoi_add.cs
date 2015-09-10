namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class giagoi_add : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhachHangs", "GiaGoi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            DropColumn("dbo.KhachHangs", "Notify");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KhachHangs", "Notify", c => c.DateTime());
            DropColumn("dbo.KhachHangs", "GiaGoi");
        }
    }
}
