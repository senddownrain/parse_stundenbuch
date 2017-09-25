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
    public class Day
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Text { get; set; }
        public string  PeriodName { get; set; }
        public string BookPart { get; set; }

    }
}
