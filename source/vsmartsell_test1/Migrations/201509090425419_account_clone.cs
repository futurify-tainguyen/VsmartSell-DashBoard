namespace vsmartsell_test1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class account_clone : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NguoiDungs",
                c => new
                    {
                        userid = c.String(nullable: false, maxLength: 128),
                        username = c.String(),
                        firstname = c.String(),
                        lastname = c.String(),
                        email = c.String(),
                    })
                .PrimaryKey(t => t.userid);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.NguoiDungs");
        }
    }
}
