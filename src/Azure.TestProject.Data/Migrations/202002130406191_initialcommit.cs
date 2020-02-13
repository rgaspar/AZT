namespace Azure.TestProject.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcommit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailAttributes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 128),
                        Attribute = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailAttributes");
        }
    }
}
