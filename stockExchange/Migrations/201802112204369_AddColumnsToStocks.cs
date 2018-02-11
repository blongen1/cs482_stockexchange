namespace stockExchange.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColumnsToStocks : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stocks", "CompanyName", c => c.String());
            AddColumn("dbo.Stocks", "PreviousClose", c => c.Double(nullable: false));
            AddColumn("dbo.Stocks", "CurrentPrice", c => c.Double(nullable: false));
            AddColumn("dbo.Stocks", "Volume", c => c.Int(nullable: false));
            AddColumn("dbo.Stocks", "DayLow", c => c.Double(nullable: false));
            AddColumn("dbo.Stocks", "DayHigh", c => c.Double(nullable: false));
            AddColumn("dbo.Stocks", "YearLow", c => c.Double(nullable: false));
            AddColumn("dbo.Stocks", "YearHigh", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stocks", "YearHigh");
            DropColumn("dbo.Stocks", "YearLow");
            DropColumn("dbo.Stocks", "DayHigh");
            DropColumn("dbo.Stocks", "DayLow");
            DropColumn("dbo.Stocks", "Volume");
            DropColumn("dbo.Stocks", "CurrentPrice");
            DropColumn("dbo.Stocks", "PreviousClose");
            DropColumn("dbo.Stocks", "CompanyName");
        }
    }
}
