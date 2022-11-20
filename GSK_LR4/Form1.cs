using System.Drawing;

namespace GSK_LR4
{
    public partial class Form1 : Form
    {
        Bitmap Bitmap;
        Graphics Graphics;
        Pen DrawPen = new (Color.Black, 1);
        List<Point> VertexList = new List<Point>(); //��� ��������� ������
        List<Pgn> pgns = new List<Pgn>(); //������ �����
        List<Transformation> TransformationList = new List<Transformation>(); //������ ������� ��������������
        Pgn NewPgn = new Pgn(); //����� ������
        int SelectedIndex; //������ ��������� ������
        int Operation = 1; // 1 - ���������, 2 - ���������
        int Transformation = 0; //����� ��������������� ��������������
        int IterationCount = 50; //����� ���������� ��������
        float XScale = 1, YScale = 1; //������������ ����������
        bool CheckPgn = false;
        Point PictureBoxPosition = new Point();

        /*
         * ����������� ������, ���������� ������������� ������� �������� 1920x1080
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
            else if(e.Button == MouseButtons.Right) //����� �����
            {
                int k = VertexList.Count;
                if(k < 3) //��������, ��� �������� ������ �������� ���� �� �������������
                {
                    MessageBox.Show("�� ����� ������������ ����� ��� ������� ��������������");
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
         * ����� ����������� ������� ������� �� pictureBox �����
         */
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (Operation)
            {
                case 1: //���������
                    {
                        InputPgn(e);
                        //if (e.Button == MouseButtons.Right) Operation = 0;
                        break;
                    }
                case 2:
                    {
                        if (CheckPgn)
                        {
                            //����� ����� ���������������� � �������������������� ���������������
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
                            //�����������, ��������� �� � ������� ������� ���� �� ���� ������
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
         * ����� ����������� ������� ��������� �����
         */
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex) // ����� �����
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
         * ����� ����������� ������� ��������� ������ ������
         */
        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Operation = comboBox3.SelectedIndex + 1;
        }

        /*
         * ����� ����������� ������� ������� �������
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
         * ���������� ��������� ��������� ��������
         */
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            timer1.Interval = trackBar1.Value;
            label5.Text = trackBar1.Value.ToString();
        }

        /*
         * ���������� ������ �������������
         */
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Transformation = comboBox1.SelectedIndex;
        }

        /*
         * ����� �������, � ������� ���������� ����������� �������
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
         * ���������� ��������� ������������� ����������
         */
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            label10.Text = (trackBar3.Value / 10F).ToString();
            YScale = trackBar3.Value / 10F;
        }

        /*
         * ���������� ��������� ��������������� ����������
         */
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            label8.Text = (trackBar2.Value / 10F).ToString();
            XScale = trackBar2.Value / 10F;
        }
    }
}