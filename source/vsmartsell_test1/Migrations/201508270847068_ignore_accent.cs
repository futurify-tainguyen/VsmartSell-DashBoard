namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ignore_accent : DbMigration
    {
        public override void Up()
        {
            Sql("alter table KhachHangs alter column TenKH nvarchar(255) COLLATE SQL_LATIN1_GENERAL_CP1_CI_AI null");
        }
        
        public override void Down()
        {
        }
    }
}
