using System.Drawing;

namespace GSK_LR4
{
    public partial class Form1 : Form
    {
        Bitmap Bitmap;
        Graphics Graphics;
        Pen DrawPen = new (Color.Black, 1);
        List<Point> VertexList = new List<Point>(); //для отрисовки сторон
        List<Pgn> pgns = new List<Pgn>(); //Список фигур
        List<Transformation> TransformationList = new List<Transformation>(); //Список текущих преобразований
        Pgn NewPgn = new Pgn(); //Новая фигура
        int SelectedIndex; //Индекс выбранной фигуры
        int Operation = 1; // 1 - рисование, 2 - выделение
        int Transformation = 0; //Выбор геометрического преобразования
        int IterationCount = 50; //Число проводимых итераций
        float XScale = 1, YScale = 1; //Коэффициенты увеличения
        bool CheckPgn = false;
        Point PictureBoxPosition = new Point();

        /*
         * Конструктор класса, содержащий инициализацию полотна размером 1920x1080
         */
        public Form1()
        {
            InitializeComponent();
            Bitmap = new Bitmap(1920, 1080);
            Graphics = Graphics.FromImage(Bitmap);
            timer1.Interval = 10;
        }

        private void InputPgn(MouseEventArgs e) 
        {
            if(e.Button == MouseButtons.Left)
            {
                Point NewP = new Point() { X = e.X, Y = e.Y };
                VertexList.Add(NewP);
                NewPgn.Add(NewP);
                int k = VertexList.Count;
                if (k > 1) Graphics.DrawLine(DrawPen, VertexList[k - 2], VertexList[k - 1]);
                else Graphics.DrawRectangle(DrawPen, e.X, e.Y, 1, 1);
            }
            else if(e.Button == MouseButtons.Right) //Конец ввода
            {
                int k = VertexList.Count;
                if(k < 3) //Проверка, что заданная фигура является хотя бы треугольником
                {
                    MessageBox.Show("Вы ввели недостаточно точек для задания многоугольника");
                    return;
                }
                Graphics.DrawLine(DrawPen, VertexList[k - 1], VertexList[0]);
                VertexList.Clear();
                NewPgn.Fill(Graphics, DrawPen);
                pgns.Add(NewPgn);
                NewPgn = new Pgn();
            }
        }

        /*
         * Метод обработчика события нажатия на pictureBox мышью
         */
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (Operation)
            {
                case 1: //Рисование
                    {
                        InputPgn(e);
                        //if (e.Button == MouseButtons.Right) Operation = 0;
                        break;
                    }
                case 2:
                    {
                        if (CheckPgn)
                        {
                            //Выбор между пользвательсикми и запрограммированными трансформациями
                            switch(Transformation)
                            {
                                case 2:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], Transformation, IterationCount, PictureBoxPosition, e.Location, XScale, YScale));
                                    break;
                                case 3:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], 2, IterationCount, PictureBoxPosition, e.Location, 2, 2));
                                    break;
                                case 4:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], 2, IterationCount, PictureBoxPosition, e.Location, 0.5F, 0.5F));
                                    break;
                                case 5:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], 1, IterationCount, PictureBoxPosition, new Point(PictureBoxPosition.X, PictureBoxPosition.Y - 1)));
                                    break;
                                case 6:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], 1, IterationCount, PictureBoxPosition, new Point(PictureBoxPosition.X, PictureBoxPosition.Y + 1)));
                                    break;
                                case 7:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], 0, IterationCount, PictureBoxPosition, new Point(PictureBoxPosition.X, PictureBoxPosition.Y + 100)));
                                    break;
                                case 8:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], 0, IterationCount, PictureBoxPosition, new Point(PictureBoxPosition.X + 100, PictureBoxPosition.Y)));
                                    break;
                                case 9:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], 3, IterationCount, PictureBoxPosition));
                                    break;
                                case 10:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], 4, IterationCount, PictureBoxPosition));
                                    break;
                                case 11:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], 5, IterationCount, PictureBoxPosition));
                                    break;
                                default:
                                    TransformationList.Add(new Transformation(pgns[SelectedIndex], Transformation, IterationCount, PictureBoxPosition, e.Location));
                                    break;
                            }
                            if(TransformationList.Count == 1)
                            {
                                timer1.Start();
                            }
                            CheckPgn = false;
                        }
                        else
                        {
                            //Определение, находится ли в текущей позиции хотя бы одна фигура
                            for (int i = 0; i < pgns.Count; i++)
                            {
                                if (pgns[i].ThisPgn(e.X, e.Y))
                                {
                                    PictureBoxPosition = e.Location;
                                    SelectedIndex = i;
                                    Graphics.DrawEllipse(new Pen(Color.Cyan), e.X - 2, e.Y - 2, 5, 5);
                                    CheckPgn = true;
                                    break;
                                }
                                else CheckPgn = false;
                            }
                        }
                        break;
                    }
            }
            pictureBox1.Image = Bitmap;
        }

        /*
         * Метод обработчика события изменения цвета
         */
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex) // выбор цвета
            {
                case 0:
                    DrawPen.Color = Color.Black;
                    break;
                case 1:
                    DrawPen.Color = Color.Red;
                    break;
                case 2:
                    DrawPen.Color = Color.Green;
                    break;
                case 3:
                    DrawPen.Color = Color.Blue;
                    break;
            }
        }

        /*
         * Метод обработчика события изменения режима работы
         */
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Operation = comboBox3.SelectedIndex + 1;
        }

        /*
         * Метод обработчика события очистки полотна
         */
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = Bitmap;
            Graphics.Clear(pictureBox1.BackColor);
            NewPgn.Clear();
            VertexList.Clear();
            pgns.Clear();
            TransformationList.Clear();
            comboBox3.SelectedIndex = 0;
            Operation = 1;
        }

        /*
         * Обработчик изменения интервала анимации
         */
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = trackBar1.Value;
            label5.Text = trackBar1.Value.ToString();
        }

        /*
         * Обработчик выбора трансформации
         */
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Transformation = comboBox1.SelectedIndex;
        }

        /*
         * Метод таймера, в котором происходит перерисовка полотна
         */
        private void timer1_Tick(object sender, EventArgs e)
        {
            if(TransformationList.Count == 0)
            {
                timer1.Stop();
            }
            foreach(Transformation transformation in TransformationList)
            {
                transformation.DoTransformation();
                if (transformation.ShouldRemove())
                {
                    TransformationList.Remove(transformation);
                    break;
                }
            }
            Graphics.Clear(pictureBox1.BackColor);
            foreach (Pgn pgn in pgns)
            {
                pgn.Fill(Graphics, DrawPen);
            }
            pictureBox1.Image = Bitmap;
        }

        /*
         * Обработчик изменения вертикального увеличения
         */
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            label10.Text = (trackBar3.Value / 10F).ToString();
            YScale = trackBar3.Value / 10F;
        }

        /*
         * Обработчик изменения горизонтального увеличения
         */
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            label8.Text = (trackBar2.Value / 10F).ToString();
            XScale = trackBar2.Value / 10F;
        }
    }
}