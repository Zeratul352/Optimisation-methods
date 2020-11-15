using System;
using System.Collections.Generic;
using CsvHelper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Optimisation_base
{
    class Dichotromia_evaluation
    {
        public static int A = 0;
        int B = 15;
        static double delta = 0.0001;
        static double sigma = 0;
        static NormalRandom shum = new NormalRandom();
        //double ddd = shum
        public List<FileLine> Output = new List<FileLine>();
        public List<double> Fibonachi = new List<double>();

        public void CalcFibonachi(int count)
        {
            double a = 0;
            double b = 1;
            Fibonachi.Add(a);
            Fibonachi.Add(b);
            for(int i = 0; i < count - 2; i++)
            {
                a = a + b;
                b = a - b;
                Fibonachi.Add(a);
            }
        }
        public void CountChanges()
        {
            List<FileLine> Output1 = new List<FileLine>();
            //sigma = 0;
            for(int i = 1; i < 20; i++)
            {
                A = 0;
                //sigma = 0.1 * i + 0.1;
                FileLine line = new FileLine();
                //line.X = i.ToString();
                /*double n = Math.Abs(SquareInterpolation(-2, 1) + 1);
                double n1 = Math.Abs(SquareApproximation(-2, 1) + 1);
                if(n1 == Double.NaN)
                {
                    n1 = 0;
                }
                for (int j = 0; j < 20; j++)
                {
                    n += Math.Abs(SquareInterpolation(-2, 1) + 1);
                    var temp = Math.Abs(SquareApproximation(-2, 1) + 1);
                    if(temp == Double.NaN)
                    {
                        temp = 0;
                    }
                    n1 += temp;
                }
                n = n / 21;
                n1 = n1 / 21;
                //delta = delta / 10;
                line.X_right = n1.ToString();
                line.X_left = n.ToString();
                line.X = sigma.ToString();*/
                var n = Golden_combinated(-2, 1, i);
                line.X_prev = i.ToString();
                line.X_min = A.ToString();
                Output1.Add(line);
                
            }
            /*var x_interpol = SquareInterpolation(-2, 1);
            var x_approx = SquareApproximation(-2, 1);
            var x_comb = Golden_combinated(-2, 1, 5);
            FileLine line_interp = new FileLine();
            FileLine line_approx = new FileLine();
            FileLine line_comb = new FileLine();
            line_interp.X_left = x_interpol.ToString();
            line_interp.X_right = F(x_interpol).ToString();
            line_interp.Iteration_number = "Interpolation";
            line_approx.Iteration_number = "Approximation";
            line_approx.X_left = x_approx.ToString();
            line_approx.X_right = F(x_approx).ToString();
            line_comb.Iteration_number = "Combinated";
            line_comb.X_left = x_comb.ToString();
            line_comb.X_right = F(x_comb).ToString();
            Output1.Add(line_interp);
            Output1.Add(line_approx);
            Output1.Add(line_comb);*/
            string filename = "X_polynom_combinated";
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {

                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(Output1);
                }
            }
            Output1.Clear();
        }
        /*public double CalcMin(double a, double b)
        {
            FileLine header = new FileLine();
            int n = 1;
            Console.WriteLine("f(x) = x ** 2");
            //Console.WriteLine("f(x) = cosh(x)");
            //header.X = "f(x) = cosh(x)";
            header.X = "f(x) = x ** 2";
            Output.Add(header);
            double x = (a + b) / 2;
            double d, x1, x2, f1, f2;
            double fn = F(a);
            
            while(Math.Abs(b - a) > delta)
            {
                FileLine line = new FileLine();
                Print(n);
                line.Iteration_number = n.ToString();
                x = (a + b) / 2;
                Print(x);
                line.X = x.ToString();
                d = delta / 10;
                //d = Math.Abs((b - a) / 100);
                x1 = x - d;
                Print(x1);
                line.X_left = x1.ToString();
                x2 = x + d;
                Print(x2);
                line.X_right = x2.ToString();
                f1 = F(x1);
                f2 = F(x2);
                fn = F(x);
                Print(f1);
                line.Func_left = f1.ToString();
                if (f1 < f2)
                {
                    b = x2;
                    Console.Write(" < |");
                    line.Comp = "<";
                }
                else
                {
                    Console.Write(" > |");
                    a = x1;
                    line.Comp = ">";
                }
                Print(f2);
                line.Func_right = f2.ToString();
                Print(a);
                line.a = a.ToString();
                Print(b);
                line.b = b.ToString();
                x = (a + b) / 2;
                n++;
                Console.WriteLine();
                Output.Add(line);
            }
            FileLine Down_header = new FileLine();
            Down_header.X = "x_min = ";
            Down_header.X_left = x.ToString();
            Down_header.X_right = "f(x_min) = ";
            Down_header.Func_left = F(x).ToString();
            Output.Add(Down_header);
            //SaveToCSV("X_square_test");
            //sigma = 0;
            return Math.Abs(F(x) - 1);
        }
        public double GoldenCut_Simetria(double a, double b)
        {
            FileLine header = new FileLine();
            int n = 1;
            Console.WriteLine("f(x) = x ** 2");
            //Console.WriteLine("f(x) = cosh(x)");
            //header.X = "f(x) = cosh(x)";
            header.X = "f(x) = x ** 2";
            Output.Add(header);
            double tau = (1 + Math.Sqrt(5)) / 2;
            tau = Math.Round(tau, B);
            double x1 = b - (b - a) / tau;
            bool flipflag = false;
            double d, x2, f1, f2;
            x2 = a + b - x1;
            double fn = F(a);
            f1 = F(x1);
            f2 = F(x2);
            while (Math.Abs(b - a) > delta && a < 0)
            {
                FileLine line = new FileLine();
                Print(n);
                line.Iteration_number = n.ToString();
                if (flipflag)
                {
                    x1 = a + b - x2;
                    f1 = F(x1);
                }
                else
                {
                    x2 = b - x1 + a;
                    f2 = F(x2);
                }
                line.X_left = x1.ToString();
                line.X_right = x2.ToString();
                d = 0.00001;
                line.Func_left = f1.ToString();
                line.Func_right = f2.ToString();
                
                if(f1 < f2)
                {
                    b = x2;
                    x2 = x1;
                    f2 = f1;
                    flipflag = true;
                    line.Comp = "<";
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    f1 = f2;
                    flipflag = false;
                    line.Comp = ">";
                }
                line.a = a.ToString();
                line.b = b.ToString();
                n++;
                Console.WriteLine();
                Output.Add(line);
            }
            double x = (b + a) / 2;
            FileLine Down_header = new FileLine();
            Down_header.X = "x_min = ";
            Down_header.X_left = x.ToString();
            Down_header.X_right = "f(x_min) = ";
            Down_header.Func_left = F(x).ToString();
            Output.Add(Down_header);
            //sigma = 0;
            //SaveToCSV("X_cosh_test_golden_simetrya");
            return Math.Abs(F(x) - 1);
        }

        public double Fibonachi_calculations(double a, double b)
        {
            FileLine header = new FileLine();
            int n = 1;
            Console.WriteLine("f(x) = x ** 2");
            //Console.WriteLine("f(x) = cosh(x)");
            header.X = "f(x) = cosh(x)";
            //header.X = "f(x) = x ** 2";
            Output.Add(header);
            CalcFibonachi(100);
            int expected_n = 10;
            while((Fibonachi[expected_n + 1] - Fibonachi[expected_n - 1]) * delta < (b - a))
            {
                expected_n++;
            }
            expected_n = expected_n - 1;
            a = b - (Fibonachi[expected_n + 1] - Fibonachi[expected_n - 1]) * delta;
            double tau = (1 + Math.Sqrt(5)) / 2;
            double l1 = b - a;
            double x1 = b - (Fibonachi[expected_n] - Fibonachi[expected_n - 2]) * delta;
            bool flipflag = false;
            double d, x2, f1, f2;
            x2 = a + b - x1;
            double fn = F(a);
            f1 = F(x1);
            f2 = F(x2);
            while (Math.Abs(b - a) > delta)
            {
                FileLine line = new FileLine();
                Print(n);
                line.Iteration_number = n.ToString();
                if (flipflag)
                {
                    x1 = a + b - x2;
                    f1 = F(x1);
                }
                else
                {
                    x2 = b - x1 + a;
                    f2 = F(x2);
                }
                line.X_left = x1.ToString();
                line.X_right = x2.ToString();
                d = 0.00001;
                line.Func_left = f1.ToString();
                line.Func_right = f2.ToString();

                if (f1 < f2)
                {
                    b = x2;
                    x2 = x1;
                    f2 = f1;
                    flipflag = true;
                    line.Comp = "<";
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    f1 = f2;
                    flipflag = false;
                    line.Comp = ">";
                }
                line.a = a.ToString();
                line.b = b.ToString();
                n++;
                Console.WriteLine();
                Output.Add(line);
            }
            double x = (b + a) / 2;
            FileLine Down_header = new FileLine();
            Down_header.X = "x_min = ";
            Down_header.X_left = x.ToString();
            Down_header.X_right = "f(x_min) = ";
            Down_header.Func_left = F(x).ToString();
            Output.Add(Down_header);
            //sigma = 0;
            //SaveToCSV("X_cosh_test_Fibonachi");
            return x;
        }
        public double GoldenCut_Calculation(double a, double b)
        {
            FileLine header = new FileLine();
            int n = 1;
            Console.WriteLine("f(x) = x ** 2");
            //Console.WriteLine("f(x) = cosh(x)");
            header.X = "f(x) = cosh(x)";
            //header.X = "f(x) = x ** 2";
            Output.Add(header);
            double tau = (1 + Math.Sqrt(5)) / 2;
            
            tau = Math.Round(tau, B);
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
                Print(n);
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
                line.X_left = x1.ToString();
                line.X_right = x2.ToString();
                d = 0.00001;
                line.Func_left = f1.ToString();
                line.Func_right = f2.ToString();

                if (f1 < f2)
                {
                    b = x2;
                    x2 = x1;
                    f2 = f1;
                    flipflag = true;
                    line.Comp = "<";
                }
                else
                {
                    a = x1;
                    x1 = x2;
                    f1 = f2;
                    flipflag = false;
                    line.Comp = ">";
                }
                line.a = a.ToString();
                line.b = b.ToString();
                n++;
                Console.WriteLine();
                Output.Add(line);
            }
            double x = (b + a) / 2;
            FileLine Down_header = new FileLine();
            Down_header.X = "x_min = ";
            Down_header.X_left = x.ToString();
            Down_header.X_right = "f(x_min) = ";
            Down_header.Func_left = F(x).ToString();
            Output.Add(Down_header);
            sigma = 0;
            //SaveToCSV("X_cosh_test_golden_calculation");
            return x;
        }*/

        public double Golden_combinated(double a, double b, int N)
        {
            FileLine header = new FileLine();
            int n = 1;
            Console.WriteLine("f(x) = x ** 2");
            //Console.WriteLine("f(x) = cosh(x)");
            //header.X = "f(x) = cosh(x)";
            //header.X = "f(x) = x ** 2";
            Output.Add(header);
            double tau = (1 + Math.Sqrt(5)) / 2;

            tau = Math.Round(tau, B);
            double x1 = b - (b - a) / tau;
            bool flipflag = false;
            double d, x2, f1, f2;
            x2 = a + b - x1;
            //double fn = F(a);
            f1 = F(x1);
            f2 = F(x2);
            for(int i = 0; i < N; i++)
            {
                FileLine line = new FileLine();
                Print(n);
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
            Console.WriteLine(A);
            /*double x = (b + a) / 2;
            FileLine Down_header = new FileLine();
            Down_header.X = "x_min = ";
            Down_header.X_left = x.ToString();
            Down_header.X_right = "f(x_min) = ";
            Down_header.Func_left = F(x).ToString();
            Output.Add(Down_header);
            sigma = 0;
            //SaveToCSV("X_cosh_test_golden_calculation");
            return x;*/
            //int n = 0;
            double F1, F2, F3;
            if (flipflag)
            {
                x2 = x1;
                F2 = f1;
            }
            else
            {
                F2 = f2;
            }
            x1 = a;
            double x3 = b;
            //x2 = (b + a) / 2;

            
            F1 = F(x1);
            //F2 = F(x2);
            F3 = F(x3);
            do
            {
                n++;
                Print(n - N - 1);
                Console.WriteLine();
                double[,] matrix = new double[3, 4];

                matrix[0, 0] = x1 * x1;
                matrix[1, 0] = x2 * x2;
                matrix[2, 0] = x3 * x3;
                matrix[0, 1] = x1;
                matrix[1, 1] = x2;
                matrix[2, 1] = x3;
                matrix[0, 2] = 1;
                matrix[1, 2] = 1;
                matrix[2, 2] = 1;
                matrix[0, 3] = F1;
                matrix[1, 3] = F2;
                matrix[2, 3] = F3;
                var res = KrammerCalc(matrix);
                double c2 = res.Item1;
                double c1 = res.Item2;
                double x_min = -c1 / (2 * c2);
                double F_min = F(x_min);
                if (Math.Abs(x2 - x_min) < delta)
                {
                    Console.WriteLine(A);
                    return x_min;
                }
                else
                {

                    if (x1 < x_min && x_min < x2)
                    {
                        if (F_min < F2)
                        {
                            x3 = x2;
                            F3 = F2;
                            x2 = x_min;
                            F2 = F_min;
                        }
                        else
                        {
                            x1 = x_min;
                            F1 = F_min;
                        }
                    }
                    else if (x2 < x_min && x_min < x3)
                    {
                        if (F_min < F2)
                        {
                            x1 = x2;
                            F1 = F2;
                            x2 = x_min;
                            F2 = F_min;
                        }
                        else
                        {
                            x3 = x_min;
                            F3 = F_min;
                        }
                    }
                    //x2 = x_min;
                }

            } while (true);
        }
        public double SquareInterpolation(double a, double b)
        {
            FileLine header = new FileLine();
            header.X_prev = "y = 5x^3 - 3x^5";
            Output.Add(header);
            int n = 0;
            double x1 = a;
            double x3 = b;
            double x2 = (b + a) / 2;
            double x_min, F_min;
            double F1, F2, F3;
            F1 = F(x1);
            F2 = F(x2);
            F3 = F(x3);
            do
            {
                FileLine line = new FileLine();
                n++;
                double[,] matrix = new double[3, 4];
                
                matrix[0, 0] = x1 * x1;
                matrix[1, 0] = x2 * x2;
                matrix[2, 0] = x3 * x3;
                matrix[0, 1] = x1;
                matrix[1, 1] = x2;
                matrix[2, 1] = x3;
                matrix[0, 2] = 1;
                matrix[1, 2] = 1;
                matrix[2, 2] = 1;
                matrix[0, 3] = F1;
                matrix[1, 3] = F2;
                matrix[2, 3] = F3;
                var res = KrammerCalc(matrix);
                double c2 = res.Item1;
                double c1 = res.Item2;
                x_min = -c1 / (2 * c2);
                F_min = F(x_min);
                line.Iteration_number = n.ToString();
                line.X_prev = x2.ToString();
                line.X_min = x_min.ToString();
                line.Func_prev = F2.ToString();
                line.Func_min = F_min.ToString();
                line.a = x1.ToString();
                line.b = x3.ToString();
                Output.Add(line);
                if(Math.Abs(x2 - x_min) < delta)
                {
                    break;
                    //return x_min;
                }
                else
                {
                    
                    if(x1 < x_min && x_min < x2)
                    {
                        if(F_min < F2)
                        {
                            x3 = x2;
                            F3 = F2;
                            x2 = x_min;
                            F2 = F_min;
                        }
                        else
                        {
                            x1 = x_min;
                            F1 = F_min;
                        }
                    }
                    else if(x2 < x_min && x_min < x3)
                    {
                        if (F_min < F2)
                        {
                            x1 = x2;
                            F1 = F2;
                            x2 = x_min;
                            F2 = F_min;
                        }
                        else
                        {
                            x3 = x_min;
                            F3 = F_min;
                        }
                    }
                    //x2 = x_min;
                }
               
            } while (true);
            FileLine Down_header = new FileLine();
            Down_header.X_prev = "x_min = ";
            Down_header.X_min = x_min.ToString();
            Down_header.Func_prev = "f(x_min) = ";
            Down_header.Func_min = F_min.ToString();
            Output.Add(Down_header);
            SaveToCSV("X_polynom_interpolation");
            return x_min;
        }

        public double SquareApproximation(double a, double b)
        {
            FileLine header = new FileLine();
            header.X_prev = "y = 5x^3 - 3x^5";
            Output.Add(header);
            double h, x1, x2, x3, F1, F2, F3, x_min, F_min;
            x2 = (a + b) / 2;
            x1 = a;
            x3 = b;
            h = delta;
            int n = 0;
            x_min = x2;
            F_min = F(x_min);
            int intervals = 10;
            do
            {
                FileLine line = new FileLine();
                n++;
                //x1 = (a + b) / 2;
                h = (x3 - x1) / (intervals + 1);
                Tuple<double, double>[] points = new Tuple<double, double>[intervals + 1];
                for(int i = 1; i <= intervals; i++)
                {
                    points[i] = new Tuple<double, double>(x1 + h * i, F(x1 + h * i));
                }
                double[,] matrix = new double[3, 4];
                for(int i = 0; i < 3; i++)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        matrix[i, j] = 0;
                    }
                }
                matrix[0, 0] = intervals;
                for(int i = 1; i <= intervals; i++)
                {
                    matrix[1, 0] += points[i].Item1;
                    matrix[2, 0] += points[i].Item1 * points[i].Item1;
                    matrix[2, 1] += points[i].Item1 * points[i].Item1 * points[i].Item1;
                    matrix[2, 2] += points[i].Item1 * points[i].Item1 * points[i].Item1 * points[i].Item1;
                    matrix[0, 3] += points[i].Item2;
                    matrix[1, 3] += points[i].Item2 * points[i].Item1;
                    matrix[2, 3] += points[i].Item1 * points[i].Item1 * points[i].Item2;
                }
                matrix[0, 1] = matrix[1, 0];
                matrix[1, 1] = matrix[2, 0];
                matrix[0, 2] = matrix[2, 0];
                matrix[1, 2] = matrix[2, 1];
                var result = KrammerCalc(matrix);
                if(result.Item3 == 0)
                {
                    break;
                }
                x_min = -1 * result.Item2 / (2 * result.Item3);
                double A = result.Item3;
                double B = result.Item2;
                double C = result.Item1;
                //F_min = A * x_min * x_min + B * x_min + C;
                //F2 = A * x2 * x2 + B * x2 * C;
                F_min = F(x_min);
                F2 = F(x2);
                line.Iteration_number = n.ToString();
                line.X_prev = x2.ToString();
                line.X_min = x_min.ToString();
                line.Func_prev = F2.ToString();
                line.Func_min = F_min.ToString();
                line.a = x1.ToString();
                line.b = x3.ToString();
                Output.Add(line);
                //F1 = F(a);
                //F3 = F(b);
                if (Math.Abs(x2 - x_min) < delta)
                {
                    break;
                }
                else
                {

                    if (x1 < x_min && x_min < x2)
                    {
                        if (F_min < F2)
                        {
                            x3 = x2;
                            F3 = F2;
                            x2 = x_min;
                            F2 = F_min;
                        }
                        else
                        {
                            x1 = x_min;
                            F1 = F_min;
                        }
                    }
                    else if (x2 < x_min && x_min < x3)
                    {
                        if (F_min < F2)
                        {
                            x1 = x2;
                            F1 = F2;
                            x2 = x_min;
                            F2 = F_min;
                        }
                        else
                        {
                            x3 = x_min;
                            F3 = F_min;
                        }
                    }
                }
                    
                } while (n < 30);
            //Print(n);
            FileLine Down_header = new FileLine();
            Down_header.X_prev = "x_min = ";
            Down_header.X_min = x_min.ToString();
            Down_header.Func_prev = "f(x_min) = ";
            Down_header.Func_min = F_min.ToString();
            Output.Add(Down_header);
            SaveToCSV("X_polynom_approximation");
            return x_min;
        }
        public static Tuple<double, double, double> KrammerCalc(double[,] matrix)
        {
            //Tuple<double, double, double> result = new Tuple<double, double, double>();
            double delta = matrix[0, 0] * matrix[1, 1] * matrix[2, 2] +
                matrix[1, 0] * matrix[0, 2] * matrix[2, 1] +
                matrix[0, 1] * matrix[1, 2] * matrix[2, 0] -
                matrix[2, 0] * matrix[1, 1] * matrix[0, 2] -
                matrix[0, 1] * matrix[1, 0] * matrix[2, 2] -
                matrix[0, 0] * matrix[2, 1] * matrix[1, 2];
            double deltax = matrix[0, 3] * matrix[1, 1] * matrix[2, 2] +
                matrix[1, 3] * matrix[0, 2] * matrix[2, 1] +
                matrix[0, 1] * matrix[1, 2] * matrix[2, 3] -
                matrix[2, 3] * matrix[1, 1] * matrix[0, 2] -
                matrix[0, 1] * matrix[1, 3] * matrix[2, 2] -
                matrix[0, 3] * matrix[2, 1] * matrix[1, 2];
            double deltay = matrix[0, 0] * matrix[1, 3] * matrix[2, 2] +
                matrix[1, 0] * matrix[0, 2] * matrix[2, 3] +
                matrix[0, 3] * matrix[1, 2] * matrix[2, 0] -
                matrix[2, 0] * matrix[1, 3] * matrix[0, 2] -
                matrix[0, 3] * matrix[1, 0] * matrix[2, 2] -
                matrix[0, 0] * matrix[2, 3] * matrix[1, 2];
            double deltaz = matrix[0, 0] * matrix[1, 1] * matrix[2, 3] +
                matrix[1, 0] * matrix[0, 3] * matrix[2, 1] +
                matrix[0, 1] * matrix[1, 3] * matrix[2, 0] -
                matrix[2, 0] * matrix[1, 1] * matrix[0, 3] -
                matrix[0, 1] * matrix[1, 0] * matrix[2, 3] -
                matrix[0, 0] * matrix[2, 1] * matrix[1, 3];
            return new Tuple<double, double, double>(deltax / delta, deltay / delta, deltaz / delta);
        }
        private void SaveToCSV(string filename)
        {
            using (StreamWriter writer = new StreamWriter(filename + ".csv"))
            {
                using (CsvWriter csvWriter = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {
                    
                    csvWriter.Configuration.Delimiter = ";";
                    csvWriter.WriteRecords(Output);
                }
            }
            Output.Clear();
        }
        private static void Print(double data)
        {
            
            Console.Write("{0:f15}" , Math.Round(data, 15) + " | ");
        }
        private static double F(double x)
        {
            A++;
            //return x * x + shum.NextDouble() * sigma * 0.0;
            //return (x -2) * (x - 2);
            return 5 * x*x*x - 3 * x*x*x*x*x + shum.NextDouble() * sigma * 0.0;
            return Math.Cosh(x) + shum.NextDouble() * sigma * 0.01;
            //return Math.Pow(Math.E, x) / 2 + Math.Pow(Math.E, -x) / 2;
        }
    }
    class NormalRandom : Random
    {
        // сохранённое предыдущее значение
        double prevSample = double.NaN;
        protected override double Sample()
        {
            // есть предыдущее значение? возвращаем его
            if (!double.IsNaN(prevSample))
            {
                double result = prevSample;
                prevSample = double.NaN;
                return result;
            }

            // нет? вычисляем следующие два
            // Marsaglia polar method из википедии
            double u, v, s;
            do
            {
                u = 2 * base.Sample() - 1;
                v = 2 * base.Sample() - 1; // [-1, 1)
                s = u * u + v * v;
            }
            while (u <= -1 || v <= -1 || s >= 1 || s == 0);
            double r = Math.Sqrt(-2 * Math.Log(s) / s);

            prevSample = r * v;
            return r * u;
        }
    }

    class FileLine
    {
        public string Iteration_number { get; set; }
        public string X_prev { get; set; }
        public string X_min { get; set; }
        //public string X { get; set; }
        //public string X_left { get; set; }
        //public string X_right { get; set; }
        public string Func_prev { get; set; }
        //public string Comp { get; set; }
        public string Func_min { get; set; }
        public string a { get; set; }
        public string b { get; set; }

    }
}
