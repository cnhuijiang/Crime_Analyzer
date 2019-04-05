using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Crime_Analyzer
{
    public class data
    {
        public int crimeyear;
        public int crimepop;
        public int crimevio;
        public int crimemur;
        public int crimerape;
        public int crimerob;
        public int crimeassault;
        public int crimeproperty;
        public int crimeburglary;
        public int crimetheft;
        public int crimevehicle_theft;
    }


    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please enter dotnet CrimeAnalyzer.dll <csv_file_path> <report_file_path> ");
                return;
            }
            
            string filename = args[0];
            string output = args[1];
            

            List<data> crimeData = new List<data>();
            StreamReader file = new StreamReader(filename);
            var line = file.ReadLine();
            while (!file.EndOfStream)
            {
                line = file.ReadLine();
                var values = line.Split(',');
                int year = int.Parse(values[0]);
                int pop = int.Parse(values[1]);
                int vio = int.Parse(values[2]);
                int mur = int.Parse(values[3]);
                int rape = int.Parse(values[4]);
                int rob = int.Parse(values[5]);
                int assault = int.Parse(values[6]);
                int property = int.Parse(values[7]);
                int burglary = int.Parse(values[8]);
                int theft = int.Parse(values[9]);
                int vehicle_theft = int.Parse(values[10]);
                crimeData.Add(new data() { crimeyear = year, crimepop = pop, crimevio = vio, crimemur = mur, crimerape = rape, crimerob = rob, crimeassault = assault, crimeproperty = property, crimeburglary = burglary, crimetheft = theft, crimevehicle_theft = vehicle_theft });
            }


            //Write report
            var report = new StreamWriter(output);
            var year_data = from x in crimeData select x.crimeyear;
            int numYear = year_data.Count();
            int begYear = year_data.Min();
            int endYear = year_data.Max();
            report.WriteLine("Period: {0}-{1} ({2} years)", begYear, endYear, numYear);

            var murder = from x in crimeData where x.crimemur < 15000 select x.crimeyear;
            string murder_data=null;
            foreach(var data in murder)
            {
                murder_data += string.Format("{0},",data);
            }
            report.WriteLine("Years murders per year < 15000:" + murder_data);

            var robbery_data = from x in crimeData where x.crimerob > 500000 select new { x.crimeyear, x.crimerob };
            string robbery_every_data = null;
            foreach(var x in robbery_data)
            {
                robbery_every_data += string.Format("{0}={1},", x.crimeyear,x.crimerob);
            }
            report.WriteLine("Robberies per year > 500000:{0}",robbery_every_data);

            var violent = from x in crimeData where x.crimeyear == 2010 select new { x.crimevio, x.crimepop };
            double violent_vio=0,violent_pop = 0;
            foreach(var x in violent)
            {
                violent_vio = x.crimevio;
                violent_pop = x.crimepop;
            }
            double pco = violent_vio / violent_pop;
            report.WriteLine("Violent crime per capita rate (2010):" + pco);

            var murderdata = from x in crimeData select x.crimemur;
            report.WriteLine("Average murder per year (all years): {0}", murderdata.Average());

            murderdata = from x in crimeData where x.crimeyear >= 1994 && x.crimeyear <= 1997 select x.crimemur;
            report.WriteLine("Average murder per year (1994-1997): {0}", murderdata.Average());

            murderdata = from x in crimeData where x.crimeyear >= 2010 && x.crimeyear <= 2013 select x.crimemur;
            report.WriteLine("Average murder per year (2010-2013): {0}", murderdata.Average());

            var theftData = from x in crimeData where x.crimeyear >= 1999 && x.crimeyear <= 2004 select x.crimemur;
            report.WriteLine("Minimum thefts per year (1999–2004): {0}", theftData.Min());
            report.WriteLine("Maximum thefts per year (1999-2004): {0}", theftData.Max());

            var vehicledata=from x in crimeData orderby x.crimevehicle_theft descending  select new { x.crimeyear, x.crimevehicle_theft };
            string a="";
            foreach(var x in vehicledata)
            {
                a+= x.crimeyear;
            }
            report.WriteLine("Year of highest number of motor vehicle thefts:" + a.Substring(0,4));
            Console.WriteLine(output + " is created");
            report.Close();
        }
    }
}
