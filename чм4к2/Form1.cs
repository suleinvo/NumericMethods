using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace чм4к2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private int Icod = 0;

        //правая часть
        private double func(double x, double y)
        {
            //double f = 2 * x+1;
             double f = 3*x*x+1;
            //double f = Math.Exp(x);
           // double f = 6 * Math.Pow(x, 5) + 5 * Math.Pow(x, 4)+1;
            //double f = 4 * Math.Pow(x, 3);
            //double f = 270 * Math.Pow(x, 4);
            return f;
        }

        public double MethodRungeKutta(double y0, double x0, double h)
        {
            double k1 = h * func(x0, y0);
            double k2 = h * func(x0 + h, y0 + k1);
            double z = y0 + (k1 + k2) / 2.0;
            return z;
        }

        private double Eps(double y1, double y2)
        {
            double eps = Math.Abs((y1 - y2) / (Math.Pow(0.5, 2) - 1));
            return eps;
        }

        private void Do(object sender, EventArgs e)
        {
            Icod = 0;
            dataGridView1.Rows.Clear();
            try
            {
                //входные данные
                double A = Convert.ToDouble(textBox1.Text);
                double B = Convert.ToDouble(textBox2.Text);
                double C = Convert.ToDouble(textBox3.Text);
                double yc = Convert.ToDouble(textBox4.Text);
                double Hmin = Convert.ToDouble(textBox5.Text);
                double Hmax= Convert.ToDouble(textBox11.Text);
                int KHmin = 0;
                int KHmax = 0;
                double Emax = Convert.ToDouble(textBox6.Text);
                if ((C != A) && (C != B))
                {
                    throw new Exceptions.InputException("Точка С должна совпадать с началом или концом отрезка.");
                }

                double h = (B - A) / 10;
                if (h < Hmin)
                {
                    throw new Exceptions.StepException("(B - A) / 10 меньше минимального шага.");
                }
                if (h > Hmax)
                {
                    throw new Exceptions.StepException("(B - A) / 10 больше максимального шага.");
                }

                double y1 = yc;
                double y2 = yc;
                double eps = 0;
                int KE = 0;
                int KM = 0;

                if (C == A)
                {
                    double x = A;
                    double y = yc;
                    dataGridView1.Rows.Add(Convert.ToString(x),
                        Convert.ToString(y), "0", "0");

                    while (B - (x + h) >= Hmin)
                    {
                        y1 = MethodRungeKutta(y, x, h);
                        y2 = MethodRungeKutta(y, x, h / 2.0);
                        y2 = MethodRungeKutta(y2, x + h / 2.0, h / 2.0);
                        eps = Eps(y1, y2);
                        if (h <= Hmin)
                        {
                            Icod = 1;
                            KHmin++;
                            KE++;
                            y1 = MethodRungeKutta(y, x, Hmin);
                            y2 = MethodRungeKutta(y, x, Hmin / 2.0);
                            y2 = MethodRungeKutta(y2, x + Hmin / 2.0, Hmin / 2.0);
                            eps = Eps(y1, y2);
                            y = y1;
                            x = x + Hmin;
                            dataGridView1.Rows.Add(Convert.ToString(x),
                                Convert.ToString(y),
                                Convert.ToString(eps),
                                Convert.ToString(Hmin));
                            continue;
                        }
                        else
                        {
                            if (h >= Hmax)
                            {
                                Icod = 1;
                                KHmax++;
                                KM++;
                                y1 = MethodRungeKutta(y, x, Hmax);
                                y2 = MethodRungeKutta(y, x, Hmax / 2.0);
                                y2 = MethodRungeKutta(y2, x + Hmax / 2.0, Hmax / 2.0);
                                eps = Eps(y1, y2);
                                y = y1;
                                x = x + Hmax;
                                dataGridView1.Rows.Add(Convert.ToString(x),
                                    Convert.ToString(y),
                                    Convert.ToString(eps),
                                    Convert.ToString(Hmax));
                                continue;
                            }
                            else
                            {
                                y = y1;
                                x = x + h;
                                dataGridView1.Rows.Add(Convert.ToString(x),
                                    Convert.ToString(y),
                                    Convert.ToString(eps),
                                    Convert.ToString(h));
                                h = 0.9 * Math.Pow((Emax / eps), 1 / 3.0) * h;
                                if (h > Hmax)
                                { h = Hmax; }

                            }
                        }
                    }
                    if ((B - x) >= (2 * Hmin))
                    {
                        h = (B - x) / 2.0;
                        y = MethodRungeKutta(y1, x, h);
                        dataGridView1.Rows.Add(Convert.ToString(x + h),
                            Convert.ToString(y),
                            Convert.ToString(eps),
                            Convert.ToString(h));
                        h = B - x;
                        y = MethodRungeKutta(y1, x, h);
                        dataGridView1.Rows.Add(Convert.ToString(x + h),
                            Convert.ToString(y),
                            Convert.ToString(eps),
                            Convert.ToString(h / 2));
                    }
                    else
                    {
                        if ((B - x) <= (1.5 * Hmin))
                        {
                            h = B - x;
                            y = MethodRungeKutta(y1, x, h);
                            x = B;
                            dataGridView1.Rows.Add(Convert.ToString(x),
                                Convert.ToString(y),
                                Convert.ToString(eps),
                                Convert.ToString(h));
                        }
                        else
                        {
                            if (((1.5 * Hmin) < (B - x)) && ((B - x) < (2 * Hmin)))
                            {

                                h = (B - x) / 2;
                                y = MethodRungeKutta(y1, x, h);
                                x = x + (B - x) / 2;
                                dataGridView1.Rows.Add(Convert.ToString(x),
                                    Convert.ToString(y),
                                    Convert.ToString(eps),
                                    Convert.ToString(h));

                                h = B - x;
                                y = MethodRungeKutta(y, x, h);
                                x = B;
                                dataGridView1.Rows.Add(Convert.ToString(x),
                                    Convert.ToString(y),
                                    Convert.ToString(eps),
                                    Convert.ToString(h));
                            }
                        }
                    }
                }
                else
                {
                    if (C == B)
                    {
                        h = -h;
                        double x = B;
                        double y = yc;
                        dataGridView1.Rows.Add(Convert.ToString(x),
                            Convert.ToString(y), "0", "0");

                        while (x > A)
                        {
                            if (((x + h) - A) >= Hmin)
                            {
                                y1 = MethodRungeKutta(y, x, h);
                                y2 = MethodRungeKutta(y, x, h / 2);
                                y2 = MethodRungeKutta(y2, x + h / 2, h / 2);
                                eps = Eps(y1, y2);
                                if (Math.Abs(h) <= Hmin)
                                {
                                    Icod = 1;
                                    KHmin++;
                                    KE++;
                                    y1 = MethodRungeKutta(y, x, Hmin);
                                    y2 = MethodRungeKutta(y, x, Hmin / 2.0);
                                    y2 = MethodRungeKutta(y2, x + Hmin / 2.0, Hmin / 2.0);
                                    eps = Eps(y1, y2);
                                    y = y1;
                                    x = x - Hmin;
                                    dataGridView1.Rows.Add(Convert.ToString(x),
                                        Convert.ToString(y),
                                        Convert.ToString(eps),
                                        Convert.ToString(Hmin));
                                    continue;
                                }
                                else
                                {
                                    if (Math.Abs(h) >= Hmax)
                                    {
                                        Icod = 1;
                                        KHmax++;
                                        KM++;
                                        y1 = MethodRungeKutta(y, x, Hmax);
                                        y2 = MethodRungeKutta(y, x, Hmax / 2.0);
                                        y2 = MethodRungeKutta(y2, x + Hmax / 2.0, Hmax / 2.0);
                                        eps = Eps(y1, y2);
                                        y = y1;
                                        x = x - Hmax;
                                        dataGridView1.Rows.Add(Convert.ToString(x),
                                            Convert.ToString(y),
                                            Convert.ToString(eps),
                                            Convert.ToString(Hmax));
                                        continue;
                                    }
                                    else
                                    {
                                        y = y1;
                                        x = x + h;
                                        dataGridView1.Rows.Add(Convert.ToString(x),
                                            Convert.ToString(y),
                                            Convert.ToString(eps),
                                            Convert.ToString(h));
                                        h = 0.9 * Math.Pow((Emax / eps), 1 / 3.0) * h;
                                    }
                                }
                            }
                            else
                            {
                                if ((x - A) >= (2 * Hmin))
                                {
                                    h = -(x - A - Hmin);
                                    y = MethodRungeKutta(y, x, h);
                                    x = A + Hmin;
                                    dataGridView1.Rows.Add(Convert.ToString(x),
                                        Convert.ToString(y),
                                        Convert.ToString(eps),
                                        Convert.ToString(h));

                                    y = MethodRungeKutta(y, x, -Hmin);
                                    x = A;
                                    KHmin++;
                                    dataGridView1.Rows.Add(Convert.ToString(x),
                                        Convert.ToString(y),
                                        Convert.ToString(eps),
                                        Convert.ToString(-Hmin));
                                }
                                else
                                {
                                    if ((x - A) <= (1.5 * Hmin))
                                    {
                                        h = -(x - A);
                                        y = MethodRungeKutta(y, x, h);
                                        x = A;
                                        dataGridView1.Rows.Add(Convert.ToString(x),
                                            Convert.ToString(y),
                                            Convert.ToString(eps),
                                            Convert.ToString(h));
                                    }
                                    else
                                    {
                                        if (((1.5 * Hmin) < (x - A)) && ((x - A) < (2 * Hmin)))
                                        {
                                            h = -((x - A) / 2);
                                            y = MethodRungeKutta(y, x, h);
                                            x = x - (x - A) / 2;
                                            dataGridView1.Rows.Add(Convert.ToString(x),
                                                Convert.ToString(y),
                                                Convert.ToString(eps),
                                                Convert.ToString(h));

                                            h = -(A + x);
                                            y = MethodRungeKutta(y, x, h);
                                            x = A;
                                            dataGridView1.Rows.Add(Convert.ToString(x),
                                                Convert.ToString(y),
                                                Convert.ToString(eps),
                                                Convert.ToString(h));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                textBox8.Text = Convert.ToString(KE);
                textBox12.Text = Convert.ToString(KM);
                textBox9.Text = Convert.ToString(KHmin);
                textBox10.Text = Convert.ToString(Icod);
                textBox7.Text = Convert.ToString(dataGridView1.Rows.Count - 1);
            }
            catch (FormatException ex)
            {
                //ошибка ввода
                Icod = 2;
                textBox10.Text = Convert.ToString(Icod);
                MessageBox.Show(ex.Message, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exceptions.InputException ex)
            {
                //ошибка ввода
                Icod = 2;
                textBox10.Text = Convert.ToString(Icod);
                MessageBox.Show(ex.Message, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exceptions.StepException ex)
            {
                //ошибка с шагом. невозможно начать вычисления
                Icod = 2;
                textBox10.Text = Convert.ToString(Icod);
                MessageBox.Show(ex.Message, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
