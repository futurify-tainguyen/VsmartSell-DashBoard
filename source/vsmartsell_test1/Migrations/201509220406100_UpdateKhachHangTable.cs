namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateKhachHangTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.KhachHangs", "LoaiKH", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.KhachHangs", "LoaiKH", c => c.String());
        }
    }
}
