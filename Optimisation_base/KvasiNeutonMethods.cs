using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using MathNet.Numerics.LinearAlgebra;

namespace Optimisation_base
{
    class KvasiNeutonMethods
    {
        private static Matrix<double> Xtemp;
        private static Matrix<double> Gradient;
        private static Matrix<double> MoveWay;
        private static int dimenthion = 2;
        private static double epsila = 0.0001;
        private static bool formflag = false;
        private static Matrix<double> Forma_A;
        private static Matrix<double> Forma_b;


        public static void MultiDimIterations()
        {
            formflag = true;
            dimenthion = 10;
            Forma_A = CreateMatrix.RandomPositiveDefinite<double>(dimenthion);
            Matrix<double> x_start = CreateMatrix.Random<double>(dimenthion, 1);
            
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line line = new Kvasi_line();
            var dfp = BFSh(x_start);
            var neuton = Neuton(x_start);
            line.X1 = dfp.Count.ToString();
            line.X2 = neuton.Count.ToString();
            output.Add(line);
            dimenthion = 50;
            Forma_A = CreateMatrix.RandomPositiveDefinite<double>(dimenthion);
            x_start = CreateMatrix.Random<double>(dimenthion, 1);

            
            line = new Kvasi_line();
            dfp = BFSh(x_start);
            neuton = Neuton(x_start);
            line.X1 = dfp.Count.ToString();
            line.X2 = neuton.Count.ToString();
            output.Add(line);
            dimenthion = 100;
            Forma_A = CreateMatrix.RandomPositiveDefinite<double>(dimenthion);
            x_start = CreateMatrix.Random<double>(dimenthion, 1);


            line = new Kvasi_line();
            dfp = BFSh(x_start);
            neuton = Neuton(x_start);
            line.X1 = dfp.Count.ToString();
            line.X2 = neuton.Count.ToString();
            output.Add(line);
            string filename = "Dimenthions_BFSh";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(output);
                }
            }
        }
        public static List<Tuple<double, double>> ParseProcess(double x, double y)
        {
            return ParseOUT(MakKormak(ParseIN(x, y)));
        }
        public static List<Matrix<double>> Neuton(Matrix<double> x){
            List<Matrix<double>> result = new List<Matrix<double>>();
            result.Add(x);
            double norm = 0;
            //Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);
            int iteration = 0;
            //Kvasi_line firstline = new Kvasi_line();
            //firstline.Iteration = iteration.ToString();
            //firstline.X1 = result[iteration][0, 0].ToString();
            //firstline.X2 = result[iteration][1, 0].ToString();
            //output.Add(header);
            //output.Add(firstline);
            do
            {
                Kvasi_line line = new Kvasi_line();
                Gradient = GetGradient(result[iteration]);
                Xtemp = result[iteration];
                MoveWay = -1 * Forma_A.Inverse() * Gradient;
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

                double alfa = GoldenCut_Calculation(firstboarder, secondboarder);
                result.Add(result[iteration] + alfa * MoveWay);
                
                norm = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i, 0] - result[iteration + 1][i, 0], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;
                

            } while (norm > epsila);
            
            return result;
        }
        public static List<Matrix<double>> DFP(Matrix<double> x)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Rosenbrok";
            List<Matrix<double>> result = new List<Matrix<double>>();
            result.Add(x);
            double norm = 0;
            Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);
            int iteration = 0;
            Kvasi_line firstline = new Kvasi_line();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = result[iteration][0, 0].ToString();
            firstline.X2 = result[iteration][1, 0].ToString();
            output.Add(header);
            output.Add(firstline);
            do
            {
                Kvasi_line line = new Kvasi_line();
                Gradient = GetGradient(result[iteration]);
                Xtemp = result[iteration];
                MoveWay = -1 * Q * Gradient;
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

                double alfa = GoldenCut_Calculation(firstboarder, secondboarder);
                result.Add(result[iteration] + alfa * MoveWay);
                var delta_x = result[iteration + 1] - result[iteration];
                var delta_g = GetGradient(result[iteration + 1]) - Gradient;

                //var delta_Q = (delta_x * delta_x.Transpose()) / (delta_x.Transpose() * delta_g)[0, 0]
                // - (Q * delta_g) * (Q * delta_g).Transpose() / ((Q * delta_g).Transpose() * delta_g)[0, 0];
                var part1 = (delta_x * delta_x.Transpose());
                var part2 = (delta_x.Transpose() * delta_g)[0, 0];
                var part3 = (Q * delta_g) * (Q * delta_g).Transpose();
                var part4 = ((Q * delta_g).Transpose() * delta_g)[0, 0];
                var delta_Q = part1 / part2 - part3 / part4;
                Q = Q + delta_Q;
                norm = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i, 0] - result[iteration + 1][i, 0], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;
                line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                line.X1 = result[iteration][0, 0].ToString();
                line.X2 = result[iteration][1, 0].ToString();
                line.F_x = Function(result[iteration]).ToString();
                line.norm_X = norm.ToString();
                output.Add(line);
                
            } while (norm > epsila);
            Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration][0, 0].ToString();
            downheader.X2 = result[iteration][1, 0].ToString();
            downheader.F_x = Function(result[iteration]).ToString();
            output.Add(downheader);
            string filename = "DFP_Rosenbrok_2_2";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(output);
                }
            }
            return result;
        }

        public static List<Matrix<double>> BFSh(Matrix<double> x)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Rosenbrok";
            List<Matrix<double>> result = new List<Matrix<double>>();
            result.Add(x);
            double norm = 0;
            Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);
            int iteration = 0;
            Kvasi_line firstline = new Kvasi_line();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = result[iteration][0, 0].ToString();
            firstline.X2 = result[iteration][1, 0].ToString();
            output.Add(header);
            output.Add(firstline);
            do
            {
                Kvasi_line line = new Kvasi_line();
                Gradient = GetGradient(result[iteration]);
                Xtemp = result[iteration];
                MoveWay = -1 * Q * Gradient;
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

                double alfa = GoldenCut_Calculation(firstboarder, secondboarder);
                result.Add(result[iteration] + alfa * MoveWay);
                var delta_x = result[iteration + 1] - result[iteration];
                var delta_g = GetGradient(result[iteration + 1]) - Gradient;

                //var delta_Q = (delta_x * delta_x.Transpose()) / (delta_x.Transpose() * delta_g)[0, 0]
                // - (Q * delta_g) * (Q * delta_g).Transpose() / ((Q * delta_g).Transpose() * delta_g)[0, 0];
                var part1 = (delta_x - Q * delta_g) * delta_x.Transpose() + delta_x * (delta_x - Q * delta_g).Transpose();
                var part2 = (delta_x.Transpose() * delta_g)[0, 0];
                var part3 = ((delta_x - Q * delta_g).Transpose() * delta_g)[0,0] * (delta_x * delta_x.Transpose());
                var part4 = Math.Pow((delta_x.Transpose() * delta_g)[0, 0], 2);
                var delta_Q = part1 / part2 - part3 / part4;
                Q = Q + delta_Q;
                norm = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i, 0] - result[iteration + 1][i, 0], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;
                line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                line.X1 = result[iteration][0, 0].ToString();
                line.X2 = result[iteration][1, 0].ToString();
                line.F_x = Function(result[iteration]).ToString();
                line.norm_X = norm.ToString();
                output.Add(line);

            } while (norm > epsila);
            Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration][0, 0].ToString();
            downheader.X2 = result[iteration][1, 0].ToString();
            downheader.F_x = Function(result[iteration]).ToString();
            output.Add(downheader);
            string filename = "BFSh_Rosenbrok_2_2";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(output);
                }
            }
            return result;
        }
        public static List<Matrix<double>> Pauel(Matrix<double> x)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Himmelblau";
            List<Matrix<double>> result = new List<Matrix<double>>();
            result.Add(x);
            double norm = 0;
            Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);
            int iteration = 0;
            Kvasi_line firstline = new Kvasi_line();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = result[iteration][0, 0].ToString();
            firstline.X2 = result[iteration][1, 0].ToString();
            output.Add(header);
            output.Add(firstline);
            do
            {
                Kvasi_line line = new Kvasi_line();
                Gradient = GetGradient(result[iteration]);
                Xtemp = result[iteration];
                MoveWay = -1 * Q * Gradient;
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

                double alfa = GoldenCut_Calculation(firstboarder, secondboarder);
                result.Add(result[iteration] + alfa * MoveWay);
                var delta_x = result[iteration + 1] - result[iteration];
                var delta_g = GetGradient(result[iteration + 1]) - Gradient;

                //var delta_Q = (delta_x * delta_x.Transpose()) / (delta_x.Transpose() * delta_g)[0, 0]
                // - (Q * delta_g) * (Q * delta_g).Transpose() / ((Q * delta_g).Transpose() * delta_g)[0, 0];
                var part1 = delta_x + Q * delta_g;
                var part2 = part1 * part1.Transpose();
                //var part3 = (Q * delta_g) * (Q * delta_g).Transpose();
                var part4 = (delta_g.Transpose() * part1)[0,0];
                var delta_Q = -1 * part2 / part4;
                Q = Q + delta_Q;
                norm = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i, 0] - result[iteration + 1][i, 0], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;
                line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                line.X1 = result[iteration][0, 0].ToString();
                line.X2 = result[iteration][1, 0].ToString();
                line.F_x = Function(result[iteration]).ToString();
                line.norm_X = norm.ToString();
                output.Add(line);

            } while (norm > epsila);
            Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration][0, 0].ToString();
            downheader.X2 = result[iteration][1, 0].ToString();
            downheader.F_x = Function(result[iteration]).ToString();
            output.Add(downheader);
            string filename = "Pauel_HRosenbrok_2_2";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(output);
                }
            }
            return result;
        }
        public static List<Matrix<double>> MakKormak(Matrix<double> x)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Rosenbrok";
            List<Matrix<double>> result = new List<Matrix<double>>();
            result.Add(x);
            double norm = 0;
            Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);
            int iteration = 0;
            Kvasi_line firstline = new Kvasi_line();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = result[iteration][0, 0].ToString();
            firstline.X2 = result[iteration][1, 0].ToString();
            output.Add(header);
            output.Add(firstline);
            do
            {
                Kvasi_line line = new Kvasi_line();
                Gradient = GetGradient(result[iteration]);
                Xtemp = result[iteration];
                MoveWay = -1 * Q * Gradient;
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

                double alfa = GoldenCut_Calculation(firstboarder, secondboarder);
                result.Add(result[iteration] + alfa * MoveWay);
                var delta_x = result[iteration + 1] - result[iteration];
                var delta_g = GetGradient(result[iteration + 1]) - Gradient;
                var part1 = delta_x + Q * delta_g;
                
                var part4 = (delta_g.Transpose() * delta_x)[0, 0];
                var delta_Q = -1 * (part1 * delta_x.Transpose()) / part4;
                Q = Q + delta_Q;
                norm = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i, 0] - result[iteration + 1][i, 0], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;
                line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                line.X1 = result[iteration][0, 0].ToString();
                line.X2 = result[iteration][1, 0].ToString();
                line.F_x = Function(result[iteration]).ToString();
                line.norm_X = norm.ToString();
                output.Add(line);
                //restart
                if(iteration % dimenthion == 0)
                {
                    Q = CreateMatrix.Diagonal<double>(dimenthion, dimenthion, 1);
                }

            } while (norm > epsila);
            Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration][0, 0].ToString();
            downheader.X2 = result[iteration][1, 0].ToString();
            downheader.F_x = Function(result[iteration]).ToString();
            output.Add(downheader);
            string filename = "Kormak_Rosenbrok_2_2";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(output);
                }
            }
            return result;
        }
        public static Matrix<double> ParseIN(double x, double y)
        {
            Matrix<double> x_vector = CreateMatrix.Dense(2, 1, 0.0);
            x_vector[0, 0] = x;
            x_vector[1, 0] = y;
            return x_vector;
        }
        public static List<Tuple<double, double>>ParseOUT(List<Matrix<double>> output)
        {
            List<Tuple<double, double>> result = new List<Tuple<double, double>>();
            foreach(var vector in output)
            {
                var x = vector[0, 0];
                var y = vector[1, 0];
                result.Add(new Tuple<double, double>(x, y));
            }
            return result;
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
            while (Math.Abs(b - a) > epsila)
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
            if (formflag)
            {
                var x_vec = Xtemp + alfa * MoveWay;
                return ((Forma_A * x_vec).Transpose() * x_vec)[0, 0] + (Forma_b.Transpose()* x_vec)[0,0];
            }
            double x1 = Xtemp[0,0] + alfa * MoveWay[0,0];
            double y1 = Xtemp[1,0] + alfa * MoveWay[1,0];
            double FI = 0;
            double x = x1 * Math.Cos(FI) - y1 * Math.Sin(FI);
            double y = x1 * Math.Sin(FI) + y1 * Math.Cos(FI);
            //return x * x + y * y - 4 * x - 2 * y;
            //return (x * x + y - 11) * (x * x + y - 11) + (x + y * y - 7) * (x + y * y - 7);
            return (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);
            //return x * x / (A * A) + y * y / (B * B);
            //return x * x + 2 * y * y - 2 * x + y - 5;
        }
        private static Matrix<double> GetGradient(Matrix<double> x_vector)
        {
            if (formflag)
            {
                return 2 * Forma_A * x_vector + Forma_b;
            }
            //x1 = 4 * x * (x * x + y - 11) + 2 * (x + y * y - 7);
            //y1 = 2 * (x * x + y - 11) + 4 * y * (x + y * y - 7);
            double x = x_vector[0, 0];
            double y = x_vector[1, 0];
            double x1, y1;
            //x1 = 4 * x * (x * x + y - 11) + 2 * (x + y * y - 7);
            //y1 = 2 * (x * x + y - 11) + 4 * y * (x + y * y - 7);
            x1 = 2 * (x - 1) - 400 * x * (y - x * x);
            y1 = 200 * (y - x * x);
            Matrix<double> gradient = CreateMatrix.Dense(dimenthion, 1, 0.0);
            gradient[0, 0] = x1;
            gradient[1, 0] = y1;
            return gradient;
        }

        private static double Function(Matrix<double> x_vector)
        {
            if (formflag)
            {
                return ((Forma_A * x_vector).Transpose() * x_vector)[0, 0] + (Forma_b.Transpose() * x_vector)[0, 0];
            }
            double x = x_vector[0, 0];
            double y = x_vector[1, 0];
            return (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);
            //return (x * x + y - 11) * (x * x + y - 11) + (x + y * y - 7) * (x + y * y - 7);
        }
    }
    class Kvasi_line
    {
        public string Iteration { get; set; }
        public string Lambda { get; set; }
        //public string deltaX1 { get; set; }
        //public string deltaX2 { get; set; }
        public string X1 { get; set; }
        public string X2 { get; set; }
        public string F_x { get; set; }
        //public string dF_dX1 { get; set; }

        //public string dF_dX2 { get; set; }
        public string norm_X { get; set; }


    }
}
