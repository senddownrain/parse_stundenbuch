using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DownloadBrev
{
    public class Hour
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        //public string Text { get; set; }
        public string PeriodName { get; set; }
        public string BookPart { get; set; }
        public string Hymnus { get; set; }
        // Psalmodie
        // Psalm 1
        public string Antiphon1 { get; set; }
        public string PsalmName1 { get; set; }
        public string PsalmComment1 { get; set; }
        public string PsalmText1 { get; set; }
        // Psalm 2
        public string Antiphon2 { get; set; }
        public string PsalmName2 { get; set; }
        public string PsalmComment2 { get; set; }
        public string PsalmText2 { get; set; }
        //Psalm 3
        public string Antiphon3 { get; set; }
        public string PsalmName3 { get; set; }
        public string PsalmComment3 { get; set; }
        public string PsalmText3 { get; set; }

        // Reading
        public string ReadingName { get; set; }
        public string ReadingText { get; set; }

        // RESPONSORIUM
        public string Responsorium { get; set; }

        //Canticum
        public string CanticumAntiphon { get; set; }
        public string CanticumText { get; set; }

        // intercessions
        public string Intercessions { get; set; }
        public string Oration { get; set; }
    }
}
