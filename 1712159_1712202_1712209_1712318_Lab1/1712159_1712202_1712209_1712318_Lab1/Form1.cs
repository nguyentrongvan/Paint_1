using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using SharpGL;

namespace _1712159_1712202_1712209_1712318_Lab1
{
    public partial class Form1 : Form
    {
        Stack<shape> st_shape = new Stack<shape>(); // TAO STACK CHUA CAC HINH
        Color colorUserColor_Line, colorFill;
        int shShape;
        bool mouseDown, mouseUp, fill = false, click = false;
        Point pFill;
        public Form1()
        {
            InitializeComponent();
            colorUserColor_Line = Color.Black;
            shShape = 0;
        }

        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
            // Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;
            // Set the clear color.
            gl.ClearColor(255, 255, 255, 255);
            // Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();
        }

        private void openGLControl_Resized(object sender, EventArgs e)
        {
            // Get the OpenGL object.
            OpenGL gl = openGLControl.OpenGL;
            // Set the projection matrix.
            gl.MatrixMode(OpenGL.GL_PROJECTION);
            // Load the identity.
            gl.LoadIdentity();
            // Create a perspective transformation.
            gl.Viewport(0, 0, openGLControl.Width, openGLControl.Height);
            gl.Ortho2D(0, openGLControl.Width, 0, openGLControl.Height);
        }

        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs args)
        {

            OpenGL gl = openGLControl.OpenGL;

            //set color
            gl.Color(colorUserColor_Line.R / 255.0, colorUserColor_Line.G / 255.0, colorUserColor_Line.B / 255.0, 0);
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT); //| OpenGL.GL_DEPTH_BUFFER_BIT);// innit
         //   gl.LineWidth(1f);

            if (st_shape.Count != 0)
            {
                foreach (shape i in st_shape)
                {       // VE TUNG HINH TRONG STACK
                    switch (i.Get_shape())
                    {
                        case 0:
                            draw_line(i.Get_start(), i.Get_end(),i.Get_color());
                            break;
                        case 1:
                            draw_circle(i.Get_start(), i.Get_end(),i.Get_color());
                            break;
                        case 2:
                            draw_rec(i.Get_start(), i.Get_end(),i.Get_color());
                            break;
                        case 3:
                            draw_esllipe(i.Get_start(), i.Get_end(),i.Get_color());
                            break;
                        case 4:
                            draw_tri(i.Get_start(), i.Get_end(),i.Get_color());
                            break;
                        case 5:
                            draw_penta(i.Get_start(), i.Get_end(),i.Get_color());
                            break;
                        case 6:
                            draw_hexa(i.Get_start(), i.Get_end(),i.Get_color());
                            break;
                        case 7:
                            draw_poly(i,i.Get_color());
                            break;
                    }
                }
            }
          
            gl.Flush();
            if (fill == true && !pFill.IsEmpty) 
            {
                byte[] oldColor;
                oldColor = new byte[4];

                GetColor(pFill.X, pFill.Y, out oldColor);
                Color color_fill;
                color_fill = colorFill;

                Color color_B = new Color();
                color_B = Color.FromArgb(oldColor[3], oldColor[0], oldColor[1], oldColor[2]);
                FloodFill(pFill.X, pFill.Y, color_fill, color_B);
            }
        }

        //draw shape 
        private void draw_line(Point pStart, Point pEnd, Color c)
        {
            OpenGL gl = openGLControl.OpenGL;
            gl.Color(c.R / 255.0, c.G / 255.0, c.B / 255.0);
            gl.Begin(OpenGL.GL_LINES);
            gl.Vertex(pStart.X, gl.RenderContextProvider.Height - pStart.Y);
            gl.Vertex(pEnd.X, gl.RenderContextProvider.Height - pEnd.Y);
            gl.End();
            gl.Flush();
        }
        private void draw_circle(Point pStart, Point pEnd, Color c)
        {
            double D = Math.Sqrt(Math.Pow(pStart.X - pEnd.X, 2) + Math.Pow(pStart.Y - pEnd.Y, 2));
            float R = (float)D / 2;

            OpenGL gl = openGLControl.OpenGL;
            gl.Color(c.R / 255.0, c.G / 255.0, c.B / 255.0);
            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int i = 0; i < 360; i++)
            {
                gl.Vertex(D * Math.Cos(2 * Math.PI * i / 360) + pStart.X, D * Math.Sin(2 * Math.PI * i / 360) + gl.RenderContextProvider.Height - pStart.Y);
            }
            gl.End();
            gl.Flush();
        }
        private void draw_rec(Point topLeft, Point bottomRight, Color c)
        {
            Point[] p = new Point[4];
            p[0] = topLeft;
            p[2] = bottomRight;
            p[1].X = topLeft.X;
            p[1].Y = bottomRight.Y;
            p[3].X = bottomRight.X;
            p[3].Y = topLeft.Y;

            OpenGL gl = openGLControl.OpenGL;
            gl.Color(c.R / 255.0, c.G / 255.0, c.B / 255.0);

            gl.Begin(OpenGL.GL_LINE_LOOP);
           for(int i=0;i<4;i++)
            {
                gl.Vertex(p[i].X, gl.RenderContextProvider.Height - p[i].Y);
            }
            gl.End();
            gl.Flush();
        }
        private void draw_esllipe(Point pStart, Point pEnd, Color c)
        {
            Point center = new Point();
            center.X = (pStart.X + pEnd.X) / 2;
            center.Y = (pStart.Y + pEnd.Y) / 2;
            int rX = Math.Abs(pStart.X - pEnd.X);
            int rY = Math.Abs(pStart.Y - pEnd.Y);

            OpenGL gl = openGLControl.OpenGL;
            gl.Color(c.R / 255.0, c.G / 255.0, c.B / 255.0);

            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int i = 0; i < 360; i++)
            {
                gl.Vertex(rX * Math.Cos(2 * Math.PI * i / 360) + pStart.X, rY * Math.Sin(2 * Math.PI * i / 360) + gl.RenderContextProvider.Height - pStart.Y, 0.0);
            }
            gl.End();
            gl.Flush();
        }
        private void draw_tri(Point pStart, Point pEnd, Color c)
        {
            OpenGL gl = openGLControl.OpenGL;
            Point p1 = pStart;
            Point p2 = pEnd;
            Point p3 = new Point();

            p1.X = pStart.X;
            p1.Y = pEnd.Y;

            double D = Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
            double high = D * Math.Sin(Math.PI / 3);
            p3.X = (p1.X + p2.X) / 2;
            p3.Y = p1.Y - (int)high;

            draw_line(p1, p2,c);
            draw_line(p2, p3,c);
            draw_line(p3, p1,c);
        }
        private void draw_penta(Point pStart, Point pEnd, Color c)
        {
            double D = Math.Sqrt(Math.Pow(pStart.X - pEnd.X, 2) + Math.Pow(pStart.Y - pEnd.Y, 2));
            float R = (float)D / 2;
            Point[] p = new Point[5];
            for (int i = 0; 72 *i < 360; i++)
            {
                p[i].X = (int)(D * Math.Cos(2 * Math.PI * 72 * i / 360) + pStart.X);
                p[i].Y = (int)(D * Math.Sin(2 * Math.PI * 72 * i / 360) + pStart.Y);
            }

            OpenGL gl = openGLControl.OpenGL;
            gl.Color(c.R / 255.0, c.G / 255.0, c.B / 255.0);

            gl.Begin(OpenGL.GL_LINE_LOOP);
            for(int i=0;i<5;i++)
            {
                gl.Vertex(p[i].X, gl.RenderContextProvider.Height - p[i].Y);
            }
            gl.End();
            gl.Flush();
        }
        private void draw_hexa(Point pStart, Point pEnd, Color c)
        {
            double D = Math.Sqrt(Math.Pow(pStart.X - pEnd.X, 2) + Math.Pow(pStart.Y - pEnd.Y, 2));
            float R = (float)D / 2;
            Point[] p = new Point[6];

            for (int i = 0; 60 * i < 360; i++)
            {
                p[i].X = (int)(D * Math.Cos(2 * Math.PI * 60 * i / 360) + pStart.X);
                p[i].Y = (int)(D * Math.Sin(2 * Math.PI * 60 * i / 360) + pStart.Y);
            }

            OpenGL gl = openGLControl.OpenGL;
            gl.Color(c.R / 255.0, c.G / 255.0, c.B / 255.0);

            gl.Begin(OpenGL.GL_LINE_LOOP);
            for (int i = 0; i < 6; i++)
            {
                gl.Vertex(p[i].X, gl.RenderContextProvider.Height - p[i].Y);
            }
            gl.End();
            gl.Flush();
        }
        private void draw_poly(shape s, Color c)
        {
            if (s.points.Count == 2)
            {  // trường hợp đa giác mới chỉ là 1 đoạn thẳng
                draw_line(s.points[0], s.points[1],c);
            }
            else if (s.points.Count > 2)
            {      // đa giác lớn hơn 3 đỉnh
                for (int i = 0; i < s.points.Count - 1; i++)
                    draw_line(s.points[i], s.points[i + 1],c);
                draw_line(s.points[0], s.points[s.points.Count - 1],c); // để nối từ điểm đầu đến điểm cuối
            }
        }

        private void bt_LineWidth_Click(object sender, EventArgs e)
        {

        }

        private void back(object sender, EventArgs e)
        {
            if (st_shape.Count() == 0)
                return;
            st_shape.Pop();
            OpenGL gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        }       // HAM DE XOA 1 HINH VE TRUOC DO, DO K TAO DC BUTTON NEN T DUNG TAM BUTTON FILL
        private void bt_line_Click(object sender, EventArgs e)
        {
            shShape = 0;
            if (fill == true)
                fill = false;
        }

        private void bt_circle_Click(object sender, EventArgs e)
        {
            shShape = 1;
            if (fill == true)
                fill = false;
        }

        private void bt_rectangle_Click(object sender, EventArgs e)
        {
            shShape = 2;
            if (fill == true)
                fill = false;
        }

        private void bt_ellipse_Click(object sender, EventArgs e)
        {
            shShape = 3;
            if (fill == true)
                fill = false;
        }


        private void bt_triangle_Click(object sender, EventArgs e)
        {
            shShape = 4;
            if (fill == true)
                fill = false;
        }

        private void bt_pentagon_Click(object sender, EventArgs e)
        {
            shShape = 5;
            if (fill == true)
                fill = false;
        }

        private void bt_hexagon_Click(object sender, EventArgs e)
        {
            shShape = 6;
            if (fill == true)
                fill = false;
        }

        private void bt_polygon_Click(object sender, EventArgs e)
        {
            shShape = 7;
            if (fill == true)
                fill = false;
        }

        private void  GetColor(int x, int y, out Byte[]pixelData)
        {
            pixelData = new byte[4*1*1];
            OpenGL gl = openGLControl.OpenGL;
            gl.ReadPixels(x,gl.RenderContextProvider.Height- y, 1, 1, OpenGL.GL_RGBA, OpenGL.GL_UNSIGNED_BYTE, pixelData);
        }

        private void SetColor(int x, int y, Color c)
        {
            OpenGL gl = openGLControl.OpenGL;
            byte[] color = new byte[4];

            color[0] = c.R;
            color[1] = c.G;
            color[2] = c.B;
            color[3] = c.A;

            gl.RasterPos(x, gl.RenderContextProvider.Height-y);
            gl.DrawPixels(1, 1, OpenGL.GL_RGBA, color);
          
            gl.Flush();      
        }

        private bool IsSameColor(byte[] c1, Color c2)
        {
            if (c1[0] == c2.R && c1[1] == c2.G && c1[2] == c2.B && c1[3] == c2.A) 
            {
                return true;
            }
            else
                return false;
        }

        private void _floodFill(Queue <Point>q, int x, int y, Point nn, Color fill)
        {
            SetColor(x, y, fill);
            nn.X = x;
            nn.Y = y;
            q.Enqueue(nn);
        }

        private void FloodFill(int x, int y, Color cfill, Color oldColor)
        {
             byte[] current = new byte[4];
             Queue<Point> Q = new Queue<Point>();
             GetColor(x, y, out current);

             Point nn;
             nn = new Point();

             if (IsSameColor(current,oldColor)) 
             {
                 Point now = new Point(x, y);
                 SetColor(now.X, now.Y, cfill);
                 Q.Enqueue(now);

                 while(Q.Count()!=0)
                 {
                     Q.Dequeue();
                     GetColor(now.X+1, now.Y, out current);
                     if (IsSameColor(current, oldColor))
                     {
                        _floodFill(Q, now.X + 1, now.Y, nn, cfill);
                     }

                     GetColor(now.X - 1, now.Y, out current);
                     if (IsSameColor(current, oldColor))
                     {
                        _floodFill(Q, now.X - 1, now.Y, nn, cfill);
                     }

                     GetColor(now.X, now.Y + 1, out current);
                     if (IsSameColor(current, oldColor))
                     {
                        _floodFill(Q, now.X, now.Y + 1, nn, cfill);
                     }

                     GetColor(now.X, now.Y - 1, out current);
                     if (IsSameColor(current, oldColor))
                     {
                        _floodFill(Q, now.X, now.Y - 1, nn, cfill);
                     }

                    if (Q.Count() != 0)
                        now = Q.First();
                 }
             }
        }

        private void bt_undo_Click(object sender, EventArgs e)
        {
            if (st_shape.Count() == 0)
                return;
            st_shape.Pop();
            OpenGL gl = openGLControl.OpenGL;
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
        }

        private void bt_clear_Click(object sender, EventArgs e)
        {
          while(st_shape.Count()!=0)
            {
                st_shape.Pop();
            }
        }

        private void ctrl_openGLControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown == true && mouseUp == false)
            {
                OpenGL gl = openGLControl.OpenGL;
                gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
                st_shape.Peek().Set_end(e.Location);   // update vi tri ket thuc cua shape
                st_shape.Peek().Set_shape(shShape);     //update shape type
            }
        }

        private void openGLControl_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                st_shape.Peek().Set_end(e.Location); // update lan cuoi vi tri ket thuc cho shape
                mouseUp = true;
                mouseDown = false;
                // them 1 shape vao stack de viec thay doi shape tren cung stack khong con dien ra nua
                if (shShape != 7)
                {
                    st_shape.Push(new shape());
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void bt_Color(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog()==DialogResult.OK)
            {
                if (fill == true)
                {
                    colorFill = colorDialog1.Color;
                }
                else
                {
                    colorUserColor_Line = colorDialog1.Color;
                    st_shape.Peek().Set_color(colorUserColor_Line);
                }
            }
        }

        private void openGLControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (fill == true)
            {
                pFill = e.Location;
            }
        }

        private void bt_fill_Click(object sender, EventArgs e)
        {
            if (fill == true)
                fill = false;
            else
                fill = true;
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (st_shape.Count == 0)
                {      // neu stack rong, khoi tao 1 shape moi them vao
                    st_shape.Push(new shape(new Point(), new Point(), shShape,colorUserColor_Line));
                }
                st_shape.Peek().Set_start(e.Location);
                st_shape.Peek().Set_end(e.Location);
                mouseDown = true;
                mouseUp = false;
                if (shShape == 7)
                {
                    Point te = e.Location;
                    st_shape.Peek().points.Add(te);
                }
            }

            if (e.Button == MouseButtons.Right && shShape == 7)
            {
                st_shape.Push(new shape()); // click chuot phai de huy ve da giac, nen them 1 shape moi vao no
            }
        }


    }
    public class shape:Form1
    {
        public Point pStart, pEnd;
        public List<Point> points;
        public int shape_type;
        Color color;
        public shape()
        {
            pStart = new Point(0, 0);
            pEnd = new Point(0, 0);
            shape_type = 0;
            points = new List<Point>();
            color = Color.Black;
        }
        public shape(Point a, Point b, int t, List<Point> c,Color color)
        {
            pStart = a;
            pEnd = b;
            shape_type = t;
            points = c;
            color = Color.Black;
        }
        public shape(Point a, Point b, int t,Color c)
        {
            pStart = a;
            pEnd = b;
            shape_type = t;
            points = new List<Point>();
            color = c;
        }
        public Point Get_start()
        {
            return pStart;
        }
        public Point Get_end()
        {
            return pEnd;
        }
        public int Get_shape()
        {
            return shape_type;
        }
        public void Set_start(Point a)
        {
            pStart = a;
        }
        public void Set_end(Point a)
        {
            pEnd = a;
        }
        public void Set_shape(int a)
        {
            shape_type = a;
        }

        public void Set_color(Color c)
        {
            color = c;
        }

        public Color  Get_color()
        {
            return color;
        }
    } //////////////////////////////////////////////////////////////////////////////////
    public class polygon
    {
        public Stack<Point> points;
    }
    ////////////////////////////////////////////////////////////////////////////////////
}
