namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class banggia2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.KhachHangs", "LoaiGoi", c => c.String(maxLength: 128));
            CreateIndex("dbo.KhachHangs", "LoaiGoi");
            AddForeignKey("dbo.KhachHangs", "LoaiGoi", "dbo.BangGias", "LoaiGoi");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KhachHangs", "LoaiGoi", "dbo.BangGias");
            DropIndex("dbo.KhachHangs", new[] { "LoaiGoi" });
            DropColumn("dbo.KhachHangs", "LoaiGoi");
        }
    }
}
