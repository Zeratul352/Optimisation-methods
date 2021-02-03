using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Optimisation_base
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var a = Dichotromia_evaluation.CalcMin(-5, 10);
            //KvasiNeutonMethods.ExploreRozenbrok();
            //return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            KvasiNeutonMethods.forma = new Form1();
            Application.Run(KvasiNeutonMethods.forma);
        }
    }
}
