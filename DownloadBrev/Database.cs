using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Path = System.IO.Path;

using SQLite;
using System.Globalization;

namespace DownloadBrev
{
    public class Database : SQLiteConnection
    {
        public Database(string path) : base(path)
        {
            //CreateTable<Day>();
            CreateTable<Hour>();
            CreateTable<Hymnus>();
        }

        public void Add(Day day)
        {
            this.Insert(day);
        }

        public void Add(Hour hour)
        {
            this.Insert(hour);
        }

        public void Add(Hymnus h)
        {
            this.Insert(h);
        }
        //public IEnumerable<Valuation> QueryValuations(Stock stock)
        //{
        //    return Table<Valuation>().Where(x => x.StockId == stock.Id);
        //}
        //public Valuation QueryLatestValuation(Stock stock)
        //{
        //    return Table<Valuation>().Where(x => x.StockId == stock.Id).OrderByDescending(x => x.Time).Take(1).FirstOrDefault();
        //}
        //public Stock QueryStock(string stockSymbol)
        //{
        //    return (from s in Table<Stock>()
        //            where s.Symbol == stockSymbol
        //            select s).FirstOrDefault();
        //}
        public IEnumerable<Day> QueryAllVesper()
        {
            return from s in Table<Day>()
                   where  s.Type == "vesper"
                   orderby s.Date
                   select s;
        }
        public IEnumerable<Day> QueryAllLaudesUndVesper()
        {
            return from s in Table<Day>()
                where s.Type == "laudes" || s.Type == "vesper"
                orderby s.Date
                select s;
        }

        public IEnumerable<Day> QueryAllSext()
        {
            return from s in Table<Day>()
                where s.Type == "sext" || s.Type == "non" || s.Type == "terz"
                   orderby s.Date
                select s;
        }

        public IEnumerable<Day> QueryAllKomplet()
        {
            return from s in Table<Day>()
                where s.Type == "komplet" 
                orderby s.Date
                select s;
        }

        public IEnumerable<Hymnus> QueryAllHymnus()
        {
           
            return from s in Table<Hymnus>() select  s;
        }

        //public void UpdateStock(string stockSymbol)
        //{
        //    //
        //    // Ensure that there is a valid Stock in the DB
        //    //
        //    var stock = QueryStock(stockSymbol);
        //    if (stock == null)
        //    {
        //        stock = new Stock { Symbol = stockSymbol };
        //        Insert(stock);
        //    }

        //    //
        //    // When was it last valued?
        //    //
        //    var latest = QueryLatestValuation(stock);
        //    var latestDate = latest != null ? latest.Time : new DateTime(1950, 1, 1);

        //    //
        //    // Get the latest valuations
        //    //
        //    try
        //    {
        //        var newVals = new YahooScraper().GetValuations(stock, latestDate + TimeSpan.FromHours(23), DateTime.Now);
        //        InsertAll(newVals);
        //    }
        //    catch (System.Net.WebException ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //}
    }
}
