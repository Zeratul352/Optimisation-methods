using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimisation_base
{
    class ArmihoGoldshteinWolfRules
    {
        public static double delta = 0.01;
        private static Tuple<double, double> Xtemp;
        private static Tuple<double, double> Gradtemp;
        private static double FI = 0;
        private static int A = 1;
        private static int B = 1;
        
        private static double epsila = 0.8;
        private static double gamma = 0.3;
        private static double beta = 0.9;
        private static Random random = new Random();

        public static void Rotation()
        {
            List<ArmihoLine> Output = new List<ArmihoLine>();
            
            for (int i = 0; i <= 20; i++)
            {
                epsila = i * 0.05;
                var line = new ArmihoLine();
                line.Iteration = epsila.ToString();
                var min = ArmihoMovedown(0, 0);
                var c = min.Count();
                line.Lambda = c.ToString();
                Output.Add(line);
            }
            string filename = "Rosenbrok_armiho_iterations";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(Output);
                }
            }
        }
        public static List<Tuple<double, double>> ArmihoMovedown(double x, double y)
        {
            int iteration = 0;
        List<ArmihoLine> Output = new List<ArmihoLine>();
            ArmihoLine header = new ArmihoLine();
            header.Lambda = "Rosenbrok";
            Output.Add(header);
            List<Tuple<double, double>> Allpoints = new List<Tuple<double, double>>();
            Allpoints.Add(new Tuple<double, double>(x, y));
            
            double norm = 1;
            Xtemp = Allpoints[iteration];
            Gradtemp = AntiGradient(Xtemp.Item1, Xtemp.Item2);
            ArmihoLine firstline = new ArmihoLine();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = x.ToString();
            firstline.X2 = y.ToString();
            firstline.F_x = Function(x, y).ToString();
            //firstline.dF_dX1 = Gradtemp.Item1.ToString();
            //firstline.dF_dX2 = Gradtemp.Item2.ToString();
            Output.Add(firstline);
            do
            {
                ArmihoLine line = new ArmihoLine();

                var alfa = CalcAlfaArmiho();
                Allpoints.Add(new Tuple<double, double>(Allpoints[iteration].Item1 + alfa * Gradtemp.Item1, Allpoints[iteration].Item2 + alfa * Gradtemp.Item2));
                iteration++;
                //norm = Function(Allpoints[iteration].Item1, Allpoints[iteration].Item2) - Function(Allpoints[iteration-1].Item1, Allpoints[iteration - 1].Item2);
                norm = Math.Sqrt(Math.Pow(Allpoints[iteration].Item1 - Allpoints[iteration - 1].Item1, 2)
                + Math.Pow(Allpoints[iteration].Item2 - Allpoints[iteration - 1].Item2, 2));
                Xtemp = Allpoints[iteration];
                Gradtemp = AntiGradient(Xtemp.Item1, Xtemp.Item2);
                line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                //line.deltaX1 = (alfa * Gradtemp.Item1).ToString();
                //line.deltaX2 = (alfa * Gradtemp.Item2).ToString();
                line.X1 = Xtemp.Item1.ToString();
                line.X2 = Xtemp.Item2.ToString();
                line.F_x = Function(Xtemp.Item1, Xtemp.Item2).ToString();
                //line.dF_dX1 = Gradtemp.Item1.ToString();
                //line.dF_dX2 = Gradtemp.Item2.ToString();
                //line.norm_X = norm.ToString();
                Output.Add(line);
            } while (norm > delta);
            ArmihoLine downheader = new ArmihoLine();
            downheader.Lambda = "Extremum";
            downheader.X1 = Xtemp.Item1.ToString();
            downheader.X2 = Xtemp.Item2.ToString();
            downheader.F_x = Function(Xtemp.Item1, Xtemp.Item2).ToString();
            Output.Add(downheader);
            string filename = "Prot_ellips11";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(Output);
                }
            }
            return Allpoints;
        }

        public static double CalcAlfaGoldshtein()
        {
            double a_floor = 0;
            double a_roof = 0;
            double alfa = Math.Round(random.NextDouble(), 7) * 10;
            var x = Xtemp;
            
            while (true)
            {
                var x_next = new Tuple<double, double>(x.Item1 + alfa * Gradtemp.Item1, x.Item2 + alfa * Gradtemp.Item2);
                if (GoldshteinRule(x, x_next, alfa))
                {
                    return alfa;
                }
                if(!ArmihoRule(x, x_next, alfa))
                {
                    a_roof = alfa;
                    alfa = (a_roof + a_floor) / 2;
                    continue;
                }
                if(!GoldshteinRule(x, x_next, alfa))
                {
                    a_floor = alfa;
                    if(a_roof == 0)
                    {
                        while(alfa <= a_floor)
                        {
                            alfa = Math.Round(random.NextDouble(), 7) * 10;
                        }
                    }
                    else
                    {
                        alfa = (a_roof + a_floor) / 2;
                    }
                }
                
            }

        }
        public static double CalcAlfaWolf()
        {
            double a_floor = 0;
            double a_roof = 0;
            double alfa = Math.Round(random.NextDouble(), 7) * 10;
            var x = Xtemp;

            while (true)
            {
                var x_next = new Tuple<double, double>(x.Item1 + alfa * Gradtemp.Item1, x.Item2 + alfa * Gradtemp.Item2);
                if (WolfRule(x, x_next, alfa))
                {
                    return alfa;
                }
                if (!ArmihoRule(x, x_next, alfa))
                {
                    a_roof = alfa;
                    alfa = (a_roof + a_floor) / 2;
                    continue;
                }
                if (!WolfRule(x, x_next, alfa))
                {
                    a_floor = alfa;
                    if (a_roof == 0)
                    {
                        while (alfa <= a_floor)
                        {
                            alfa = Math.Round(random.NextDouble(), 7) * 10;
                        }
                    }
                    else
                    {
                        alfa = (a_roof + a_floor) / 2;
                    }
                }

            }

        }
        public static double CalcAlfaArmiho()
        {
            double a_floor = 0;
            double a_roof = 0;
            double alfa = Math.Round(random.NextDouble(), 10) * 10;
            var x = Xtemp;

            while (true)
            {
                var x_next = new Tuple<double, double>(x.Item1 + alfa * Gradtemp.Item1, x.Item2 + alfa * Gradtemp.Item2);
                if (ArmihoRule(x, x_next, alfa))
                {
                    return alfa;
                }
                else
                {
                    a_roof = alfa;
                }
                alfa = (a_roof + a_floor) / 2;
            }

        }



        public static bool ArmihoRule(Tuple<double, double> x, Tuple<double, double> x_next, double alfa)
        {
            var component = epsila * alfa * (Gradtemp.Item1 * Gradtemp.Item1 * -1 + Gradtemp.Item2 * Gradtemp.Item2 * -1);
            var F_x = Function(x.Item1, x.Item2);
            var F_x_next = Function(x_next.Item1, x_next.Item2);
            if(F_x_next <= F_x + component)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool GoldshteinRule(Tuple<double, double> x, Tuple<double, double> x_next, double alfa)
        {
            var component = alfa * (Gradtemp.Item1 * Gradtemp.Item1 * -1 + Gradtemp.Item2 * Gradtemp.Item2 * -1);
            var F_x = Function(x.Item1, x.Item2);
            var F_x_next = Function(x_next.Item1, x_next.Item2);
            var res = (F_x_next - F_x) / component;
            if((res >= gamma) && (res <= epsila))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool WolfRule(Tuple<double, double> x, Tuple<double, double> x_next, double alfa)
        {
            var component =(Gradtemp.Item1 * Gradtemp.Item1 * -1 + Gradtemp.Item2 * Gradtemp.Item2 * -1);
            var F_x = Function(x.Item1, x.Item2);
            var F_x_next = Function(x_next.Item1, x_next.Item2);
            var Grad_next = AntiGradient(x_next.Item1, x_next.Item2);
            var comp2 = (Grad_next.Item1 * Gradtemp.Item1 * -1 + Gradtemp.Item2 * Grad_next.Item2 * -1);
            if((comp2 > beta * component) && (F_x_next < F_x + epsila * alfa * component))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static double GoldenCut_Calculation(double a, double b)
        {

            FileLine header = new FileLine();
            int n = 1;
            //Console.WriteLine("f(x) = x ** 2");
            //Console.WriteLine("f(x) = cosh(x)");
            //header.X = "f(x) = cosh(x)";
            //header.X = "f(x) = x ** 2";
            //Output.Add(header);
            double tau = (1 + Math.Sqrt(5)) / 2;

            //tau = Math.Round(tau, B);
            double x1 = b - (b - a) / tau;
            bool flipflag = false;
            double d, x2, f1, f2;
            x2 = a + b - x1;
            double fn = F(a);
            f1 = F(x1);
            f2 = F(x2);
            while (Math.Abs(b - a) > delta)
            {
                FileLine line = new FileLine();
                //Print(n);
                line.Iteration_number = n.ToString();
                if (flipflag)
                {
                    x1 = b - (b - a) / tau;
                    f1 = F(x1);
                }
                else
                {
                    x2 = (b - a) / tau + a;

                    f2 = F(x2);
                }
                //line.X_left = x1.ToString();
                //line.X_right = x2.ToString();
                d = 0.00001;
                //line.Func_left = f1.ToString();
                //line.Func_right = f2.ToString();

                if (f1 < f2)
                {
                    b = x2;
                    x2 = x1;
                    f2 = f1;
                    flipflag = true;
                    //line.Comp = "<";
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    f1 = f2;
                    flipflag = false;
                    //line.Comp = ">";
                }
                line.a = a.ToString();
                line.b = b.ToString();
                n++;
                Console.WriteLine();
                //Output.Add(line);
            }
            double x = (b + a) / 2;
            FileLine Down_header = new FileLine();
            //Down_header.X = "x_min = ";
            //Down_header.X_left = x.ToString();
            //Down_header.X_right = "f(x_min) = ";
            //Down_header.Func_left = F(x).ToString();
            //Output.Add(Down_header);
            //sigma = 0;
            //SaveToCSV("X_cosh_test_golden_calculation");
            return x;
        }
        private static double F(double alfa)
        {
            double x1 = Xtemp.Item1 + alfa * Gradtemp.Item1;
            double y1 = Xtemp.Item2 + alfa * Gradtemp.Item2;
            double x = x1 * Math.Cos(FI) - y1 * Math.Sin(FI);
            double y = x1 * Math.Sin(FI) + y1 * Math.Cos(FI);
            //return (x * x + y - 11) * (x * x + y - 11) + (x + y * y - 7) * (x + y * y - 7);
            return (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);
            //return x * x / (A * A) + y * y / (B * B);
            //return x * x + 2 * y * y - 2 * x + y - 5;
        }
        private static double Function(double x1, double y1)
        {

           
            double x = x1 * Math.Cos(FI) - y1 * Math.Sin(FI);
            double y = x1 * Math.Sin(FI) + y1 * Math.Cos(FI);
            return x * x + 10 * y * y;
            //return (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);
            //return (x * x + y - 11) * (x * x + y - 11) + (x + y * y - 7) * (x + y * y - 7);
            //return 1 * x * x + 3 * y * y - 2 * x * y + 1 * x - 4 * y - 4;
            //return x * x / (A * A) + y * y / (B * B);
            return x * x + 2 * y * y - 2 * x + y - 5;
        }
        private static Tuple<double, double> AntiGradient(double x1, double y1)
        {

            
            double x = x1 * Math.Cos(FI) - y1 * Math.Sin(FI);
            double y = x1 * Math.Sin(FI) + y1 * Math.Cos(FI);
            //x1 = 2 * x - 2 * y + 1;
            //y1 = -2 * x + 6 * y - 4;
            //x1 = 2 * x / (A * A);
            //y1 = 2 * y / (B * B);
            //x1 = 2 * x - 2;
            //y1 = 4 * y + 1;
            //x1 = 4 * x * (x * x + y - 11) + 2 * (x + y * y - 7);
            //y1 = 2 * (x * x + y - 11) + 4 * y * (x + y * y - 7);
            //x1 = 2 * (x - 1) - 400 * x * (y - x * x);
            //y1 = 200 * (y - x * x);
            x1 = 2 * x;
            y1 = 20 * y;
            return new Tuple<double, double>(-x1, -y1);
        }
    }
    class ArmihoLine
    {
        public string Iteration { get; set; }
        public string Lambda { get; set; }
        public string X1 { get; set; }
        public string X2 { get; set; }
        public string F_x { get; set; }
    }
}

