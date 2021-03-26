using System;
using System.Text;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace MTGdb
{
    static class Program
    {
        private const string URL = "https://api.scryfall.com/cards/collection";
        public static List<Card> allcards = new List<Card>();
        public static List<Card> cardsbycolorandcmc = new List<Card>();
        public static List<CardWithFilter> cardsbytype = new List<CardWithFilter>();
        public static List<CardWithFilter> cardsbysubtype = new List<CardWithFilter>();
        public static List<CardWithFilter> cardsbykeyword = new List<CardWithFilter>();
        public static void Start()
        {
            string filedirc = Directory.GetCurrentDirectory() + "/Files";
            if (!System.IO.Directory.Exists(filedirc))
            {
                Console.WriteLine("Mising 'Files' Directory");
                Console.ReadLine();
                Environment.Exit(0);
            }

            string dbpath = filedirc + "/Carddb.csv";
            string tcgpath = filedirc + "/TCGplayer.csv";

            if (!File.Exists(dbpath) || !File.Exists(tcgpath))
            {
                Console.WriteLine("Mising Carddb.csv and/or TCGplayer.csv In 'Files' Directory");
                Console.ReadLine();
                Environment.Exit(0);
            }

            string[] args = new string[] { dbpath, tcgpath };

            List<ExcelCard> carddb = new List<ExcelCard>(); // REF TO DB
            List<ExcelCard> tcgplayercards = new List<ExcelCard>();
            var csvTable = new DataTable();
            using (var csvReader = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(System.IO.File.OpenRead(@args[0])), true))
            {
                csvTable.Load(csvReader);
            }

            foreach (DataRow row in csvTable.Rows)
            {

                carddb.Add(new ExcelCard { Name = row.Field<string>(0), Special_name = row.Field<string>(1), Set = row.Field<string>(2), Amount = Convert.ToInt32(row.Field<string>(3)), Printing = row.Field<string>(4), Collector_number = row.Field<string>(5) });
            }


            csvTable = new DataTable();
            using (var csvReader = new LumenWorks.Framework.IO.Csv.CsvReader(new StreamReader(System.IO.File.OpenRead(@args[1])), true))
            {
                csvTable.Load(csvReader);
            }

            foreach (DataRow row in csvTable.Rows)
            {

                tcgplayercards.Add(new ExcelCard { Name = row.Field<string>(2), Special_name = row.Field<string>(1), Set = row.Field<string>(5), Amount = Convert.ToInt32(row.Field<string>(0)), Printing = row.Field<string>(6), Collector_number = row.Field<string>(4) });
                if (tcgplayercards[tcgplayercards.Count - 1].Set.ToLower().Equals("10e") && tcgplayercards[tcgplayercards.Count - 1].Printing.Equals("Foil"))
                {
                    tcgplayercards[tcgplayercards.Count - 1].Collector_number = tcgplayercards[tcgplayercards.Count - 1].Collector_number + "★";
                }
                else if (tcgplayercards[tcgplayercards.Count - 1].Set.ToLower().Equals("tlp"))
                {
                    tcgplayercards[tcgplayercards.Count - 1].Set = "plist";
                }
            }

            if (tcgplayercards.Count != 0)
            {

                foreach (ExcelCard excard in tcgplayercards)
                {
                    ExcelCard match = carddb.Find(c => c.Set.Equals(excard.Set.ToLower()) && c.Collector_number.Equals(excard.Collector_number) && c.Printing.Equals(excard.Printing));
                    if (match != null)
                    {
                        carddb[carddb.IndexOf(match)].Amount = carddb[carddb.IndexOf(match)].Amount + 1;
                    }
                    else
                    {
                        carddb.Add(excard);
                    }
                }
            }


            ICollection<CardSearch> CardsToSearch = new List<CardSearch>();
            string jsonstring;
            StringContent httpContent;
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            Cards returncards;
            //List<Card> allcards = new List<Card>(); // HAS ALL CARDS AND INFROMATION
            for (int i = 0; i < carddb.Count; i++)
            {
                CardsToSearch.Add(new CardSearch { Set = carddb[i].Set, Collector_number = carddb[i].Collector_number });
                if (CardsToSearch.Count == 75 || i == carddb.Count - 1)
                {
                    jsonstring = JsonConvert.SerializeObject(new Identifier { IDs = CardsToSearch });
                    httpContent = new StringContent(jsonstring, Encoding.UTF8, "application/json");
                    response = client.PostAsync(URL, httpContent).Result;
                    returncards = response.Content.ReadAsAsync<Cards>().Result;
                    allcards.AddRange(returncards.Data);
                    CardsToSearch.Clear();
                }
            }

            allcards.Sort((x, y) => {
                if (x.Name.CompareTo(y.Name) != 0)
                {
                    return x.Name.CompareTo(y.Name);
                }
                else if (x.Set.CompareTo(y.Set) != 0)
                {
                    return x.Set.CompareTo(y.Set);
                }
                else
                {
                    return x.Collector_number.CompareTo(y.Collector_number);
                }
            });
            carddb.Sort((x, y) => {
                if (x.Name.CompareTo(y.Name) != 0)
                {
                    return x.Name.CompareTo(y.Name);
                }
                else if (x.Set.CompareTo(y.Set) != 0)
                {
                    return x.Set.CompareTo(y.Set);
                }
                else
                {
                    return x.Collector_number.CompareTo(y.Collector_number);
                }
            });

            for (int i = 0; i < allcards.Count; i++)
            {
                allcards[i].Amount = carddb[i].Amount;
                allcards[i].Special_name = carddb[i].Special_name;
                if ("Foil".Equals(carddb[i].Printing))
                {
                    allcards[i].Printing = "Foil";
                }
                else
                {
                    allcards[i].Printing = "Normal";
                }


                //color and cmc
                allcards[i].Colorstring = new string(allcards[i].Color_identity.ToArray());
                if(allcards[i].Colorstring.Length == 0)
                {
                    allcards[i].Colorstring = "C";
                }
                allcards[i].Coloridcmc = new ColorIdCmc(allcards[i].Cmc, allcards[i].Colorstring);
                cardsbycolorandcmc.Add(allcards[i]);


                //color and types
                if (allcards[i].Card_faces is null)
                {
                    if (allcards[i].Type_line.Contains('—'))
                    {
                        cardsbytype.Add(new CardWithFilter { Thecard = allcards[i], Coloridfilter = new ColorIdFilter(allcards[i].Type_line.Substring(0, allcards[i].Type_line.IndexOf('—') - 1), allcards[i].Colorstring) });
                        //ADD SUBTYPE HERE
                        cardsbysubtype.Add(new CardWithFilter { Thecard = allcards[i], Coloridfilter = new ColorIdFilter(allcards[i].Type_line.Substring(allcards[i].Type_line.IndexOf('—') + 2, allcards[i].Type_line.Length - (allcards[i].Type_line.IndexOf('—') + 2)), allcards[i].Colorstring) });
                    }
                    else
                    {
                        cardsbytype.Add(new CardWithFilter { Thecard = allcards[i], Coloridfilter = new ColorIdFilter(allcards[i].Type_line, allcards[i].Colorstring) });
                    }
                }
                else
                {
                    foreach (Cardface cardface in allcards[i].Card_faces)
                    {
                        if (cardface.Type_line.Contains('—'))
                        {
                            cardsbytype.Add(new CardWithFilter { Thecard = allcards[i], Coloridfilter = new ColorIdFilter(cardface.Type_line.Substring(0, cardface.Type_line.IndexOf('—') - 1), allcards[i].Colorstring) });
                            //ADD SUBTYPE HERE
                            cardsbysubtype.Add(new CardWithFilter { Thecard = allcards[i], Coloridfilter = new ColorIdFilter(cardface.Type_line.Substring(cardface.Type_line.IndexOf('—') + 2, cardface.Type_line.Length - (cardface.Type_line.IndexOf('—') + 2)), allcards[i].Colorstring) });
                        }
                        else
                        {
                            cardsbytype.Add(new CardWithFilter { Thecard = allcards[i], Coloridfilter = new ColorIdFilter(cardface.Type_line, allcards[i].Colorstring) });
                        }
                    }
                }

                //color and keywords
                foreach(string keyword in allcards[i].Keywords)
                {
                    cardsbykeyword.Add(new CardWithFilter { Thecard = allcards[i], Coloridfilter = new ColorIdFilter(keyword, allcards[i].Colorstring) });
                }

                //prices
                if(allcards[i].Printing.Equals("Foil"))
                {
                    allcards[i].Pricetotal = Math.Round(Convert.ToDouble(allcards[i].Prices["usd_foil"]) * allcards[i].Amount, 2);
                }
                else
                {
                    allcards[i].Pricetotal = Math.Round(Convert.ToDouble(allcards[i].Prices["usd"]) * allcards[i].Amount, 2);
                }

            }

            cardsbytype.Sort((x, y) =>
            {
                return x.Coloridfilter.Display.CompareTo(y.Coloridfilter.Display);
            });

            cardsbysubtype.Sort((x, y) =>
            {
                return x.Coloridfilter.Display.CompareTo(y.Coloridfilter.Display);
            });

           cardsbykeyword.Sort((x, y) =>
           {
               return x.Coloridfilter.Display.CompareTo(y.Coloridfilter.Display);
           });

            cardsbycolorandcmc.Sort((x, y) =>
            {
                return x.Coloridcmc.Display.CompareTo(y.Coloridcmc.Display);
            });

            //https://www.wpf-tutorial.com/listview-control/listview-filtering/


            // System.IO.File.WriteAllText(@args[0], string.Empty);
            //System.IO.File.WriteAllText(@args[1], string.Empty);
            //using (var writer = new StreamWriter(args[0]))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //{
            //  csv.WriteRecords(allcards);
            //}
        }
    }
}
