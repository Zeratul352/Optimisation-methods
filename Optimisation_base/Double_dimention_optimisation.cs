using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet;

namespace Optimisation_base
{
    class Double_dimention_optimisation
    {
        public static double delta = 0.003;
        private static Tuple<double, double> Xtemp;
        private static Tuple<double, double> MovedownWay;
        private static double FI = 0;
        private static int A = 1;
        private static int B = 2;

        public static void Rotation()
        {
            List<Double_dim_line> Output = new List<Double_dim_line>();
            int[] multiplier = new int[7];
            multiplier[0] = 1;
            multiplier[1] = 5;
            multiplier[2] = 10;
            multiplier[3] = 20;
            multiplier[4] = 50;
            multiplier[5] = 70;
            multiplier[6] = 100;
            for(int i = 0; i <= 8; i++)
            {
                //B = multiplier[i];
                FI = i * Math.PI / 4;
                var line = new Double_dim_line();
                //double dist = Math.Sqrt(Math.Pow(i * 0.5, 2) * 2);
                line.Iteration = i.ToString();
                var min = FastestMovedown(4, 5);
                var c = min.Count();
                line.deltaX1 = c.ToString();
                Output.Add(line);
            }
            string filename = "Neuton_rotation1";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(Output);
                }
            }
        }
        public static List<Tuple<double, double>> FastestMovedown(double x, double y)
        {
            List<Double_dim_line> Output = new List<Double_dim_line>();
            Double_dim_line header = new Double_dim_line();
            header.Iteration = "F(x,y) = x^2 + 2y^2 -4x + 2y";
            Output.Add(header);
            List<Tuple<double, double>> Allpoints = new List<Tuple<double, double>>();
            Allpoints.Add(new Tuple<double, double>(x, y));
            int iteration = 0;
            double norm = 1;
            Xtemp = Allpoints[iteration];
            MovedownWay = SecondTierWay(Xtemp.Item1, Xtemp.Item2);
            Double_dim_line firstline = new Double_dim_line();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = x.ToString();
            firstline.X2 = y.ToString();
            firstline.F_x = Function(x, y).ToString();
            firstline.dF_dX1 = MovedownWay.Item1.ToString();
            firstline.dF_dX2 = MovedownWay.Item2.ToString();
            Output.Add(firstline);
            do
            {
                Double_dim_line line = new Double_dim_line();
                
                double firstboarder = 0;
                double alfatemp = 0.0001;
                int direction = 1;
                if (F(firstboarder) > F(firstboarder + alfatemp))
                {
                    direction = 1;
                }
                else
                {
                    direction = -1;
                }
                while (F(firstboarder) > F(firstboarder + alfatemp * direction))
                {
                    alfatemp *= 2;
                }
                double secondboarder = firstboarder + alfatemp * direction;
                if (firstboarder > secondboarder)
                {
                    var bubble = firstboarder;
                    firstboarder = secondboarder;
                    secondboarder = bubble;
                }
                var alfa = 1;// GoldenCut_Calculation(firstboarder, secondboarder);               
                Allpoints.Add(new Tuple<double, double>(Allpoints[iteration].Item1 + alfa * MovedownWay.Item1, Allpoints[iteration].Item2 + alfa * MovedownWay.Item2));
                iteration++;
                norm = Math.Sqrt(Math.Pow(Allpoints[iteration].Item1 - Allpoints[iteration - 1].Item1, 2)
            + Math.Pow(Allpoints[iteration].Item2 - Allpoints[iteration - 1].Item2, 2));
                Xtemp = Allpoints[iteration];
                MovedownWay = SecondTierWay(Xtemp.Item1, Xtemp.Item2);
                line.Iteration = iteration.ToString();
                //line.Lambda = alfa.ToString();
                line.deltaX1 = (alfa * MovedownWay.Item1).ToString();
                line.deltaX2 = (alfa * MovedownWay.Item2).ToString();
                line.X1 = Xtemp.Item1.ToString();
                line.X2 = Xtemp.Item2.ToString();
                line.F_x = Function(Xtemp.Item1, Xtemp.Item2).ToString();
                line.dF_dX1 = MovedownWay.Item1.ToString();
                line.dF_dX2 = MovedownWay.Item2.ToString();
                line.norm_X = norm.ToString();
                Output.Add(line);
            } while (norm > delta);
            Double_dim_line downheader = new Double_dim_line();
            downheader.deltaX2 = "Extremum";
            downheader.X1 = Xtemp.Item1.ToString();
            downheader.X2 = Xtemp.Item2.ToString();
            downheader.F_x = Function(Xtemp.Item1, Xtemp.Item2).ToString();
            Output.Add(downheader);
            string filename = "Prot_9_11_20_11";
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
            double x1 = Xtemp.Item1 + alfa * MovedownWay.Item1;
            double y1 = Xtemp.Item2 + alfa * MovedownWay.Item2;
            double x = x1 * Math.Cos(FI) - y1 * Math.Sin(FI);
            double y = x1 * Math.Sin(FI) + y1 * Math.Cos(FI);
            return x * x + y * y - 4 * x - 2 * y;
            //return (x * x + y - 11) * (x * x + y - 11) + (x + y * y - 7) * (x + y * y - 7);
            //return (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);
            //return x * x / (A * A) + y * y / (B * B);
            //return x * x + 2 * y * y - 2 * x + y - 5;
        }
        private static double Function(double x1, double y1)
        {
            
            
            double x = x1 * Math.Cos(FI) - y1 * Math.Sin(FI);
            double y = x1 * Math.Sin(FI) + y1 * Math.Cos(FI);
            //return (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);
            //return (x * x + y - 11) * (x * x + y - 11) + (x + y * y - 7) * (x + y * y - 7);
            //return 1 * x * x + 3 * y * y - 2 * x * y + 1 * x - 4 * y - 4;
            //return x * x / (A * A) + y * y / (B * B);
            return x * x +   y * y - 4 * x - 2 * y;
            //return x * x + 2 * y * y - 2 * x + y - 5;
        }
        private static Tuple<double, double> AntiGradient(double x1, double y1)
        {


            double x = x1;// x1 * Math.Cos(FI) - y1 * Math.Sin(FI);
            double y = y1;// x1 * Math.Sin(FI) + y1 * Math.Cos(FI);
            //x1 = 2 * x - 2 * y + 1;
            //y1 = -2 * x + 6 * y - 4;
            //x1 = 2 * x / (A * A);
            //y1 = 2 * y / (B * B);
            //x1 = 2 * x - 2;
            //y1 = 4 * y + 1;
            //x1 = 4 * x * (x * x + y - 11) + 2 * (x + y * y - 7);
            //y1 = 2 * (x * x + y - 11) + 4 * y * (x + y * y - 7);
            x1 = 2 * x - (4 * Math.Cos(FI) + 2 * Math.Sin(FI));
            y1 = 2 * y - (4 * Math.Sin(FI) - 2 * Math.Cos(FI));
            //x1 = 2 * (x - 1) - 400 * x * (y - x * x);
            //y1 = 200 * (y - x * x);
            return new Tuple<double, double>(-x1, -y1);
        }
        private static Tuple<double, double> SecondTierWay(double x1, double y1)
        {
            var antigrad = AntiGradient(x1, y1);
            double[,] matrix = new double[2, 2];
            double x = x1 * Math.Cos(FI) - y1 * Math.Sin(FI);
            double y = x1 * Math.Sin(FI) + y1 * Math.Cos(FI);
            //matrix[1, 1] = 12 * x * x + 4 * y - 42;//calc algeb dopolnenie matrix
            //matrix[1, 0] =-1 * ( 4 * x + 4 * y);
            //matrix[0, 1] = matrix[1, 0];
            //matrix[0, 0] = 12 * y * y + 4 * x - 26;
            //matrix[1, 1] = 2 - 400 * y + 1200 * x * x;
            //matrix[1, 0] = 400 * x;
            //matrix[0, 1] = matrix[1, 0];
            //matrix[0, 0] = 200;
            matrix[1, 1] = 2;
            matrix[0, 0] = 2;
            matrix[1, 0] = 0;
            matrix[0, 1] = 0;
            //matrix[1, 1] = 2 / (A * A);
            //matrix[0, 0] = 2 / (B * B);
            //matrix[1, 0] = 0;
            //matrix[0, 1] = 0;
            double norm = matrix[1, 1] * matrix[0, 0] - matrix[1, 0] * matrix[0, 1];
            x1 = matrix[0, 0] * antigrad.Item1 / norm + matrix[0, 1] * antigrad.Item2 / norm;
            y1 = matrix[1, 0] * antigrad.Item1 / norm + matrix[1, 1] * antigrad.Item2 / norm;
            return new Tuple<double, double>(x1, y1);
        }
    }
    class Double_dim_line
    {
        public string Iteration { get; set; }
        //public string Lambda { get; set; }
        public string deltaX1 { get; set; }
        public string deltaX2 { get; set; }
        public string X1 { get; set; }
        public string X2 { get; set; }
        public string F_x { get; set; }
        public string dF_dX1 { get; set; }
        
        public string dF_dX2 { get; set; }
        public string norm_X { get; set; }
       

    }
}
