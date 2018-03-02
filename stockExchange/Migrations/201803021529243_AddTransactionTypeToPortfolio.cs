namespace stockExchange.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTransactionTypeToPortfolio : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Portfolios", "TransactionType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Portfolios", "TransactionType");
        }
    }
}
