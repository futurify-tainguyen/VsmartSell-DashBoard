namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class khachhang_phone : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KhachHangs", "Phone", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KhachHangs", "Phone", c => c.Int(nullable: false));
        }
    }
}
