using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.IO;
using System.Runtime.InteropServices;
using HtmlAgilityPack;
using Path = System.IO.Path;

namespace DownloadBrev
{
    class Program
    {
        static void Main(string[] args)
        {
            //Download();
            Initialize();
            //var days = _db.QueryAllVesper();
            ParseLaudesAndVesper();
            ParseSext();
            ParseKomplet();
        }

        private static void ParseKomplet()
        {
            var hymnuses = _db.QueryAllHymnus().ToList();
            var days = _db.QueryAllKomplet(); // und vesper
            foreach (var d in days)
            {
                var h = new Hour
                {
                    Date = d.Date,
                    BookPart = d.BookPart,
                    PeriodName = d.PeriodName,
                    Type = d.Type
                };

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(d.Text);

                //var orDefault = doc.DocumentNode.SelectNodes("//*[@class=\"red\"]");
                var f = doc.DocumentNode.ChildNodes.FindFirst("h3");
                var ht = f.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
                h.Hymnus = P(ht.InnerHtml);

                // Psalm 1
                var a = string.Empty;
                var n = string.Empty;
                var c = string.Empty;
                var t = string.Empty;

                var pst1 = GetPsalmSext(ht.NextSibling.NextSibling.NextSibling, out a, out n, out c, out t);
                h.Antiphon1 = P(a);
                h.PsalmName1 = P(n);
                h.PsalmComment1 = P(c);
                h.PsalmText1 = P(t);

                var pst2 = pst1;
                // Psalm 2
                if (!pst1.NextSibling.NextSibling.NextSibling.InnerText.Contains("KURZLESUNG"))
                {
                        // var at2 = ;
                    pst2 = GetPsalmSext(pst1.NextSibling.NextSibling.NextSibling.NextSibling, out a, out n, out c, out t);

                    h.Antiphon2 = P(a);
                    h.PsalmName2 = P(n);
                    h.PsalmComment2 = P(c);
                    h.PsalmText2 = P(t);
                }
                
                /* h.Antiphon2 = at2.InnerText;

                var psn2 = at2.NextSibling;
                h.PsalmName2 = psn2.InnerText;

                var psc2 = psn2.NextSibling;
                h.PsalmComment2 = psc2.InnerText;

                var pst2 = psc2.NextSibling;
                if (pst2.Name == "p")
                {
                    h.PsalmComment2 = $"{h.PsalmComment2} | {pst2.InnerText}";
                    pst2 = pst2.NextSibling;
                }
                h.PsalmText2 = ParsePsalm(pst2);*/

                // Psalm 3
//                var pst3 = GetPsalm(pst2.NextSibling.NextSibling.NextSibling.NextSibling, out a, out n, out c, out t);
//
//                h.Antiphon3 = P(a);
//                h.PsalmName3 = P(n);
//                h.PsalmComment3 = P(c);
//                h.PsalmText3 = P(t);
                /* var at3 = pst1.NextSibling.NextSibling.NextSibling.NextSibling;
                h.Antiphon3 = at3.InnerText;

                var psn3 = at3.NextSibling;
                h.PsalmName3 = psn3.InnerText;

                var psc3 = psn3.NextSibling;
                h.PsalmComment3 = psc3.InnerText;

                var pst3 = psc3.NextSibling;
                if (pst3.Name == "p")
                {
                    h.PsalmComment3 = $"{h.PsalmComment3} | {pst3.InnerText}";
                    pst3 = pst3.NextSibling;
                }
                h.PsalmText3 = ParsePsalm(pst3);*/

                var rn = pst2.NextSibling.NextSibling.NextSibling;
                h.ReadingName = P(rn.InnerText);
                var rt = rn.NextSibling;
                h.ReadingText = P(ParsePsalm(rt));
                var resp = rt.NextSibling.NextSibling;
                h.Responsorium = P(resp.InnerHtml);

                var cant = GetPsalm(resp.NextSibling.NextSibling, out a, out n, out c, out t);
                h.CanticumAntiphon = P(a);
                //h.PsalmName3 = n;
                //h.PsalmComment3 = c;
                h.CanticumText = P(t);



                var or = cant.NextSibling.NextSibling.NextSibling.NextSibling;
                h.Oration = P(or.InnerHtml);
                _db.Add(h);
                if (!hymnuses.Select(ho => ho.Text).Contains(h.Hymnus))
                {
                    var nhymnus = new Hymnus()
                    {
                        Type = h.Type,
                        Text = h.Hymnus
                    };
                    hymnuses.Add(nhymnus);
                    _db.Add(nhymnus);
                }
                Console.WriteLine("{0} - {1}", d.Type, d.Date);
            }
        }
        private static void ParseSext()
        {
            var hymnuses = _db.QueryAllHymnus().ToList();
            var days = _db.QueryAllSext(); // und vesper
            foreach (var d in days)
            {
                var h = new Hour
                {
                    Date = d.Date,
                    BookPart = d.BookPart,
                    PeriodName = d.PeriodName,
                    Type = d.Type
                };

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(d.Text);

                //var orDefault = doc.DocumentNode.SelectNodes("//*[@class=\"red\"]");
                var f = doc.DocumentNode.ChildNodes.FindFirst("h3");
                var ht = f.NextSibling;
                h.Hymnus = P(ht.InnerHtml);

                // Psalm 1
                var a = string.Empty;
                var n = string.Empty;
                var c = string.Empty;
                var t = string.Empty;

                var pst1 = GetPsalmSext(ht.NextSibling.NextSibling.NextSibling, out a, out n, out c, out t);
                h.Antiphon1 = P(a);
                h.PsalmName1 = P(n);
                h.PsalmComment1 = P(c);
                h.PsalmText1 = P(t);

                // Psalm 2

                // var at2 = ;
                var pst2 = GetPsalmSext(pst1.NextSibling.NextSibling.NextSibling.NextSibling, out a, out n, out c, out t);

                h.Antiphon2 = P(a);
                h.PsalmName2 = P(n);
                h.PsalmComment2 = P(c);
                h.PsalmText2 = P(t);
                /* h.Antiphon2 = at2.InnerText;

                var psn2 = at2.NextSibling;
                h.PsalmName2 = psn2.InnerText;

                var psc2 = psn2.NextSibling;
                h.PsalmComment2 = psc2.InnerText;

                var pst2 = psc2.NextSibling;
                if (pst2.Name == "p")
                {
                    h.PsalmComment2 = $"{h.PsalmComment2} | {pst2.InnerText}";
                    pst2 = pst2.NextSibling;
                }
                h.PsalmText2 = ParsePsalm(pst2);*/

                // Psalm 3
                var pst3 = GetPsalmSext(pst2.NextSibling.NextSibling.NextSibling.NextSibling, out a, out n, out c, out t);

                h.Antiphon3 = P(a);
                h.PsalmName3 = P(n);
                h.PsalmComment3 = P(c);
                h.PsalmText3 = P(t);
                /* var at3 = pst1.NextSibling.NextSibling.NextSibling.NextSibling;
                h.Antiphon3 = at3.InnerText;

                var psn3 = at3.NextSibling;
                h.PsalmName3 = psn3.InnerText;

                var psc3 = psn3.NextSibling;
                h.PsalmComment3 = psc3.InnerText;

                var pst3 = psc3.NextSibling;
                if (pst3.Name == "p")
                {
                    h.PsalmComment3 = $"{h.PsalmComment3} | {pst3.InnerText}";
                    pst3 = pst3.NextSibling;
                }
                h.PsalmText3 = ParsePsalm(pst3);*/

                var rn = pst3.NextSibling.NextSibling.NextSibling;
                h.ReadingName = P(rn.InnerText);
                var rt = rn.NextSibling;
                h.ReadingText = P(ParsePsalm(rt));
                var resp = rt.NextSibling.NextSibling;
                h.Responsorium = P(resp.InnerHtml);

                var or = resp.NextSibling.NextSibling;
                h.Oration = P(or.InnerHtml);
                _db.Add(h);
                if (!hymnuses.Select(ho => ho.Text).Contains(h.Hymnus))
                {
                    var nhymnus = new Hymnus()
                    {
                        Type = h.Type,
                        Text = h.Hymnus
                    };
                    hymnuses.Add(nhymnus);
                    _db.Add(nhymnus);
                }
                Console.WriteLine("{0} - {1}", d.Type, d.Date);
            }
        }
        private static void ParseLaudesAndVesper()
        {
            var hymnuses = _db.QueryAllHymnus().ToList();
            var days = _db.QueryAllLaudesUndVesper(); // und vesper
            foreach (var d in days)
            {
                var h = new Hour
                {
                    Date = d.Date,
                    BookPart = d.BookPart,
                    PeriodName = d.PeriodName,
                    Type = d.Type
                };

                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(d.Text);

                //var orDefault = doc.DocumentNode.SelectNodes("//*[@class=\"red\"]");
                var f = doc.DocumentNode.ChildNodes.FindFirst("h3");
                var ht = f.NextSibling;
                h.Hymnus = P(ht.InnerHtml);

                // Psalm 1
                var a = string.Empty;
                var n = string.Empty;
                var c = string.Empty;
                var t = string.Empty;
                HtmlNode rn = null;
                if (ht.NextSibling.InnerText == "PSALMODIE")
                {
                         var pst1 = GetPsalm(ht.NextSibling.NextSibling.NextSibling, out a, out n, out c, out t);
                    h.Antiphon1 = P(a);
                    h.PsalmName1 = P(n);
                    h.PsalmComment1 = P(c);
                    h.PsalmText1 = P(t);

                    // Psalm 2

                    // var at2 = ;
                    var pst2 = GetPsalm(pst1.NextSibling.NextSibling.NextSibling.NextSibling, out a, out n, out c, out t);

                    h.Antiphon2 = P(a);
                    h.PsalmName2 = P(n);
                    h.PsalmComment2 = P(c);
                    h.PsalmText2 = P(t);
                    /* h.Antiphon2 = at2.InnerText;

                    var psn2 = at2.NextSibling;
                    h.PsalmName2 = psn2.InnerText;

                    var psc2 = psn2.NextSibling;
                    h.PsalmComment2 = psc2.InnerText;

                    var pst2 = psc2.NextSibling;
                    if (pst2.Name == "p")
                    {
                        h.PsalmComment2 = $"{h.PsalmComment2} | {pst2.InnerText}";
                        pst2 = pst2.NextSibling;
                    }
                    h.PsalmText2 = ParsePsalm(pst2);*/

                    // Psalm 3
                    var pst3 = GetPsalm(pst2.NextSibling.NextSibling.NextSibling.NextSibling, out a, out n, out c, out t);

                    h.Antiphon3 = P(a);
                    h.PsalmName3 = P(n);
                    h.PsalmComment3 = P(c);
                    h.PsalmText3 = P(t);
                    /* var at3 = pst1.NextSibling.NextSibling.NextSibling.NextSibling;
                    h.Antiphon3 = at3.InnerText;

                    var psn3 = at3.NextSibling;
                    h.PsalmName3 = psn3.InnerText;

                    var psc3 = psn3.NextSibling;
                    h.PsalmComment3 = psc3.InnerText;

                    var pst3 = psc3.NextSibling;
                    if (pst3.Name == "p")
                    {
                        h.PsalmComment3 = $"{h.PsalmComment3} | {pst3.InnerText}";
                        pst3 = pst3.NextSibling;
                    }
                    h.PsalmText3 = ParsePsalm(pst3);*/

                    rn = pst3.NextSibling.NextSibling.NextSibling;
                }
                else
                {
                    rn = ht.NextSibling.NextSibling.NextSibling;
                }
                h.ReadingName = P(rn.InnerText);
                var rt = rn.NextSibling;
                h.ReadingText = P(ParsePsalm(rt));
                var resp = rt.NextSibling.NextSibling;
                h.Responsorium = P(resp.InnerHtml);

                var cant = GetPsalm(resp.NextSibling.NextSibling, out a, out n, out c, out t);
                h.CanticumAntiphon = P(a);
                //h.PsalmName3 = n;
                //h.PsalmComment3 = c;
                h.CanticumText = P(t);

                var intens = cant.NextSibling.NextSibling.NextSibling.NextSibling;
                h.Intercessions = P(intens.InnerHtml);

                var or = intens.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling.NextSibling;
                h.Oration = P(or.InnerHtml);
                _db.Add(h);
                if (!hymnuses.Select(ho => ho.Text).Contains(h.Hymnus))
                {
                    var nhymnus = new Hymnus()
                    {
                        Type = h.Type,
                        Text = h.Hymnus
                    };
                    hymnuses.Add(nhymnus);
                    _db.Add(nhymnus);
                }
                Console.WriteLine("{0} - {1}", d.Type, d.Date);
            }
        }

