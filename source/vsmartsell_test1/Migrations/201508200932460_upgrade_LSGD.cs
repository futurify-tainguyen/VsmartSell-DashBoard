namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class upgrade_LSGD : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LichSuGDs", "ThoiHan", c => c.Int(nullable: false));
            AddColumn("dbo.LichSuGDs", "CuocPhi", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LichSuGDs", "TienGiam", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.LichSuGDs", "Note", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.LichSuGDs", "Note");
            DropColumn("dbo.LichSuGDs", "TienGiam");
            DropColumn("dbo.LichSuGDs", "CuocPhi");
            DropColumn("dbo.LichSuGDs", "ThoiHan");
        }
    }
}
