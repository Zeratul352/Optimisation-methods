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
        private static Vector<double> Xtemp;
        private static Vector<double> Gradient;
        private static Vector<double> MoveWay;
        private static int dimenthion = 2;
        private static double epsila = 0.0001;
        private static bool formflag = false;
        private static bool roseflag = false;
        private static bool logflag = true;
        private static Matrix<double> Forma_A;
        private static Vector<double> Forma_b;
        private static int RoundNum = 10;
        private static int CountGradient = 0;
        private static int CountFunction = 0;
        public static Form1 forma;


        public static void MultiDimIterations()
        {
            formflag = true;
            logflag = false;
            List<Kvasi_line> output = new List<Kvasi_line>();
            int[] dimensions = { 2, 5, 10, 50, 100, 250, 500, 1000};
            for (int i = 0; i <= 7; i++)
            {
                dimenthion = dimensions[i];
                Forma_A = CreateMatrix.RandomPositiveDefinite<double>(dimenthion);
                Forma_b = CreateVector.Dense<double>(dimenthion, 0);
                Vector<double> x_start = CreateVector.Dense<double>(dimenthion, 1);

                
                Kvasi_line line = new Kvasi_line();
                line.Iteration = dimenthion.ToString();
                var timer = DateTime.Now;
                CountGradient = 0;
                CountFunction = 0;
                var neuton = DFP(x_start);
                line.Lambda = ((DateTime.Now - timer).Milliseconds / 1000.0 + (DateTime.Now - timer).Seconds
                    + (DateTime.Now - timer).Minutes * 60).ToString();
                line.X1 = CountFunction.ToString();
                line.X2 = CountGradient.ToString();
                /*timer = DateTime.Now;
                var mns = MNS(x_start);
                line.X1 = ((DateTime.Now - timer).Milliseconds / 1000.0 + (DateTime.Now - timer).Seconds).ToString();
                timer = DateTime.Now;
                var dfp = DFP(x_start);
                line.X2 = ((DateTime.Now - timer).Milliseconds / 1000.0 + (DateTime.Now - timer).Seconds).ToString();
                timer = DateTime.Now;
                var msg = MSG(x_start);
                line.F_x = ((DateTime.Now - timer).Milliseconds / 1000.0 + (DateTime.Now - timer).Seconds).ToString();
                timer = DateTime.Now;*/

                output.Add(line);
            }
            /*Matrix<double> origin = CreateMatrix.Dense<double>(dimenthion, dimenthion);
            origin[0, 0] = 1;
            origin[1, 1] = 2;
            Forma_b = CreateMatrix.Dense<double>(dimenthion, 1);
            int[] ellips = { 1, 2, 5, 10, 20, 35, 50, 75, 100 };
            for(int i = 0; i <= 8; i++)
            {
                double FI = Math.PI * i / 4;
                Matrix<double> rotator = CreateMatrix.Dense<double>(dimenthion, dimenthion);
                rotator[0, 0] = 1;
                rotator[1, 1] = 1 * ellips[i];
                //rotator[0, 0] = Math.Cos(FI);
                //rotator[0, 1] = -Math.Sin(FI);
                //rotator[1, 0] = Math.Sin(FI);
                //rotator[1, 1] = Math.Cos(FI);
                Forma_A = rotator;
                Kvasi_line line = new Kvasi_line();
                line.Iteration = ellips[i].ToString();
                line.Lambda = MSG(ParseIN(3, 3)).Count.ToString();
                output.Add(line);
            }*/
            /*dimenthion = 50;
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
            output.Add(line);*/
            string filename = "Dimensions_DFP1";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(output);
                }
            }
        }

        public static void ExploreRozenbrok()
        {
            roseflag = true;
            logflag = false;
            List<Kvasi_line> output = new List<Kvasi_line>();
            int[] dimensions = { 2, 10, 50, 100, 250 };
            for (int i = 0; i <= 4; i++)
            {
                dimenthion = dimensions[i];
                
                Vector<double> x_start = CreateVector.Dense<double>(dimenthion, 1);
                for(int k = 0; k < dimenthion; k+= 2)
                {
                    x_start[k] = -1.2;
                    x_start[k + 1] = 1;
                }

                Kvasi_line line = new Kvasi_line();
                line.Iteration = dimenthion.ToString();
                var timer = DateTime.Now;
                CountGradient = 0;
                CountFunction = 0;
                var neuton = HelderMid(x_start);
                line.Lambda = ((DateTime.Now - timer).Milliseconds / 1000.0 + (DateTime.Now - timer).Seconds
                    + (DateTime.Now - timer).Minutes * 60).ToString();
                line.X1 = CountFunction.ToString();
                line.X2 = CountGradient.ToString();
                

                output.Add(line);
            }
            
            string filename = "Dimensions_Rosenbrok_HelderMid_fix_250";
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
            //formflag = true;
            Forma_A = CreateMatrix.Dense<double>(dimenthion, dimenthion);
            Forma_A[0, 0] = 1;
            Forma_A[1, 1] = 2;
            Forma_A[1, 0] = 0;
            Forma_A[0, 1] = 0;
            Forma_b = CreateVector.Dense<double>(2, 1);
            Forma_b[0] = -2;
            Forma_b[1] = 1;
            //roseflag = true;
            return ParseOUT(HukJivs(ParseIN(x, y)));
        }
        public static List<Vector<double>> Neuton(Vector<double> x)
        {
            List<Vector<double>> result = new List<Vector<double>>();
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
                Matrix<double> matrix = CreateMatrix.Dense<double>(dimenthion, dimenthion);
                if (roseflag)
                {
                    
                    for(int i = 0; i < dimenthion/2; i++)
                    {
                        //matrix[1, 1] = 2 - 400 * y + 1200 * x * x;
                        //matrix[1, 0] = 400 * x;
                        //matrix[0, 1] = matrix[1, 0];
                        //matrix[0, 0] = 200;
                        matrix[2 * i, 2 * i] = 2 - 400 * Xtemp[i * 2 + 1] + 1200 * Xtemp[2 * i] * Xtemp[2 * i];
                        matrix[2 * i, 2 * i + 1] = -400 * Xtemp[2 * i];
                        matrix[2 * i + 1, 2 * i] = -400 * Xtemp[2 * i];
                        matrix[2 * i + 1, 2 * i + 1] = 200;
                    }
                    matrix.Inverse();
                }
                MoveWay = -1 * matrix * Gradient;
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
                    norm += Math.Pow(result[iteration][i] - result[iteration + 1][i], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;


            } while (norm > epsila);

            return result;
        }
        public static List<Vector<double>> DFP(Vector<double> x)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Rosenbrok";
            List<Vector<double>> result = new List<Vector<double>>();
            result.Add(x);
            double norm = 0;
            Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);
            int iteration = 0;
            /*Kvasi_line firstline = new Kvasi_line();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = result[iteration][0, 0].ToString();
            firstline.X2 = result[iteration][1, 0].ToString();
            output.Add(header);
            output.Add(firstline);*/
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
                var part1 = delta_x.OuterProduct(delta_x);
                var part2 = (delta_x * delta_g);
                var part3 = (Q * delta_g).OuterProduct(Q * delta_g);
                var part4 = ((Q * delta_g) * delta_g);
                var delta_Q = part1 / part2 - part3 / part4;
                Q = Q + delta_Q;
                norm = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i] - result[iteration + 1][i], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;
                /*line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                line.X1 = result[iteration][0, 0].ToString();
                line.X2 = result[iteration][1, 0].ToString();
                line.F_x = Function(result[iteration]).ToString();
                line.norm_X = norm.ToString();
                output.Add(line);*/

            } while (norm > epsila);
            /*Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration][0, 0].ToString();
            downheader.X2 = result[iteration][1, 0].ToString();
            downheader.F_x = Function(result[iteration]).ToString();
            output.Add(downheader);*/
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

        public static List<Vector<double>> BFSh(Vector<double> x)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Rosenbrok";
            List<Vector<double>> result = new List<Vector<double>>();
            result.Add(x);
            double norm = 0;
            Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);
            int iteration = 0;
            Kvasi_line firstline = new Kvasi_line();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = result[iteration][0].ToString();
            firstline.X2 = result[iteration][1].ToString();
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
                var part1 = (delta_x - Q * delta_g).OuterProduct(delta_x) + delta_x.OuterProduct(delta_x - Q * delta_g);
                var part2 = (delta_x * delta_g);
                var part3 = ((delta_x - Q * delta_g) * delta_g) * (delta_x.OuterProduct(delta_x));
                var part4 = Math.Pow((delta_x * delta_g), 2);
                var delta_Q = part1 / part2 - part3 / part4;
                Q = Q + delta_Q;
                norm = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i] - result[iteration + 1][i], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;
                line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                line.X1 = result[iteration][0].ToString();
                line.X2 = result[iteration][1].ToString();
                line.F_x = Function(result[iteration]).ToString();
                line.norm_X = norm.ToString();
                output.Add(line);

            } while (norm > epsila);
            Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration][0].ToString();
            downheader.X2 = result[iteration][1].ToString();
            downheader.F_x = Function(result[iteration]).ToString();
            output.Add(downheader);
            string filename = "BFSh_Rosenbrok_fix";
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
        public static List<Vector<double>> Pauel(Vector<double> x)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Himmelblau";
            List<Vector<double>> result = new List<Vector<double>>();
            result.Add(x);
            double norm = 0;
            Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);
            int iteration = 0;
            Kvasi_line firstline = new Kvasi_line();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = result[iteration][0].ToString();
            firstline.X2 = result[iteration][1].ToString();
            output.Add(header);
            output.Add(firstline);
            do
            {
                Kvasi_line line = new Kvasi_line();
                Gradient = GetGradient(result[iteration]);
                Xtemp = result[iteration];
                MoveWay = 1 * Q * Gradient;
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
                var part2 = part1.OuterProduct(part1);
                //var part3 = (Q * delta_g) * (Q * delta_g).Transpose();
                var part4 = (delta_g * part1);
                var delta_Q = -1 * part2 / part4;
                Q = Q + delta_Q;
                norm = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i] - result[iteration + 1][i], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;
                line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                line.X1 = result[iteration][0].ToString();
                line.X2 = result[iteration][1].ToString();
                line.F_x = Function(result[iteration]).ToString();
                line.norm_X = norm.ToString();
                output.Add(line);
                if (iteration % 6 == 0)
                {
                    Q = CreateMatrix.Diagonal<double>(dimenthion, dimenthion, 1);
                }
            } while (norm > epsila);
            Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration][0].ToString();
            downheader.X2 = result[iteration][1].ToString();
            downheader.F_x = Function(result[iteration]).ToString();
            output.Add(downheader);
            string filename = "MakKormak_Rosenbrok_fix";
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
        public static List<Vector<double>> MakKormak(Vector<double> x)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Rosenbrok";
            List<Vector<double>> result = new List<Vector<double>>();
            result.Add(x);
            double norm = 0;
            double normF = 0;
            Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);
            int iteration = 0;
            Kvasi_line firstline = new Kvasi_line();
            firstline.Iteration = iteration.ToString();
            firstline.X1 = result[iteration][0].ToString();
            firstline.X2 = result[iteration][1].ToString();
            output.Add(header);
            output.Add(firstline);
            bool restartflag = true;
            while(true)
            {
                
                Kvasi_line line = new Kvasi_line();
                Gradient = GetGradient(result[iteration]);
                Xtemp = result[iteration];
                MoveWay = -1 * Q * Gradient;
                double firstboarder = 0;
                double alfatemp = 0.00001;
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
                var delta_y = delta_x + Q * delta_g;

                var koef = (delta_g * delta_x);
                var delta_Q = 1 * (delta_y.OuterProduct(delta_x)) / koef;
                Q = Q + delta_Q;
                Q[0, 0] = Round(Q[0, 0]);
                Q[1, 0] = Round(Q[1, 0]);
                Q[0, 1] = Round(Q[0, 1]);
                Q[1, 1] = Round(Q[1, 1]);
                norm = 0;
                normF = 0;
                double normMod = 0;
                double normFMod = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i] - result[iteration + 1][i], 2);
                    normMod += Math.Pow(result[iteration][i], 2);
                }
                norm = Math.Sqrt(norm);
                normMod = Math.Sqrt(normMod);
                norm = norm / normMod;
                normF = Math.Abs(Function(result[iteration + 1]) - Function(result[iteration]));
                normFMod = Math.Abs(Function(result[iteration]));
                //normF = normF / normFMod;
                iteration++;
                line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                line.X1 = result[iteration][0].ToString();
                line.X2 = result[iteration][1].ToString();
                line.F_x = Function(result[iteration]).ToString();
                line.norm_X = norm.ToString();
                output.Add(line);
                //restart
                if ((iteration % 5 == 0) || Q[0,0] <= 0 || Q.Determinant() <= 0)
                {
                    Q = CreateMatrix.Diagonal<double>(dimenthion, dimenthion, 2);
                    line.norm_X = "Restart";
                }
                else
                {
                    if (norm <= epsila && normF <= epsila && !restartflag)
                    {
                        break;
                    }
                    if (norm <= epsila && normF <= epsila)
                    {
                        
                        Q = CreateMatrix.Diagonal<double>(dimenthion, dimenthion, 2);
                        line.norm_X = "Restart";
                        restartflag = false;
                    }
                    else
                    {
                        restartflag = true;
                    }
                }
                
                

            };
            Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration][0].ToString();
            downheader.X2 = result[iteration][1].ToString();
            downheader.F_x = Function(result[iteration]).ToString();
            downheader.norm_X = Math.Sqrt(Math.Pow(result[iteration][0] - 1, 2) + Math.Pow(result[iteration][1] - 1, 2)).ToString();
            output.Add(downheader);
            string filename = "Kormak_Rosenbrok_fix" + x[0].ToString() + '_' + x[1].ToString();
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

        public static List<Vector<double>> MSG(Vector<double> x)
        {
            
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Forma";
            List<Vector<double>> result = new List<Vector<double>>();
            List<Vector<double>> Directions = new List<Vector<double>>();
            int iteration = 0;
            double norm = 0;
            result.Add(x);
            if (logflag)
            {
                Kvasi_line firstline = new Kvasi_line();

                firstline.Iteration = iteration.ToString();
                firstline.X1 = result[iteration][0].ToString();
                firstline.X2 = result[iteration][1].ToString();
                output.Add(header);
                output.Add(firstline);
            }
            
            //////////////////
            //first iteration
            Kvasi_line line = new Kvasi_line();
            
            Gradient = GetGradient(result[iteration]);
            Directions.Add(-1 * Gradient);
            
            Xtemp = result[iteration];
            MoveWay = -1 * Gradient;
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
            //double alfa = -1 * ((Forma_A * result[iteration] + Forma_b).Transpose() * Directions[iteration])[0, 0] /
                //(2 * ((Forma_A * Directions[iteration]).Transpose() * Directions[iteration])[0, 0]);
            result.Add(result[iteration] + alfa * MoveWay);
            /*norm = 0;
            for (int i = 0; i < dimenthion; i++)
            {
                norm += Math.Pow(result[iteration][i, 0] - result[iteration + 1][i, 0], 2);
            }
            norm = Math.Sqrt(norm);*/
            iteration++;
            if (logflag)
            {
                line.Iteration = iteration.ToString();
                line.Lambda = alfa.ToString();
                line.X1 = result[iteration][0].ToString();
                line.X2 = result[iteration][1].ToString();
                line.F_x = Function(result[iteration]).ToString();
                //line.norm_X = norm.ToString();
                output.Add(line);
            }
            
            //////////////////

            //Matrix<double> Q = CreateMatrix.DenseDiagonal(dimenthion, dimenthion, 1.0);


            for(int m = 1; m < dimenthion; m++)
            {
                line = new Kvasi_line();
                Gradient = GetGradient(result[iteration]);
                Xtemp = result[iteration];
                var prevGragient = GetGradient(result[iteration - 1]);

                double beta = (Gradient * (Gradient - prevGragient)) / Math.Pow(prevGragient.L2Norm(), 2);
                //double beta = ((Forma_A * Directions[iteration-1]) * Gradient) /
                   // ((Forma_A * Directions[iteration-1]) * Directions[iteration-1]);
                Directions.Add(-1 * Gradient + beta * Directions[iteration-1]);
                MoveWay = Directions[iteration];

                firstboarder = 0;
                alfatemp = 0.0001;
                direction = 1;
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
                secondboarder = firstboarder + alfatemp * direction;
                if (firstboarder > secondboarder)
                {
                    var bubble = firstboarder;
                    firstboarder = secondboarder;
                    secondboarder = bubble;
                }

                alfa = GoldenCut_Calculation(firstboarder, secondboarder);
                //alfa = -1 * ((Forma_A * result[iteration] + Forma_b).Transpose() * Directions[iteration])[0, 0] /
                //(2 * ((Forma_A * Directions[iteration]).Transpose() * Directions[iteration])[0, 0]);
                result.Add(result[iteration] + alfa * MoveWay);
                
                norm = 0;
                for (int i = 0; i < dimenthion; i++)
                {
                    norm += Math.Pow(result[iteration][i] - result[iteration + 1][i], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;
                if (logflag)
                {
                    line.Iteration = iteration.ToString();
                    line.Lambda = alfa.ToString();
                    line.X1 = result[iteration][0].ToString();
                    line.X2 = result[iteration][1].ToString();
                    line.F_x = Function(result[iteration]).ToString();
                    //line.norm_X = norm.ToString();
                    output.Add(line);
                }
                if(norm < epsila)
                {
                    break;
                }
                

            }
            if (!logflag)
            {
                return result;
            }
            Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration][0].ToString();
            downheader.X2 = result[iteration][1].ToString();
            downheader.F_x = Function(result[iteration]).ToString();
            output.Add(downheader);
            string filename = "MSG_Forma_test_ulia";
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


        public static List<Vector<double>> HelderMid(Vector<double> x_start)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "x^2 + 2y^2 - x + 2y";
            output.Add(header);
            List<Vector<double>> result = new List<Vector<double>>();
            List<Vector<double>> simplex = new List<Vector<double>>();

            result.Add(x_start.Clone());

            simplex.Add(x_start);
            double a = 1;
            double d1 = a * (Math.Sqrt(dimenthion) + dimenthion - 1) / (dimenthion * Math.Sqrt(2));
            double d2 = a * (Math.Sqrt(dimenthion + 1) - 1) / (dimenthion * Math.Sqrt(2));
            for(int i = 0; i < dimenthion; i++)//initialisation
            {
                var vector = x_start.Clone();
                //Vector<double> vector = CreateVector.Dense<double>(dimenthion);
                for(int j = 0; j < dimenthion; j++)
                {
                    if(i == j)
                    {
                        vector[j] += d1;
                    }
                    else
                    {
                        vector[j] += d2;
                    }
                }
                simplex.Add(vector);
                
            }

            double alfa = 1;
            int iteration = 0;

            /////////////////////////////////
            Kvasi_line line1 = new Kvasi_line();
            line1.Iteration = iteration.ToString();
            line1.Lambda = "( " + simplex[0][0].ToString() + "; " + simplex[0][1].ToString() + ")";
            line1.X1 = "( " + simplex[1][0].ToString() + "; " + simplex[1][1].ToString() + ")";
            line1.X2 = "( " + simplex[2][0].ToString() + "; " + simplex[2][1].ToString() + ")";
            iteration++;
            output.Add(line1);
            ///////////////////////////

            while (true)
            {
                forma.DrawTriangle(ParseOUT(simplex));
                List<double> Functions = new List<double>();
                
                for(int i = 0; i <= dimenthion; i++)
                {
                    Functions.Add(Function(simplex[i]));
                }
                double worstFunc = Functions[0];
                double bestFunc = worstFunc;
                int worstindex = 0;
                int bestindex = 0;

                for (int i = 1; i <= dimenthion; i++)
                {
                    if(Functions[i] > worstFunc)
                    {
                        worstFunc = Functions[i];
                        worstindex = i;
                    }else if(Functions[i] < bestFunc)
                    {
                        bestFunc = Functions[i];
                        bestindex = i;
                    }
                }
                double secondworstFunc = bestFunc;
                int secondworstindex = bestindex;
                for(int i = 0; i <= dimenthion; i++)
                {
                    if(Functions[i] > secondworstFunc && i != worstindex)
                    {
                        secondworstFunc = Functions[i];
                        secondworstindex = i;
                    }
                }
                if (simplex[bestindex] != result[result.Count - 1] && iteration > 0)
                {
                    
                    Kvasi_line line = new Kvasi_line();
                    if (logflag)
                    {
                        line.Iteration = iteration.ToString();
                        line.Lambda = "( " + simplex[0][0].ToString() + "; " + simplex[0][1].ToString() + ")";
                        line.X1 = "( " + simplex[1][0].ToString() + "; " + simplex[1][1].ToString() + ")";
                        line.X2 = "( " + simplex[2][0].ToString() + "; " + simplex[2][1].ToString() + ")";
                        line.F_x = bestFunc.ToString();
                        output.Add(line);
                        
                        
                        iteration++;

                    }
                    result.Add(simplex[bestindex]);
                    var delta = result[result.Count - 1] - result[result.Count - 2];
                    if (delta.L2Norm() < epsila && iteration > 3)
                    {
                        break;
                    }
                }
                Vector<double> middle = CreateVector.Dense<double>(dimenthion);
                for(int i = 0; i <= dimenthion; i++)
                {
                    if(i != worstindex)
                    {
                        middle += simplex[i];
                    }
                }
                middle /= dimenthion;

                var x_mirrow = middle + alfa * (middle - simplex[worstindex]);
                var F_mirrow = Function(x_mirrow);
                if (F_mirrow < bestFunc)
                {
                    var x_long = middle + 2 * alfa * (x_mirrow - middle);
                    if (Function(x_long) < F_mirrow)
                    {
                        simplex[worstindex] = x_long;
                    }
                    else
                    {
                        simplex[worstindex] = x_mirrow;
                    }
                }
                else
                {
                    if(F_mirrow < secondworstFunc)
                    {
                        simplex[worstindex] = x_mirrow;
                    }
                    else
                    {
                        
                        if (F_mirrow < worstFunc)
                        {
                            var bubble_vec = simplex[worstindex];
                            simplex[worstindex] = x_mirrow;
                            x_mirrow = bubble_vec;
                            var bubble = worstFunc;
                            worstFunc = F_mirrow;
                            F_mirrow = bubble;
                        }
                        var x_short = alfa * 0.5 * simplex[worstindex] + (1 - alfa * 0.5) * middle;
                        var F_short = Function(x_short);
                        if (F_short < worstFunc)
                        {
                            simplex[worstindex] = x_short;
                        }
                        else
                        {
                            for (int i = 0; i <= dimenthion; i++)
                            {
                                if (i != bestindex)
                                {
                                    var new_vector = CreateVector.Dense<double>(dimenthion);
                                    new_vector = simplex[bestindex] + 0.5 * (simplex[i] - simplex[bestindex]);
                                    simplex[i] = new_vector;
                                    Kvasi_line line = new Kvasi_line();
                                    line.Iteration = "Shrinking";
                                    output.Add(line);
                                }
                            }
                        }
                    }
                    

                }
                /*iteration++;
                Kvasi_line line = new Kvasi_line();
                line.Iteration = iteration.ToString();
                line.Lambda = "( " + simplex[0][0].ToString() + "; " + simplex[0][1].ToString() + ")";
                line.X1 = "( " + simplex[1][0].ToString() + "; " + simplex[1][1].ToString() + ")";
                line.X2 = "( " + simplex[2][0].ToString() + "; " + simplex[2][1].ToString() + ")";
                line.F_x = bestFunc.ToString();
                output.Add(line);*/
                
                

            }
            if (logflag)
            {
                Kvasi_line downheader = new Kvasi_line();
                downheader.Lambda = "Extremum";
                downheader.X1 = result[result.Count - 1][0].ToString();
                downheader.X2 = result[result.Count - 1][1].ToString();
                downheader.F_x = Function(result[result.Count - 1]).ToString();
                output.Add(downheader);
                string filename = "HelderMid_Testing" + "_" + result[0][0].ToString() + "_" + result[0][1].ToString();
                using (StreamWriter writer = new StreamWriter(filename + ".csv"))
                {
                    using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                    {

                        csvWriter.Configuration.Delimiter = ";";
                        csvWriter.WriteRecords(output);
                    }
                }
            }
            
            return result;
        }

        public static List<Vector<double>> HukJivs(Vector<double> x)
        {
            List<Kvasi_line> output = new List<Kvasi_line>();
            Kvasi_line header = new Kvasi_line();
            header.Lambda = "Rosenbrok";
            output.Add(header);
            List<Vector<double>> result = new List<Vector<double>>();
            Vector<double> steps = CreateVector.Dense<double>(dimenthion, 1);
            int iteration = 0;
           
            var x_prev = x.Clone();
            var x_zero = x.Clone();
            result.Add(x_prev.Clone());
            /*if (formflag)
            {
                result.AddRange(ExploreSearch(x, steps, false));
                x = Xtemp;
            }
            else
            {
                result.AddRange(ExploreSearch(x, steps));
                x = Xtemp;
            }*/

            result.AddRange(ExploreSearch(x, steps));
            x = result[result.Count - 1];
            x_prev = result[result.Count - 2];
            x_zero = result[result.Count - 3];
            
            //result.Add(x.Clone());
            bool stepflag = true;
            while (true)
            {
                if (stepflag)
                {
                    /*var x_temp = (x + (x - x_prev)).Clone();
                    var way = ExploreSearch(x_temp, steps, false);
                    var x_next = Xtemp;
                    if(Function(x_next) < Function(x))
                    {
                        x_prev = x.Clone();
                        x = x_next.Clone();
                        result.AddRange(way);
                    }
                    else
                    {
                        stepflag = false;
                    }*/
                    MoveWay = x - x_zero;
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
                    x = x + alfa * MoveWay;
                    stepflag = false;
                    continue;
                }
                if((x - x_zero).L2Norm() < epsila && formflag)
                {
                    break;
                }
                else
                {
                    if(formflag)
                        steps /= 2;
                    x_prev = x.Clone();
                    var vay = ExploreSearch(x, steps);
                    x = Xtemp;
                    //if(formflag)
                       // x = ExploreSearch(x, steps, false);
                    //else
                       // x = ExploreSearch(x, steps);
                    if (Function(x) <= Function(x_prev))
                    {
                        stepflag = true;
                        result.AddRange(vay);
                        x = result[result.Count - 1];
                        x_prev = result[result.Count - 2];
                        x_zero = result[result.Count - 3];
                    }
                    else
                    {
                        stepflag = false;
                    }
                }
                if (!formflag)
                {
                    if((x - x_zero).L2Norm() < epsila)
                    {
                        break;
                    }
                    /*int exitnum = 0;
                    for (int i = 0; i < dimenthion; i++)
                    {
                        if (steps[i] < epsila)
                        {
                            exitnum++;
                        }
                    }
                    if (exitnum == dimenthion)
                    {
                        break;
                    }*/
                }
                

            }
            foreach(var point in result)
            {
                Kvasi_line line = new Kvasi_line();
                line.Iteration = iteration.ToString();
                iteration++;
                line.X1 = point[0].ToString();
                line.X2 = point[1].ToString();
                line.F_x = Function(point).ToString();
                output.Add(line);
            }
            Kvasi_line downheader = new Kvasi_line();
            downheader.Lambda = "Extremum";
            downheader.X1 = result[iteration - 1][0].ToString();
            downheader.X2 = result[iteration - 1][1].ToString();
            downheader.F_x = Function(result[iteration - 1]).ToString();
            output.Add(downheader);
            string filename = "Huk_Jivs_Testing_fixed" + "_" + result[0][0].ToString() + "_" + result[0][1].ToString()+ "_" + dimenthion.ToString();
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

        private static List<Vector<double>> ExploreSearch(Vector<double> x_input, Vector<double> steps, bool cutdown = true)
        {
            if(cutdown == false)
            {
                MoveWay = x_input;
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

            }
            Vector<double> x = x_input.Clone();
            List<Vector<double>> route = new List<Vector<double>>();
            route.Add(x);
            for(int i = 0; i < dimenthion; i++)
            {

                //Gradient = GetGradient(result[iteration]);
                Xtemp = x;
                MoveWay = CreateVector.Dense<double>(dimenthion);
                MoveWay[i] = 1;
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
                x += alfa * MoveWay;
                route.Add(x.Clone());
                /*if(steps[i] < epsila)
                {
                    continue;
                }
                
                double F = Function(x);
                Vector<double> x_up = x.Clone();
                Vector<double> x_down = x.Clone();
                
                x_up[i] += steps[i];
                var F_up = Function(x_up);
                
                x_down[i] += -1 * steps[i];
                var F_down = Function(x_down);
                if(F < F_up && F < F_down)
                {
                    if (cutdown)
                        steps[i] /= 2;
                    continue;

                }
                if(F_down < F_up)
                {
                    x = x_down;
                }
                else
                {
                    x = x_up;
                }*/


            }
            Xtemp = x;
            return route;
        }

        /*private static Vector<double> SampleSearch(Vector<double> X1, Vector<double> X2, Vector<double> steps)
        {
            var X3 = X1 + 2 * (X2 - X1);
            var X4 = ExploreSearch(X3, steps, false);
            if(X3 != X4)
            {
                return SampleSearch(X2, X4, steps);
            }
            else
            {
                return X3;
            }
        }*/

        public static List<Vector<double>> MNS(Vector<double> x)
        {
            List<Vector<double>> result = new List<Vector<double>>();
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
                MoveWay = -1 * Gradient;
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
                    norm += Math.Pow(result[iteration][i] - result[iteration + 1][i], 2);
                }
                norm = Math.Sqrt(norm);
                iteration++;


            } while (norm > epsila);

            return result;
        }

        private static double Round(double x)
        {
            return Math.Round(x, RoundNum);
        }
        public static Vector<double> ParseIN(double x, double y)
        {
            Vector<double> x_vector = CreateVector.Dense(2, 0.0);
            x_vector[0] = x;
            x_vector[1] = y;
            return x_vector;
        }
        public static List<Tuple<double, double>> ParseOUT(List<Vector<double>> output)
        {
            List<Tuple<double, double>> result = new List<Tuple<double, double>>();
            foreach (var vector in output)
            {
                var x = vector[0];
                var y = vector[1];
                result.Add(new Tuple<double, double>(x, y));
            }
            return result;
        }
        public static double GoldenCut_Calculation(double a, double b)
        {

            FileLine header = new FileLine();
            int n = 1;
            
            double tau = (1 + Math.Sqrt(5)) / 2;

            
            double x1 = b - (b - a) / tau;
            bool flipflag = false;
            double d, x2, f1, f2;
            x2 = a + b - x1;
            double fn = F(a);
            f1 = F(x1);
            f2 = F(x2);
            while (Math.Abs(b - a) > 0.0001)
            {
                FileLine line = new FileLine();
                
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
                
                d = 0.00001;
                

                if (f1 < f2)
                {
                    b = x2;
                    x2 = x1;
                    f2 = f1;
                    flipflag = true;
                    
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    f1 = f2;
                    flipflag = false;
                    
                }
                line.a = a.ToString();
                line.b = b.ToString();
                n++;
                Console.WriteLine();
                
            }
            double x = (b + a) / 2;
            FileLine Down_header = new FileLine();
            
            return x;
        }

        private static double F(double alfa)
        {
            if (formflag)
            {
                CountFunction++;
                var x_vec = Xtemp + alfa * MoveWay;
                return ((Forma_A * x_vec) * x_vec) + (Forma_b * x_vec);
            }
            if (roseflag)
            {
                CountFunction++;
                var x_vector = Xtemp + alfa * MoveWay;
                double res = 0;
                for (int i = 0; i < dimenthion / 2; i += 1)
                {
                    res += 100 * Math.Pow((Math.Pow(x_vector[2 * i], 2) - x_vector[2 * i + 1]), 2) +
                        Math.Pow(x_vector[2 * i] - 1, 2);
                }
                return res;
            }
            double x1 = Xtemp[0] + alfa * MoveWay[0];
            double y1 = Xtemp[1] + alfa * MoveWay[1];
            double FI = 0;
            double x = x1 * Math.Cos(FI) - y1 * Math.Sin(FI);
            double y = x1 * Math.Sin(FI) + y1 * Math.Cos(FI);
            //return x * x + y * y - 4 * x - 2 * y;
            return (x * x + y - 11) * (x * x + y - 11) + (x + y * y - 7) * (x + y * y - 7);
            //return (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);
            //return x * x / (A * A) + y * y / (B * B);
            //return x * x + 2 * y * y - 2 * x + y - 5;
        }
        private static Vector<double> GetGradient(Vector<double> x_vector)
        {
            if (formflag)
            {
                CountGradient++;
                return 2 * Forma_A * x_vector + Forma_b;
            }
            //x1 = 4 * x * (x * x + y - 11) + 2 * (x + y * y - 7);
            //y1 = 2 * (x * x + y - 11) + 4 * y * (x + y * y - 7);
            Vector<double> gradient = CreateVector.Dense<double>(dimenthion, 0);
            for(int i = 0; i < dimenthion/2; i++)
            {
                CountGradient++;
                double x = x_vector[2 * i];
                double y = x_vector[2 * i + 1];
                double x1, y1;
                x1 = 2 * (x - 1) - 400 * x * (y - x * x);
                y1 = 200 * (y - x * x);
                gradient[i * 2] = x1;
                gradient[2 * i + 1] = y1;
            }
            /*double x = x_vector[0];
            double y = x_vector[1];
            double x1, y1;
            //x1 = 4 * x * (x * x + y - 11) + 2 * (x + y * y - 7);
            //y1 = 2 * (x * x + y - 11) + 4 * y * (x + y * y - 7);
            x1 = 2 * (x - 1) - 400 * x * (y - x * x);
            y1 = 200 * (y - x * x);
            
            gradient[0] = x1;
            gradient[1] = y1;*/
            return gradient;
        }

        private static double Function(Vector<double> x_vector)
        {
            if (formflag)
            {
                CountFunction++;
                return ((Forma_A * x_vector) * x_vector) + (Forma_b * x_vector);
            }
            if (roseflag)
            {
                CountFunction++;
                double res = 0;
                for (int i = 0; i < dimenthion/2; i += 1)
                {
                    res += 100 * Math.Pow((Math.Pow(x_vector[2 * i], 2) - x_vector[2 * i + 1]), 2) +
                        Math.Pow(x_vector[2 * i] - 1, 2);
                }
                return res;
            }
            double x = x_vector[0];
            double y = x_vector[1];
            //double res = (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);
            //return res;
            return (x * x + y - 11) * (x * x + y - 11) + (x + y * y - 7) * (x + y * y - 7);
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