        private static HtmlAgilityPack.HtmlNode GetPsalm(HtmlAgilityPack.HtmlNode at, out string a, out string n, out string c, out string t)
        {
            var at1 = at;
            a = at1.InnerText;

            var psn1 = at1.NextSibling;
            n = psn1.InnerText;

            var psc1 = psn1.NextSibling;
            var pst1 = psc1;
            c = string.Empty;
            if (psc1.Name == "p")
            {
                c = psc1.InnerText;
                pst1 = psc1.NextSibling; //pst1
                if (pst1.Name == "p")
                {
                    c = $"{c} | {pst1.InnerText}";
                    pst1 = pst1.NextSibling; //pst1
                }

            }
            t = ParsePsalm(pst1);
           // t = pst1.InnerText;
           

            return pst1;
        }

        private static HtmlAgilityPack.HtmlNode GetPsalmSext(HtmlAgilityPack.HtmlNode at, out string a, out string n, out string c, out string t)
        {
            var at1 = at;
            a = at1.InnerText;

            var psn1 = at1.NextSibling;
            n = psn1.InnerText;

            var psc1 = psn1.NextSibling;
            var pst1 = psc1;
            c = string.Empty;
            if (psc1.Name != "div")
            {
                c = psc1.InnerText;
                pst1 = FindNode(psc1, "div");
            }
            // t = ParsePsalm(pst1);
            if (pst1 == null)
                Console.ReadKey();
            t = ParsePsalm(pst1);
            return pst1;
        }

