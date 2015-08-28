namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class validation : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KhachHangs", "TenKH", c => c.String(nullable: false));
            AlterColumn("dbo.KhachHangs", "Phone", c => c.String(nullable: false, maxLength: 13));
            AlterColumn("dbo.KhachHangs", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.KhachHangs", "TenCH", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KhachHangs", "TenCH", c => c.String());
            AlterColumn("dbo.KhachHangs", "Email", c => c.String());
            AlterColumn("dbo.KhachHangs", "Phone", c => c.String());
            AlterColumn("dbo.KhachHangs", "TenKH", c => c.String());
        }
    }
}
