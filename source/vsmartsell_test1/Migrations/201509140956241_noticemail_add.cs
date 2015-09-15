namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noticemail_add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NoticeMails",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MailType = c.String(nullable: false),
                        Name = c.String(nullable: false),
                        Email = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NoticeMails");
        }
    }
}