        private static HtmlAgilityPack.HtmlNode FindNode(HtmlAgilityPack.HtmlNode n, string name)
        {
            if (n != null && n.Name != name)
            {
                return FindNode(n.NextSibling, name);
            }
            return n;
        }
        private static string ParsePsalm(HtmlAgilityPack.HtmlNode psalmText)
        {
            var resT = string.Empty;
            if (psalmText != null)
            {
                resT = psalmText.InnerText;
                if (!psalmText.InnerText.Contains("span")) return resT;
                foreach (HtmlNode span in psalmText.SelectNodes(".//span"))
                {
                    string attributeValue = span.GetAttributeValue("style", "");
                    if (string.IsNullOrEmpty(attributeValue))
                        resT += $"{span.InnerHtml}";
                }
            }
            
            return resT;
        }

        private static string P(string s) //"ÄÜÖßäüöß"
        {
            s = s.Replace("<br>", "\n")
                .Replace("<span class='red'>", "||")
                .Replace("</span>", "||")
                .Replace("&uuml;", "ü").Replace("&Uuml;", "Ü")
                .Replace("&laquo;", "\"").Replace("&raquo;", "\"")
                .Replace("&ouml;", "ö").Replace("&Ouml;", "Ö")
                .Replace("&auml;", "ä").Replace("&Auml;", "Ä")
                .Replace("&szlig;", "ß").Replace("&Szlig;", "ß").Replace("&rsquo;", "'").Trim();
            return s;
        }

