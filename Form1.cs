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
        Brush b;
        string mode;
        bool isClicked, isFrom;
        Vertex Displacemented, from, to;
        Dictionary<Point, Vertex> vertices = new Dictionary<Point, Vertex>();
        List<Edge> edges = new List<Edge>();
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
            p = new Pen(Color.Red);
            b = new SolidBrush(Color.Red);
            isClicked = false;
            isFrom = true;
            Displacemented = null;
        }

        public void draw()
        {
            g.Clear(Color.White);
            foreach (Vertex v in vertices.Values)
            {
                
                g.FillEllipse(b, v.Coords.X - 25, v.Coords.Y - 25, 50, 50);
                pictureBox1.Image = bmp;
            }
            foreach(var e in edges)
            {
                Point p1 = new Point(e.From.Coords.X, e.From.Coords.Y);
                Point p2 = new Point(e.To.Coords.X, e.To.Coords.Y);
                g.DrawLine(p, p1, p2);
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
                        label3.Text = CheckAround(e.X, e.Y, v.Value.Coords.X, v.Value.Coords.Y, 25).ToString();
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

        private void button2_Click(object sender, EventArgs e)
        {
            mode = "Displacement";
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = $"X: {e.X}; Y: {e.Y}";
            label2.Text = mode;
            label4.Text = isFrom.ToString();
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
