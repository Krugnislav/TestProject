namespace TestProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrateDB2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "Status", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "Status", c => c.String(maxLength: 150));
        }
    }
}
