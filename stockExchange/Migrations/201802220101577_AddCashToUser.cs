namespace stockExchange.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCashToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Cash", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Cash");
        }
    }
}