        private static void Download()
        {
            var currentDate = new DateTime(2017, 1, 1);
            var lastDate = new DateTime(2017, 12, 31);
            var types = new List<string>() { "laudes" };//, "vesper", "terz", "non", "komplet" };
            Initialize();

            using (var file =
            new StreamWriter("WriteLines4.txt"))
            {


                using (var wc = new WebClient()) // "using" keyword automatically closes WebClient stream on download completed
                {
                    while (currentDate <= lastDate)
                    {
                        file.WriteLine(currentDate);
                        foreach (var type in types)
                        {
                            var url =
                                $"http://stundenbuch.katholisch.de/stundenbuch.php?type={type}&date={currentDate.ToString("d.M.yyyy")}";
                            var html = wc.DownloadString(url);
                            var doc = new HtmlAgilityPack.HtmlDocument();
                            doc.LoadHtml(html);
                            var day = new Day
                            {
                                Date = currentDate.ToShortDateString(),
                                Type = type
                            };

                            var orDefault = doc.DocumentNode.SelectNodes("//*[@class=\"stdhead w1030\"]/h3[1]")
                                ?.FirstOrDefault();
                            if (orDefault !=
                                null)
                                day.PeriodName = orDefault
                                    ?.InnerText;

                            var firstOrDefault = doc.DocumentNode.SelectNodes("//*[@class=\"stdhead w1030\"]/h3[2]")
                                ?.FirstOrDefault();
                            if (firstOrDefault !=
                                null)
                                day.BookPart = firstOrDefault
                                    ?.InnerText;

                            var content = doc.DocumentNode.SelectNodes("//*[@class=\"stundenbuch-output\"]");
                            day.Text = content.FirstOrDefault()?.InnerHtml;
                            _db.Add(day);

                        }
                        currentDate = currentDate.AddDays(1);

                    };

                }
            }
        }

        public static Database _db;

        public static void Initialize()
        {
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Days444.db");
            _db = new Database(dbPath);
        }
    }
}
