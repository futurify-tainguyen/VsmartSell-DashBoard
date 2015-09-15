namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noticemail_remove_require_name : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.NoticeMails", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.NoticeMails", "Name", c => c.String(nullable: false));
        }
    }
}
