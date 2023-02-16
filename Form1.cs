using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NIceOneGraph
{
    public partial class Form1 : Form
    {
        //Переменные
        Graphics g;
        Bitmap bmp;
        Pen p;
        Brush b1, b2;
        Font f;
        string mode;
        bool isClicked, isFrom;
        Vertex Displacemented, from, to;
        Dictionary<Point, Vertex> vertices = new Dictionary<Point, Vertex>();
        List<Edge> edges = new List<Edge>();
        int[,] pathMatrix, shortPathMatrix;
        //Dictionary<Point, int> edges = new Dictionary<Point, int>();
        //
        public Form1()
        {
            InitializeComponent();
            Init();
            g.Clear(Color.White);
            pictureBox1.Image = bmp;
        }

        public void Init()
        {
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(bmp);
            mode = "Displacement";
            p = new Pen(Color.Red, 2);
            b1 = new SolidBrush(Color.Red);
            b2 = new SolidBrush(Color.Black);
            f = new Font("Arial", 14);
            isClicked = false;
            isFrom = true;
            Displacemented = null;
            textBox1.Text = "1";
        }

        public void fillPathMatrix()
        {
            List<Vertex> verticess = vertices.Select(kvp => kvp.Value).ToList();
            int n = vertices.Count;
            pathMatrix = new int[n, n];
            shortPathMatrix = new int[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Vertex iv = verticess[i];
                    Vertex jv = verticess[j];
                    foreach (var e in edges)
                    {
                        if(i == j)
                        {
                            pathMatrix[i, j] = 0;
                            break;
                        }
                        if ((e.From == iv && e.To == jv) || (e.From == jv && e.To == iv))
                        {
                            pathMatrix[i, j] = e.Weight;
                            break;
                        }
                        else if (e == edges.Last())
                        {
                            pathMatrix[i, j] = 100000;
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
            }
            shortPathMatrix = pathMatrix;
            for (int k = 0; k < n; ++k)
            {
                for (int i = 0; i < n; ++i)
                {
                    for (int j = 0; j < n; ++j)
                    {
                        if (shortPathMatrix[i, k] + shortPathMatrix[k, j] < shortPathMatrix[i, j])
                            shortPathMatrix[i, j] = shortPathMatrix[i, k] + shortPathMatrix[k, j];
                    }
                }
            }
            Console.WriteLine(shortPathMatrix.ToString());
        }
        public void draw()
        {
            g.Clear(Color.White);
            foreach (Vertex v in vertices.Values)
            {
                
                g.FillEllipse(b1, v.Coords.X - 25, v.Coords.Y - 25, 50, 50);
                pictureBox1.Image = bmp;
            }
            foreach(var e in edges)
            {
                Point p1 = new Point(e.From.Coords.X, e.From.Coords.Y);
                Point p2 = new Point(e.To.Coords.X, e.To.Coords.Y);
                g.DrawLine(p, p1, p2);
                if(e.Weight != null)
                {
                    Point p = new Point((p1.X + p2.X)/2,( p1.Y + p2.Y)/2);
                    g.DrawString(e.Weight.ToString(), f, b2,p);
                }
            }
        }

        public bool CheckAround(int x, int y, int x0, int y0, int r) 
        {
            return (x <= x0+r) && (x0-r <= x) && (y <= y0+r) && (y0-r <= y);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            mode = "Dots";
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if(mode == "Displacement")
            {
                Point current = new Point(e.X, e.Y);
                
                foreach(var v in vertices)
                {
                    if(CheckAround(current.X, current.Y, v.Value.Coords.X, v.Value.Coords.Y, 25))
                    {
                        label3.Text = CheckAround(current.X, current.Y, v.Value.Coords.X, v.Value.Coords.Y, 25).ToString();
                        isClicked = true;
                        Displacemented = v.Value;
                        isClicked = true;
                        break;
                    }
                }
            }
            if(mode == "Line")
            {
                foreach (var v in vertices)
                {
                    if (CheckAround(e.X, e.Y, v.Value.Coords.X, v.Value.Coords.Y, 25))
                    {
                        isClicked = true;
                        if (isFrom)
                        {
                            from =v.Value;
                            isFrom= false;
                        }
                        else
                        {
                            to = v.Value;
                            edges.Add(new Edge(from, to));
                            from = null; to = null;
                            isFrom= true;
                        }
                        break;
                    }
                }
            }
            if(mode == "Weight")
            {
                foreach(var ed in edges)
                {
                    if ((e.X-ed.From.Coords.X)/(ed.To.Coords.X-ed.From.Coords.X) == (e.Y - ed.From.Coords.Y) / (ed.To.Coords.Y - ed.From.Coords.Y))
                    {
                        ed.addWeight(Convert.ToInt32(textBox1.Text));
                    }
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            draw();
            isClicked =false;
            
            if(mode == "Dots")
            {
                Point coord = new Point(e.X, e.Y);
                
                //g.FillEllipse(b, e.X-25, e.Y-25, 50, 50);
                vertices.Add(coord, new Vertex(coord));
                draw();
                //pictureBox1.Image = bmp;
            }
            if(mode == "Line")
            {
                isFrom = false;
                if(from == null) { isFrom= true; }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            fillPathMatrix();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            mode = "Weight";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            mode = "Displacement";
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = $"X: {e.X}; Y: {e.Y}";
            if(mode == "Displacement" && isClicked)
            {
                Displacemented.Coords.X = e.X;
                Displacemented.Coords.Y = e.Y;
                draw();
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mode = "Line";
        }
    }
}
