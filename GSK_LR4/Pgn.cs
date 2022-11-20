using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSK_LR4
{
    public class Pgn
    {
        //Список точек фигуры
        List<PointF> VertexList;

        /* 
         * Конструктор
         */
        public Pgn()
        {
            VertexList = new List<PointF>();
        }

        /*
         * Метод добавления вершины
         */
        public void Add(Point NewVertex)
        {
            VertexList.Add(NewVertex);
        }

        /*
         * Метод вывода закрашенного многоугольника с помощью FillPolygon
         */
        public void Fill(Graphics graphics, Pen DrawPen)
        {
            Brush DrawBrush = new SolidBrush(DrawPen.Color);
            Point[] PgVertex = new Point[VertexList.Count];
            for(int i = 0; i < VertexList.Count; i++)
            {
                PgVertex[i].X = (int)Math.Round(VertexList[i].X);
                PgVertex[i].Y = (int)Math.Round(VertexList[i].Y);
            }
            graphics.FillPolygon(DrawBrush, PgVertex);
        }

        /*
         * Метод выделения многоугольника
         */
        public bool ThisPgn(int mX, int mY)
        {
            int n = VertexList.Count() - 1, k = 0, m = 0;
            PointF Pi, Pk; double x;
            bool check = false;
            for (int i = 0; i <= n; i++)
            {
                if (i < n) k = i + 1; else k = 0;
                Pi = VertexList[i]; Pk = VertexList[k];
                if ((Pi.Y < mY) & (Pk.Y >= mY) | (Pi.Y >= mY) & (Pk.Y < mY))
                    if ((mY - Pi.Y) * (Pk.X - Pi.X) / (Pk.Y - Pi.Y) + Pi.X < mX) m++;
            }
            if (m % 2 == 1) check = true;
            return check;
        }

        /*
         * Метод плоско-параллельного перемещения
         */
        public void Move(float dx, float dy)
        {
            PointF fP = new PointF();
            for (int i = 0; i < VertexList.Count; i++)
            {
                fP.X = VertexList[i].X + dx;
                fP.Y = VertexList[i].Y + dy;
                VertexList[i] = fP;
            }
        }

        /*
         * Метод вращения
         */
        public void Rotate(float df, float xc, float yc)
        {
            PointF fP = new PointF();
            for(int i = 0; i < VertexList.Count; i++)
            {
                fP.X = VertexList[i].X - xc;
                fP.Y = VertexList[i].Y - yc;
                fP.X = fP.X * (float)Math.Cos(df) - fP.Y * (float)Math.Sin(df);
                fP.Y = fP.Y * (float)Math.Cos(df) + fP.X * (float)Math.Sin(df);
                fP.X += xc;
                fP.Y += yc;
                VertexList[i] = fP;
            }
        }

        /*
         * Метод масштабирования
         */
        public void Resize(float dx, float dy, float xc, float yc)
        {
            PointF fP = new PointF();
            for (int i = 0; i < VertexList.Count; i++)
            {
                fP.X = VertexList[i].X - xc;
                fP.Y = VertexList[i].Y - yc;
                fP.X = fP.X * (1 + dx);
                fP.Y = fP.Y * (1 + dy);
                fP.X += xc;
                fP.Y += yc;
                VertexList[i] = fP;
            }
        }

        /*
         * Отражение по X
         */
        public void MirrorX(float yc)
        {
            PointF fP = new PointF();
            for (int i = 0; i < VertexList.Count; i++)
            {
                fP.X = VertexList[i].X;
                fP.Y = VertexList[i].Y - yc;
                fP.Y = -fP.Y;
                fP.Y += yc;
                VertexList[i] = fP;
            }
        }

        /*
         * Отражение по Y
         */
        public void MirrorY(float xc)
        {
            PointF fP = new PointF();
            for (int i = 0; i < VertexList.Count; i++)
            {
                fP.Y = VertexList[i].Y;
                fP.X = VertexList[i].X - xc;
                fP.X = -fP.X;
                fP.X += xc;
                VertexList[i] = fP;
            }
        }

        /*
         * Отражение по (0,0)
         */
        public void MirrorXY(float xc, float yc)
        {
            PointF fP = new PointF();
            for (int i = 0; i < VertexList.Count; i++)
            {
                fP.X = VertexList[i].X - xc;
                fP.Y = VertexList[i].Y - yc;
                fP.X = -fP.X;
                fP.Y = -fP.Y;
                fP.X += xc;
                fP.Y += yc;
                VertexList[i] = fP;
            }
        }

        /*
         * Метод очистки списка вершин
         */
        public void Clear()
        { 
            VertexList.Clear(); 
        }
    }
}
