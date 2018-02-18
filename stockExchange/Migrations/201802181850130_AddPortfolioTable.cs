namespace stockExchange.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPortfolioTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Portfolios",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(),
                        Price = c.Double(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Portfolios");
        }
    }
}
