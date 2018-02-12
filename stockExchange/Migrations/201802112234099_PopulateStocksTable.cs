namespace stockExchange.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateStocksTable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('AAPL', 'Apple Inc.', " +
                "154.52, 156.41, 70672608, 150.24, 157.89, 132.05, 180.1)");
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('GOOG', 'Alphabet Inc', " +
                "1001.52, 1037.78, 3505862, 992.56, 1043.97, 803.37, 1186.89)");
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('MSFT', 'Microsoft Corporation', " +
                "85.01, 88.18, 63499065, 83.83, 88.93, 63.62, 96.07)");
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('AMZN', 'Amazon.com Inc.', " +
                "1350.50, 1339.60, 14141524, 1265.93, 1383.50, 822.85, 1498.00)");
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('FB', 'Facebook Inc.', " +
                "171.58, 176.11, 39887626, 167.18, 176.90, 132.55, 195.32)");
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('JNJ', 'Johnson & Johnson', " +
                "126.36, 129.53, 15030881, 125.44, 130.92, 114.23, 148.32)");
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('XOM', 'Exxon Mobil Corporation', " +
                "75.30, 75.78, 29491592, 73.90, 76.48, 73.90, 89.30)");
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('WMT', 'Walmart Inc', " +
                "100.11, 99.37, 14184609, 96.43, 101.10, 67.56, 109.98)");
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('NVDA', 'NVIDIA Corporation', " +
                "217.52, 232.08, 41865077, 217.52, 238.89, 95.17, 249.27)");
            Sql("INSERT INTO Stocks (Symbol, CompanyName, PreviousClose, CurrentPrice, " +
                "Volume, DayLow, DayHigh, YearLow, YearHigh) VALUES ('TSLA', 'Tesla Inc', " +
                "315.23, 310.42, 12933721, 294.76, 320.98, 242.01, 389.61)");
        }
        
        public override void Down()
        {
        }
    }
}
