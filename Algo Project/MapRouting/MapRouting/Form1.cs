using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapRouting
{
    public partial class Form1 : Form
    {
        private Graphics gfx;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
             var watch2 = System.Diagnostics.Stopwatch.StartNew();
            MapRouting f = new MapRouting();
            f.SourceDestinationFinder();
            watch2.Stop();
            string w2 = watch2.ElapsedMilliseconds.ToString()+" ms";
            f.lines.Add(w2);
            System.IO.File.WriteAllLines(@"C:\Users\Salma Emad\Desktop\be el ui\MapRouting\MapRouting\output2.txt", f.lines);
            System.Drawing.Graphics grphobj;
            grphobj = this.CreateGraphics();
            Pen mypen = new Pen(System.Drawing.Color.HotPink, 1);

            //f.FillMap();
            //MessageBox.Show(f.vertices.ToString());
            for (int i = 0; i < f.vertices; i++)
            {
                float x;
                float y;
                x = (float)f.antimap[i].Item1;
                y = (float)f.antimap[i].Item2;
                x *= 10;
                y *= 10;


                grphobj.DrawPie(mypen, x, y, 30, 30, 0, 360);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel1.Hide();
            gfx = this.CreateGraphics();
            /*Rectangle rect = new Rectangle();
            rect.X = 50;
            rect.Y=50;
            rect.Width=10;
            rect.Height=15;*/
            Image myImg = Image.FromFile(@"C:\Users\Salma Emad\Desktop\loc.jpg");
            //gfx.DrawImageUnscaled(myImg, 50, 50, 5, 4);
            //gfx.DrawImageUnscaledAndClipped(myImg, rect);
            //x.DrawImage(myImg,new Point( 50, 50));
            MapRouting f = new MapRouting();
            //f.SourceDestinationFinder();
            System.Drawing.Graphics grphobj;
            grphobj = this.CreateGraphics();
            Pen mypen = new Pen(System.Drawing.Color.Blue, 2);

            f.FillMap();
            gfx = this.CreateGraphics();
            //MessageBox.Show(f.vertices.ToString());
            float x;
            float y;
            for (int i = 0; i < f.vertices; i++)
            {
               
                x =(float) f.antimap[i].Item1;
                y = (float)f.antimap[i].Item2;
               /* Rectangle rect = new Rectangle();
                 rect.X = (int)x;
            rect.Y=(int)y;
            rect.Width=10;
            rect.Height=15;
                * */
                //Image myImg = Image.FromFile(@"C:\Users\Gamela\Pictures\My Screen Shots\Favorites\images.jpg");
                gfx.DrawImageUnscaled(myImg,(int)x,(int)y);

            }
            //MessageBox.Show(f.AdjList.Count().ToString());
            for (int i = 0; i < f.vertices-1; i++)
            {

                float x1 = (float)f.antimap[i].Item1;
                float y1 = (float)f.antimap[i].Item2;
                
                for (int j = 0; j < f.AdjList[i].Count; j++)
                {
                    int index;
                   index = f.AdjList[i][j].Item2;
                    float x2=(float) f.antimap[index].Item1;
                    float y2= (float)f.antimap[index].Item2;
                   
                    grphobj.DrawLine(mypen, (int)x1,(int) y1,(int) x2,(int) y2);
                }
            }
            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }
    }
}