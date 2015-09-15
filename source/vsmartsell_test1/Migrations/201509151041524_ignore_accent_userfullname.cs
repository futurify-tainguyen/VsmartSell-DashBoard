namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ignore_accent_userfullname : DbMigration
    {
        public override void Up()
        {
            Sql("alter table NguoiDungs alter column firstname nvarchar(MAX) COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI null");
            Sql("alter table NguoiDungs alter column lastname nvarchar(MAX) COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI null");
        }
        
        public override void Down()
        {
        }
    }
}
