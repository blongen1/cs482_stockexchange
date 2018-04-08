namespace stockExchange.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPriceHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PriceHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Symbol = c.String(),
                        Price = c.Double(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PriceHistories");
        }
    }
}
