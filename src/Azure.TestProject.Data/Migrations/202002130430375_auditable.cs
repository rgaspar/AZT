namespace Azure.TestProject.Data
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class auditable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EmailAttributes", "CreatedAtUtc", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EmailAttributes", "CreatedAtUtc");
        }
    }
}
