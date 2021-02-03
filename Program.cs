using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleApp_MnTDefferDyNr
{
    class Program
    {
        public static List<DayTemp> monthTemps = new List<DayTemp>();
        public static String MonthYear;
        public class DayTemp
        {
            public int Dy { get; set; }
            public int MxT { get; set; }
            public int MnT { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("******Program Start******\n");
            LoadData();
            FindMinDifferDy();
        }

        private static void FindMinDifferDy()
        {
            //find the min value
            var minDiffer = monthTemps.Min(DayTemp => DayTemp.MxT-DayTemp.MnT);
            //selected the value/values equals min value
            var minDifferDys = monthTemps.Where(x => x.MxT - x.MnT == minDiffer).Select(d => d.Dy);
            //construct result as a string
            string allMinDifferDys = string.Join(" ", minDifferDys);           
            Console.WriteLine("Day number with the minimum difference in temperature: " + allMinDifferDys);
        }

        private static void LoadData()
        {
            try
            {
                string[] lineReader = File.ReadAllLines("weather.txt");
                int counter = lineReader.Count();
                //Console.WriteLine("TEST: Total data Lines Nr: " + counter);
                Boolean dataBlock = false;
                int DyValue;
                
                foreach (string line in lineReader)
                {
                    DayTemp dayTemp = new DayTemp();
                    // Use a tab to indent each line of the file.
                    //Console.WriteLine("TEST: show what a line read \n" + line);
                    // Split a string line delimited by ' ' and '*', return all non-empty elements.
                    if (line != "")
                    {
                        string[] dataFields = line.Split(new[] { ' ', '*' }, StringSplitOptions.RemoveEmptyEntries);
                        // process each line's data
                        switch (dataFields[0])
                        {
                            case "<pre>":
                                dataBlock = true;
                                Console.WriteLine("Data loading...");
                                break;
                            case "MMU":
                                MonthYear = dataFields[1] + " " + dataFields[2];
                                break;
                            case "</pre>":
                                dataBlock = false;
                                Console.WriteLine("Loading finished.");
                                break;
                        }
                        //check if it's in a valid data line
                        if (int.TryParse(dataFields[0], out DyValue) && dataBlock == true)
                        {
                            dayTemp.Dy = int.Parse(dataFields[0]);
                            dayTemp.MxT = int.Parse(dataFields[1]);
                            dayTemp.MnT = int.Parse(dataFields[2]);
                            monthTemps.Add(dayTemp);
                        }
                    }                 
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something wrong in file loading...");
            }
            finally
            {
                if (monthTemps.Count() != 0)
                {
                    Console.WriteLine("Month & Year: " + MonthYear);
                }
                else
                {
                    Console.WriteLine("No valid data loaded.");
                }
            }
        }
    }
}
