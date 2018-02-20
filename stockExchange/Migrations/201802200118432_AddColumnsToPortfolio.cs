namespace stockExchange.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsToPortfolio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Portfolios", "UserId", c => c.String());
            AddColumn("dbo.Portfolios", "Time", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Portfolios", "Time");
            DropColumn("dbo.Portfolios", "UserId");
        }
    }
}
