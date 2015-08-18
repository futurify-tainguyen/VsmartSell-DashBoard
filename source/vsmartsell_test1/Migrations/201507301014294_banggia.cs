namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class banggia : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BangGias",
                c => new
                    {
                        LoaiGoi = c.String(nullable: false, maxLength: 128),
                        GiaTien = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.LoaiGoi);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BangGias");
        }
    }
}
