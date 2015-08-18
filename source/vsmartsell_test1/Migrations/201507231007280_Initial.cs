namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.KhachHangs",
                c => new
                    {
                        MaKH = c.Int(nullable: false, identity: true),
                        TenKH = c.String(),
                        Phone = c.Int(nullable: false),
                        Email = c.String(),
                        LoaiKH = c.String(),
                        LoaiGoi = c.String(),
                        NgayDangKy = c.DateTime(nullable: false),
                        NgayHetHan = c.DateTime(nullable: false),
                        TenCH = c.String(),
                        DiaChi = c.String(),
                        HoTro = c.String(),
                        Archive = c.Boolean(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.MaKH);
            
            CreateTable(
                "dbo.LichSuGDs",
                c => new
                    {
                        MaGD = c.Int(nullable: false, identity: true),
                        MaKH = c.Int(nullable: false),
                        NgayGD = c.DateTime(nullable: false),
                        NgayHetHan = c.DateTime(nullable: false),
                        SoTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NguoiThu = c.String(),
                    })
                .PrimaryKey(t => t.MaGD)
                .ForeignKey("dbo.KhachHangs", t => t.MaKH, cascadeDelete: true)
                .Index(t => t.MaKH);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LichSuGDs", "MaKH", "dbo.KhachHangs");
            DropIndex("dbo.LichSuGDs", new[] { "MaKH" });
            DropTable("dbo.LichSuGDs");
            DropTable("dbo.KhachHangs");
        }
    }
}
