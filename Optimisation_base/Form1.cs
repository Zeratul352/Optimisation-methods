using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Optimisation_base
{
    public partial class Form1 : Form
    {
        private static int size = 601;
        private static int modifx = size / 2;
        private static int modify = size / 2;
        private static double deltax;
        private static double deltay;
        private static int definitionmultiplier = 5;
        bool GridFlag = false;
        public static Bitmap bitmap = new Bitmap(size, size);
        List<Tuple<double, double>> points = new List<Tuple<double, double>>();
        static double[,] AllValues = new double[size * definitionmultiplier, size * definitionmultiplier];
        static List<List<Point>> LevelLines = new List<List<Point>>();
        double maximum;
        double minimum;
        private static bool LineFlag = true;
        public Form1()
        {
            InitializeComponent();
            CalcModificators();
        }

        

        private void DoCalculations()
        {
            maximum = F(Convert.ToDouble(CentreXBox.Text), Convert.ToDouble(CentreYBox.Text));
            minimum = maximum;
            for(int i = 0; i < size; i++)
            {
                //CalcRowAsync(i);
                CalcRowAsync(i);
            }
            
        }
        private async void CalcRowAsync(int i)
        {
            await Task.Run(() => CalcRow(i));
        }
        private void CalcRow(int i)
        {
            for (int j = 0; j < size; j++)
            {
                for (int x = 0; x < definitionmultiplier; x++)
                {
                    for (int y = 0; y < definitionmultiplier; y++)
                    {
                        AllValues[i * definitionmultiplier + x, j * definitionmultiplier + y] = F((i - modifx + x * 1.0 / definitionmultiplier) / deltax, (j - modify + y * 1.0 / definitionmultiplier) / deltay * -1);//can cause rotation!
                        if (AllValues[i * definitionmultiplier + x, j * definitionmultiplier + y] > maximum)
                        {
                            maximum = AllValues[i * definitionmultiplier + x, j * definitionmultiplier + y];
                        }
                        else if (AllValues[i * definitionmultiplier + x, j * definitionmultiplier + y] < minimum)
                        {
                            minimum = AllValues[i * definitionmultiplier + x, j * definitionmultiplier + y];
                        }
                    }
                }


            }
        }
        private double Round(double x)
        {
            return Math.Round(x, 1);
        }
        private void AddLevelLine(double Z)
        {
            List<Point> LevelLine = new List<Point>();
            for(int i = 0; i < size * definitionmultiplier; i++)
            {
                for(int j = 0; j < size * definitionmultiplier; j++)
                {
                    if(Round(AllValues[i,j]) == Z)
                    {
                        Point point = new Point(i / definitionmultiplier, j / definitionmultiplier);
                        if (!LevelLine.Contains(point))
                        {
                            LevelLine.Add(point);
                        }
                    }
                }
            }
            LevelLines.Add(LevelLine);
        }
        private void DrawAllLines()
        {
            foreach(var LevelLine in LevelLines)
            {
                foreach(var point in LevelLine)
                {
                    bitmap.SetPixel(point.X, point.Y, Color.Green);
                }
                /*using(Graphics g = Graphics.FromImage(bitmap))
                {
                    /*Point[] Line = new Point[LevelLine.Count];
                    if(LevelLine.Count == 0)
                    {
                        continue;
                    }
                    for(int i = 0; i < LevelLine.Count; i++)
                    {
                        Line[0] = LevelLine[0];
                    }
                    g.DrawCurve(new Pen(Brushes.Green), Line);
                    LevelLine.
                    for(int i = 1; i < LevelLine.Count; i++)
                    {
                        g.DrawLine(new Pen(Brushes.Green), LevelLine[i - 1], LevelLine[i]);
                    }
                }*/
            }
            Graphic.Image = bitmap;
        }
        public void DrawTriangle(List<Tuple<double, double>> points1)
        {
            LineFlag = false;
            if (points1.Count <= 1)
            {
                return;
            }
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                for (int i = 0; i < points1.Count - 1; i++)
                {
                    Point first = new Point(Convert.ToInt32(Math.Round(points1[i].Item1 * Convert.ToInt32(deltax))) + modifx, size - Convert.ToInt32(Math.Round(points1[i].Item2 * Convert.ToInt32(deltay))) - modify - 1 - 2 * Convert.ToInt32(Convert.ToDouble(CentreYBox.Text) * Convert.ToInt32(DeltaYBox.Text)));
                    Point second = new Point(Convert.ToInt32(Math.Round(points1[i + 1].Item1 * Convert.ToInt32(deltax))) + modifx, size - Convert.ToInt32(Math.Round(points1[i + 1].Item2 * Convert.ToInt32(deltay))) - modify - 1 - 2 * Convert.ToInt32(Convert.ToDouble(CentreYBox.Text) * Convert.ToInt32(DeltaYBox.Text)));
                    g.DrawLine(new Pen(Brushes.Red, 2), first, second);
                }
                Point first1 = new Point(Convert.ToInt32(Math.Round(points1[points1.Count - 1].Item1 * Convert.ToInt32(deltax))) + modifx, size - Convert.ToInt32(Math.Round(points1[points1.Count - 1].Item2 * Convert.ToInt32(deltay))) - modify - 1 - 2 * Convert.ToInt32(Convert.ToDouble(CentreYBox.Text) * Convert.ToInt32(DeltaYBox.Text)));
                Point second1 = new Point(Convert.ToInt32(Math.Round(points1[0].Item1 * Convert.ToInt32(deltax))) + modifx, size - Convert.ToInt32(Math.Round(points1[0].Item2 * Convert.ToInt32(deltay))) - modify - 1 - 2 * Convert.ToInt32(Convert.ToDouble(CentreYBox.Text) * Convert.ToInt32(DeltaYBox.Text)));
                g.DrawLine(new Pen(Brushes.Red, 2), first1, second1);
            }
            Graphic.Image = bitmap;
        }
        private void DrawLines()
        {
            double delta = (maximum - minimum) / Convert.ToDouble(LineCountBox.Text);
            int count = Convert.ToInt32(LineCountBox.Text) + 1;
            List<double> Z = new List<double>();
            for(int i = 0; i < Math.Min(points.Count * 2, 3);i+= 2 )
            {
                var point = points[i];
                Z.Add(Round(F(point.Item1, point.Item2)));
            }
            for(int i = 1; i < count; i++)
            {
                var temp = Round(minimum + i * delta);
                if (!Z.Contains(temp))
                {
                    Z.Add(temp);
                }
                
            }
            if(points.Count > 2)
            {
                Z.Add(Round(F(points[points.Count - 1].Item1, points[points.Count - 1].Item2)));
            }
            
            Z.Add(0);
            //Z.Add(0.1);
            Z.Add(1.0);
            //Z.Add(2.0);
            Z.Add(3.0);
            //Z.Add(5.0);
            Z.Add(10.0);
            foreach(var z in Z)
            {
                //AddLevelLine(z);
            }
            //DrawAllLines();
            //Point[] line = new Point[4];
            //line[0] = new Point()
            //int counter = 0;
            for(int i = 0; i < size * definitionmultiplier; i++)
            {
                for(int j = 0; j < size * definitionmultiplier; j++)
                {
                    /*if(Math.Round(AllValues[i,j], 4) == 0)
                    {
                        if(!line.Contains(new Point(i / definitionmultiplier, j / definitionmultiplier)))
                        {
                            line[counter] = new Point(i / definitionmultiplier, j / definitionmultiplier);
                            counter++;
                        }
                        
                    }*/
                    if(Z.Contains(Round(AllValues[i, j])))
                    {
                        bitmap.SetPixel(i / definitionmultiplier, j / definitionmultiplier, Color.Blue);
                    }
                }
            }
            try
            {
                if (LineFlag)
                {
                    DrawLomanaya();
                }
                
            }
            catch
            {

            }
            Graphic.Image = bitmap;
        }
        private double F(double x, double y)
        {
            return (x * x + y - 11) * (x * x + y - 11) + (x + y * y - 7) * (x + y * y - 7);//Himmelblau
            //return (1 - x) * (1 - x) + 100 * (y - x * x) * (y - x * x);//Rosenbrok
            //return 1 * x * x + 3 * y * y  - 2 * x * y +1 * x -4 * y - 4;
            //return x * x + 2 * y * y  - 2 * x + 1 * y;//Test form
            //return x * x + 2 * y * y - 4 * x + 2 * y;
            //return x * x + y * y + x * y + x + y;
            //return x * x + 10 * y * y;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private static Bitmap Resize(Bitmap sourceBMP, int width, int height)
        {
            Bitmap result = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(result))
                g.DrawImage(sourceBMP, 0, 0, width, height);
            return result;
        }
        
        private void DrawLomanaya()
        {
            if(points.Count <= 1)
            {
                return;
            }
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                for(int i = 0; i < points.Count - 1; i++)
                {
                    Point first = new Point(Convert.ToInt32(Math.Round(points[i].Item1 * Convert.ToInt32(deltax))) + modifx, size - Convert.ToInt32(Math.Round(points[i].Item2 * Convert.ToInt32(deltay)))  - modify - 1 - 2* Convert.ToInt32(Convert.ToDouble(CentreYBox.Text) * Convert.ToInt32(DeltaYBox.Text)));
                    Point second = new Point(Convert.ToInt32(Math.Round(points[i + 1].Item1 * Convert.ToInt32(deltax))) + modifx, size - Convert.ToInt32(Math.Round(points[i + 1].Item2 * Convert.ToInt32(deltay))) - modify - 1 - 2 * Convert.ToInt32(Convert.ToDouble(CentreYBox.Text) * Convert.ToInt32(DeltaYBox.Text)));
                    g.DrawLine(new Pen(Brushes.Red, 2), first, second);
                }
            }
            Graphic.Image = bitmap;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
        private void CalcModificators()
        {
            modifx = size / 2 + Convert.ToInt32(Convert.ToDouble(CentreXBox.Text) * Convert.ToInt32(DeltaXBox.Text));
            modify = size / 2 - Convert.ToInt32(Convert.ToDouble(CentreYBox.Text) * Convert.ToInt32(DeltaYBox.Text));
            deltax = Convert.ToDouble(DeltaXBox.Text);
            deltay = Convert.ToDouble(DeltaYBox.Text);
        }
        private void DrawGrid()
        {
            for (int i = 0; i < size; i++)
            {
                bitmap.SetPixel(i, modify, Color.Black);
                bitmap.SetPixel(modifx, i, Color.Black);
            }
            try
            {
                int dx = Convert.ToInt32(deltax);
                int dy = Convert.ToInt32(deltay);
                if(dx % 2 == 0 && dy % 2 == 0)
                {
                    dx = dx / 2;
                    dy = dy / 2;
                }
                for (int i = 0; i < size; i += dx)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bitmap.SetPixel(i, modify + j, Color.Black);
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        bitmap.SetPixel(i, modify - j, Color.Black);
                    }
                }
                for (int i = 0; i < size; i += dy)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bitmap.SetPixel(modifx + j, size - i - 1, Color.Black);
                    }
                    for (int j = 0; j < 4; j++)
                    {
                        bitmap.SetPixel(modifx - j, size - i - 1, Color.Black);
                    }
                }
            }
            catch
            {

            }
            finally
            {
                Graphic.Image = bitmap;
                
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (GridFlag)
            {
                for(int i = 0; i < size; i++)
                {
                    for(int j = 0; j < size; j += Convert.ToInt32(deltax))
                    {
                        bitmap.SetPixel(i, size - j - 1, Color.DeepSkyBlue);
                    }
                }
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j += Convert.ToInt32(deltay))
                    {
                        bitmap.SetPixel(j, i, Color.DeepSkyBlue);
                    }
                }
                Graphic.Image = bitmap;
                
            }
            
            
                CalcModificators();
            
            DrawGrid();
            GridFlag = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DoCalculations();
            DrawLines();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(size, size);
            
            AllValues = new double[size * definitionmultiplier, size * definitionmultiplier];
            GridFlag = false;
            Graphic.Image = bitmap;
            CalcModificators();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            definitionmultiplier = trackBar2.Value + 1;
            bitmap = new Bitmap(size, size);
            AllValues = new double[size * definitionmultiplier, size * definitionmultiplier];
            GridFlag = false;
            Graphic.Image = bitmap;
            CalcModificators();
            DoCalculations();
            DrawLines();
            DrawGrid();
        }

        private void AddPoint_Click(object sender, EventArgs e)
        {
            points.Add(new Tuple<double, double>(Convert.ToDouble(PointX.Text), Convert.ToDouble(PointY.Text)));
            DrawLomanaya();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            var x = Convert.ToDouble(PointX.Text);
            var y = Convert.ToDouble(PointY.Text);
            points = KvasiNeutonMethods.ParseProcess(x, y);
            //points = ArmihoGoldshteinWolfRules.ArmihoMovedown(x, y);
            DrawLomanaya();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Graphic.Image.Save(saveFileDialog1.FileName);
            }
        }

        private void Graphic_MouseClick(object sender, MouseEventArgs e)
        {
            // Calculate the width and height of the portion of the image we want

            // to show in the picZoom picturebox. This value changes when the zoom

            // factor is changed.
            int _ZoomFactor = Zoombar.Value;
            int zoomWidth = picZoom.Width / _ZoomFactor;
            int zoomHeight = picZoom.Height / _ZoomFactor;

            // Calculate the horizontal and vertical midpoints for the crosshair

            // cursor and correct centering of the new image

            int halfWidth = zoomWidth / 2;
            int halfHeight = zoomHeight / 2;

            // Create a new temporary bitmap to fit inside the picZoom picturebox

            Bitmap tempBitmap = new Bitmap(zoomWidth, zoomHeight,
                                           PixelFormat.Format24bppRgb);

            // Create a temporary Graphics object to work on the bitmap

            Graphics bmGraphics = Graphics.FromImage(tempBitmap);

            // Clear the bitmap with the selected backcolor

            bmGraphics.Clear(Color.White);

            // Set the interpolation mode

            bmGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Draw the portion of the main image onto the bitmap

            // The target rectangle is already known now.

            // Here the mouse position of the cursor on the main image is used to

            // cut out a portion of the main image.

            bmGraphics.DrawImage(Graphic.Image,
                                 new Rectangle(0, 0, zoomWidth, zoomHeight),
                                 new Rectangle(e.X - halfWidth, e.Y - halfHeight,
                                 zoomWidth, zoomHeight), GraphicsUnit.Pixel);

            // Draw the bitmap on the picZoom picturebox

            picZoom.Image = tempBitmap;

            // Draw a crosshair on the bitmap to simulate the cursor position

            /*bmGraphics.DrawLine(Pens.Black, halfWidth + 1,
                                halfHeight - 4, halfWidth + 1, halfHeight - 1);
            bmGraphics.DrawLine(Pens.Black, halfWidth + 1, halfHeight + 6,
                                halfWidth + 1, halfHeight + 3);
            bmGraphics.DrawLine(Pens.Black, halfWidth - 4, halfHeight + 1,
                                halfWidth - 1, halfHeight + 1);
            bmGraphics.DrawLine(Pens.Black, halfWidth + 6, halfHeight + 1,
                                halfWidth + 3, halfHeight + 1);*/

            // Dispose of the Graphics object

            bmGraphics.Dispose();

            // Refresh the picZoom picturebox to reflect the changes

            picZoom.Refresh();
        }

        private void picZoom_Click(object sender, EventArgs e)
        {

        }
    }
}
