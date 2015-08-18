namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class banggia1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.KhachHangs", "LoaiGoi");
        }
        
        public override void Down()
        {
            AddColumn("dbo.KhachHangs", "LoaiGoi", c => c.String());
        }
    }
}
