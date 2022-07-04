using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace prac
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int k; //количество городов 

        List<TextBox> t_x = new List<TextBox>(); // текст боксы координаты городов 
        List<TextBox> t_y = new List<TextBox>();
        List<Label> l_q = new List<Label>();

        int[] x;// коордитаны
        int[] y;

        List<double[]> mass = new List<double[]>();//города и расстояния между ними

        List<double[]> ready = new List<double[]>();//города которые подсоеденены 

        List<int> ready_1 = new List<int>();
        List<int> ready_2 = new List<int>();//общая длинна кабеля 

        private void button1_Click(object sender, EventArgs e) 
        {
            foreach (TextBox tb in t_x)
                tableLayoutPanel1.Controls.Remove(tb); //удоляет текстобоксы если нужно добавить городов
            foreach (TextBox tb in t_y)
                tableLayoutPanel1.Controls.Remove(tb);
            foreach (Label lb in l_q)
                tableLayoutPanel1.Controls.Remove(lb);
            l_q.Clear();
            t_x.Clear();
            t_y.Clear();

            try //задает количество городов
            {
                k = Convert.ToInt32(textBox1.Text);
                tableLayoutPanel1.RowCount = k;
                for (int i = 0; i < k; i++)
                {
                    Label l = new Label();
                    string str = ("Город " + (i+1).ToString());
                    l.Text = (str);
                    l_q.Add(l);

                    tableLayoutPanel1.Controls.Add(l, 0, i);
                    TextBox textBox_x = new TextBox();
                    t_x.Add(textBox_x);
                        tableLayoutPanel1.Controls.Add(textBox_x, 1, i);
                    

                    TextBox textBox_y = new TextBox();
                    t_y.Add(textBox_y);
                    tableLayoutPanel1.Controls.Add(textBox_y, 2, i);
                }
                
            }
            catch
            {
                MessageBox.Show("Введено неверное число или введены не все коодинаты");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double sum = 0;
            ready_1.Clear();//очистка выполненной работы 
            ready_2.Clear();
            ready.Clear();
            x = new int[k];
            y = new int[k];
            int i = 0;
            try
            {
                foreach (TextBox t in t_x) //вписываем в массивы координаты городов
                {
                    x[i] = Convert.ToInt32(t.Text);
                    i++;
                }
                i = 0;
                foreach (TextBox t in t_y)
                {
                    y[i] = Convert.ToInt32(t.Text);
                    i++;
                }
                for (int a = 0; a < k-1; a++) //длинны соеденений 
                {
                    for (int b = a+1; b < k; b++)
                    {
                        double[] temp = new double[3];
                        temp[0] = a;
                        temp[1] = b;
                        temp[2] = Math.Sqrt(Math.Pow(y[b]-y[a],2) + Math.Pow(x[b] - x[a], 2));
                        mass.Add(temp);
                    }
                }
                sort(mass);
                for (i=0; i<k;i++)
                {
                    if (check_1(ready_1, ready_2, Convert.ToInt32(mass[i][0]), Convert.ToInt32(mass[i][1])))
                    {
                        ready_1.Add(Convert.ToInt32(mass[i][0]));
                        ready_2.Add(Convert.ToInt32(mass[i][1]));
                        check(ready_1, ready_2, Convert.ToInt32(mass[i][0]), Convert.ToInt32(mass[i][1]));
                        ready.Add(mass[i]);
                        sum += mass[i][2];
                    }
                }
                pictureBox1.Invalidate();
                MessageBox.Show("Длинна наименьшего пути: "+sum.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неправильный ввод" + ex.ToString());
                x = new int[k];
                y = new int[k];
            }

            
        }

        private void sort(List<double[]> mass) //сортировка методом пузырька
        {
            double[] temp = new double[3];
            for(int i = 0; i < mass.Count; i++)
            {
                for(int j = i+ 1; j< mass.Count;j++ )
                {
                    if (mass[i][2] > mass[j][2])
                    {
                        temp = mass[i];
                        mass[i] = mass[j];
                        mass[j] = temp;
                    }
                }
            }
        }

        void check(List<int> ready_1, List<int> ready_2, int a, int b) //проверка на связи
        {
            List<int> temp = new List<int>();
            for (int i = 0; i < ready_1.Count; i++)
            {
                if (a == ready_1[i])
                    temp.Add(ready_2[i]);
            }
            foreach (int i in temp)
            {
                if (check_1(ready_1, ready_2, b, i))
                {
                    ready_1.Add(Math.Min(b, i));
                    ready_2.Add(Math.Max(b, i));
                }
            }
            temp.Clear();
            for (int i = 0; i < ready_1.Count; i++)
            {
                if (b == ready_2[i])
                    temp.Add(ready_1[i]);
            }
            foreach (int i in temp)
            {
                if (check_1(ready_1, ready_2, a, i))
                {
                    ready_1.Add(Math.Min(a, i));
                    ready_2.Add(Math.Max(a, i));
                }
            }
            temp.Clear();
            for (int i = 0; i < ready_1.Count; i++)
            {
                if (a == ready_2[i])
                    temp.Add(ready_1[i]);
            }
            foreach (int i in temp)
            {
                if (check_1(ready_1, ready_2, b, i))
                {
                    ready_1.Add(Math.Min(b, i));
                    ready_2.Add(Math.Max(b, i));
                }
            }
            temp.Clear();
            for (int i = 0; i < ready_1.Count; i++)
            {
                if (b == ready_1[i])
                    temp.Add(ready_2[i]);
            }
            foreach (int i in temp)
            {
                if (check_1(ready_1, ready_2, a, i))
                {
                    ready_1.Add(Math.Min(a, i));
                    ready_2.Add(Math.Max(a, i));
                }
            }
            temp.Clear();
        }

        private bool check_1(List<int> ready_1, List<int> ready_2, int a, int b)//проверка на соеденение в списке
        {
            for (int i = 0; i < ready_1.Count; i++)
            {
                if (ready_1[i] == a && ready_2[i] == b)
                    return false;
            }
            return true;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)//рисование точек и линий
        {
            try
            {
                float max = Math.Max(Math.Max(Math.Abs(x.Max()), Math.Abs(x.Min())), Math.Max(Math.Abs(y.Max()), Math.Abs(y.Min())));
                float k_1 = ((Math.Min(pictureBox1.Width, pictureBox1.Height) / 2) / max) * 0.98f;
                Graphics g = e.Graphics;
                Brush br = new SolidBrush(Color.Black);
                Pen p = new Pen(Color.Coral);
                foreach (double[] d in ready)
                {
                    g.FillEllipse(br, x[Convert.ToInt32(d[0])]*k_1 + pictureBox1.Width / 2 - 3, pictureBox1.Height / 2 - y[Convert.ToInt32(d[0])]*k_1 - 3, 6, 6);
                    g.FillEllipse(br, x[Convert.ToInt32(d[1])]*k_1 + pictureBox1.Width / 2 - 3, pictureBox1.Height / 2 - y[Convert.ToInt32(d[1])]*k_1 - 3, 6, 6);
                    g.DrawLine(p, x[Convert.ToInt32(d[0])]*k_1 + pictureBox1.Width / 2, pictureBox1.Height / 2 - y[Convert.ToInt32(d[0])]*k_1,
                        x[Convert.ToInt32(d[1])] * k_1 + pictureBox1.Width / 2, pictureBox1.Height / 2 - y[Convert.ToInt32(d[1])] * k_1);
                }
            }
            catch
            {
                
            }
        }
    }
}
