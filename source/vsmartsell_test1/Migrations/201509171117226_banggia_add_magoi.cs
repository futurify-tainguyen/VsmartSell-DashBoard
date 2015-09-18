namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class banggia_add_magoi : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.KhachHangs", "LoaiGoi", "dbo.BangGias");
            DropIndex("dbo.KhachHangs", new[] { "LoaiGoi" });
            RenameColumn(table: "dbo.KhachHangs", name: "LoaiGoi", newName: "MaGoi");
            DropPrimaryKey("dbo.BangGias");
            AddColumn("dbo.BangGias", "MaGoi", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.BangGias", "LoaiGoi", c => c.String(nullable: false));
            AlterColumn("dbo.KhachHangs", "MaGoi", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.BangGias", "MaGoi");
            CreateIndex("dbo.KhachHangs", "MaGoi");
            AddForeignKey("dbo.KhachHangs", "MaGoi", "dbo.BangGias", "MaGoi", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.KhachHangs", "MaGoi", "dbo.BangGias");
            DropIndex("dbo.KhachHangs", new[] { "MaGoi" });
            DropPrimaryKey("dbo.BangGias");
            AlterColumn("dbo.KhachHangs", "MaGoi", c => c.String(maxLength: 128));
            AlterColumn("dbo.BangGias", "LoaiGoi", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.BangGias", "MaGoi");
            AddPrimaryKey("dbo.BangGias", "LoaiGoi");
            RenameColumn(table: "dbo.KhachHangs", name: "MaGoi", newName: "LoaiGoi");
            CreateIndex("dbo.KhachHangs", "LoaiGoi");
            AddForeignKey("dbo.KhachHangs", "LoaiGoi", "dbo.BangGias", "LoaiGoi");
        }
    }
}
