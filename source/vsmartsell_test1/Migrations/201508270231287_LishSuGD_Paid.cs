namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LishSuGD_Paid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LichSuGDs", "Paid", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LichSuGDs", "Paid");
        }
    }
}
