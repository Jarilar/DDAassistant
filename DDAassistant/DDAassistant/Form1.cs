using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DDAassistant
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static double[,] PlanArray;//计划计算数组
        public int num;//行数计数
        public int Nocol;//当前所在行数

        //导入计划的计算文件(预计包括相对湿度值，有效半径，模型大小)
        private void IMPORTbtn_Click(object sender, EventArgs e)
        {
            //使用文件浏览器导入txt文件
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "txt files (*.txt)|*.txt";
            fileDialog.ShowDialog();
            string path = fileDialog.FileName;

            //将txt文件内的数据读入到二维数组内
            string[] fileLines = File.ReadAllLines(path);//读取所有数据
            PlanArray = new double[fileLines.Length, fileLines[0].Split(',').Length];//定义二维数组
            for (int i = 0; i < fileLines.Length; ++i)
            {
                string line = fileLines[i];
                num++;
                for (int j = 0; j < PlanArray.GetLength(1); ++j)
                {                    
                    string[] split = line.Split(',');
                    PlanArray[i, j] = double.Parse(split[j]);
                }
            }

            //监视
            StringBuilder ss = new StringBuilder();
            foreach (var s in PlanArray)
            {
                ss.Append(s.ToString()+"\r\n");
            }
            this.importShbox.Text = ss.ToString();
        }

        private void Buildbtn_Click(object sender, EventArgs e)
        {
            switch(PlanArray[0,0])
            {
                case 1:
                    for(int i=1;i<num;i++)
                    {
                        Nocol = i;
                        SingleBull();
                        string ModelName = "球体";
                        string Fname = PlanArray[i, 0].ToString();
                        monitorbox.Text += "模型：" + ModelName + "RH=" + Fname + "%     完成" + "\r\n";
                    }
                    break;
                case 2:
                    DoubleBull();
                    break;
                case 3:
                    TripleBull();
                    break;
                case 4:
                    Cube();
                    break;
                case 5:
                    Cylinder();
                    break;

            }
                
        }

        private void Parbtn_Click(object sender, EventArgs e)
        {
            switch (PlanArray[0, 0])
            {
                case 1:
                    Parameter1();
                    break;
                case 2:
                    Parameter2();
                    break;
                case 3:
                    Parameter3();
                    break;
                case 4:
                    Parameter1();
                    break;
                case 5:
                    Parameter1();
                    break;

            }
        }

        private void Ribtn_Click(object sender, EventArgs e)
        {
            RI();
        }

        //模型生成函数

        private void SingleBull()//球状单颗粒
        {
            int n = 1;//计数用
            string[,] arr = new string[500000, 7];//定义一个字符数组

            ////录入数据////////////////////////////////////////////

            double R1, R2;

            //生成形状

            arr[0, 0] = ">TARCEL:";
            arr[0, 1] = "shape;";
            n++;

            arr[1, 1] = "=";
            arr[1, 2] = "NAT";
            n++;
            arr[2, 1] = "1.000000";
            arr[2, 2] = "0.000000";
            arr[2, 3] = "0.000000";
            arr[2, 4] = "=";
            arr[2, 5] = "A_1 vector";
            n++;
            arr[3, 0] = "0.000000";
            arr[3, 1] = "1.000000";
            arr[3, 2] = "0.000000";
            arr[3, 3] = "=";
            arr[3, 4] = "A_2 vector";
            n++;
            arr[4, 0] = "1.000000";
            arr[4, 1] = "1.000000";
            arr[4, 2] = "1.000000";
            arr[4, 3] = "=";
            arr[4, 4] = "lattice spacings (d_x,d_y,d_z)/d";
            n++;
            arr[5, 0] = "-0.500000";
            arr[5, 1] = "-0.500000";
            arr[5, 2] = "-0.500000";
            arr[5, 3] = "=";
            arr[5, 4] = "lattice offset x0(1-3) ";
            arr[5, 5] = "=";
            arr[5, 6] = "(x_TF,y_TF,z_TF)/d for dipole 0 0 0";
            n++;
            arr[6, 0] = "JA";
            arr[6, 1] = "IX";
            arr[6, 2] = "IY";
            arr[6, 3] = "IZ";
            arr[6, 4] = "ICOMP(x,y,z)";

                R1 = PlanArray[Nocol, 2];
                R2 = PlanArray[Nocol, 3];
                for (int i = -(int)R2; i <= R2; i++)
                {
                    for (int j = -(int)R2; j <= R2; j++)
                    {
                        for (int k = -(int)R2; k <= R2; k++)
                        {
                            double tempR = Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2) + Math.Pow(k, 2));

                            if (R1 < tempR && tempR <= R2)
                            {
                                arr[n, 0] = (n - 6).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "1";
                                arr[n, 5] = "1";
                                arr[n, 6] = "1";
                                n++;
                            }
                            else if (R1 >= tempR)
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "2";
                                arr[n, 5] = "2";
                                arr[n, 6] = "2";
                                n++;
                            }

                        }
                    }
                }
                arr[1, 0] = (n - 7).ToString();
                //写入txt文档
                string Fname = PlanArray[Nocol, 0].ToString();
                //保存地址为D盘自定义文件名
                FileStream fs = new FileStream("D:\\test\\" + Fname + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //初始化二维数组s 用来接收arr
                string[,] s = new string[500000, 7];
                //接收arr
                s = arr;
                for (int l = 0; l < 500000; ++l)
                {
                    for (int h = 0; h < 7; ++h)
                    {
                        //s的每个值和arr的每个值对应
                        s[l, h] = arr[l, h];
                        string output;
                        output = s[l, h];
                        //有个空格作为间隔，
                        sw.Write(output + " ");
                    }
                    sw.WriteLine();
                }
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();  
        }

        private void DoubleBull()//球状双颗粒
        {
            double Rmax, DRmax;
            int n = 1;//计数用
            string[,] arr = new string[500000, 7];//定义一个字符数组

            ////录入数据////////////////////////////////////////////

            //获得最大半径           

            //生成形状

            arr[0, 0] = ">TARCEL:";
            arr[0, 1] = "shape;";
            n++;

            arr[1, 1] = "=";
            arr[1, 2] = "NAT";
            n++;
            arr[2, 1] = "1.000000";
            arr[2, 2] = "0.000000";
            arr[2, 3] = "0.000000";
            arr[2, 4] = "=";
            arr[2, 5] = "A_1 vector";
            n++;
            arr[3, 0] = "0.000000";
            arr[3, 1] = "1.000000";
            arr[3, 2] = "0.000000";
            arr[3, 3] = "=";
            arr[3, 4] = "A_2 vector";
            n++;
            arr[4, 0] = "1.000000";
            arr[4, 1] = "1.000000";
            arr[4, 2] = "1.000000";
            arr[4, 3] = "=";
            arr[4, 4] = "lattice spacings (d_x,d_y,d_z)/d";
            n++;
            arr[5, 0] = "-0.500000";
            arr[5, 1] = "-0.500000";
            arr[5, 2] = "-0.500000";
            arr[5, 3] = "=";
            arr[5, 4] = "lattice offset x0(1-3) ";
            arr[5, 5] = "=";
            arr[5, 6] = "(x_TF,y_TF,z_TF)/d for dipole 0 0 0";
            n++;
            arr[6, 0] = "JA";
            arr[6, 1] = "IX";
            arr[6, 2] = "IY";
            arr[6, 3] = "IZ";
            arr[6, 4] = "ICOMP(x,y,z)";

            for (int m = 0; m < num; m++)
            {
                double R1 = PlanArray[m, 2];
                double R2 = PlanArray[m, 3];
                double R3 = PlanArray[m, 4];
                double R4 = PlanArray[m, 5];

                if (R2 <= R4)
                    Rmax = R4;
                else
                    Rmax = R2;

                DRmax = 2 * Rmax;
                for (int i = -(int)DRmax; i <= DRmax; i++)
                {
                    for (int j = -(int)Rmax; j <= Rmax; j++)
                    {
                        for (int k = -(int)Rmax; k <= Rmax; k++)
                        {
                            double CA = Math.Sqrt(Math.Pow(i + R1, 2) + Math.Pow(j, 2) + Math.Pow(k, 2));
                            double CB = Math.Sqrt(Math.Pow(i - R3, 2) + Math.Pow(j, 2) + Math.Pow(k, 2));

                            if (R1 < CA && CA <= R2 && CB > R3)
                            {
                                arr[n, 0] = (n - 6).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "1";
                                arr[n, 5] = "1";
                                arr[n, 6] = "1";
                                n++;
                            }
                            else if (R3 < CB && CB <= R4 && CA > R1)
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "1";
                                arr[n, 5] = "1";
                                arr[n, 6] = "1";
                                n++;
                            }
                            else if (CA <= R1)
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "2";
                                arr[n, 5] = "2";
                                arr[n, 6] = "2";
                                n++;
                            }
                            else if (CB <= R3)
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "3";
                                arr[n, 5] = "3";
                                arr[n, 6] = "3";
                                n++;
                            }
                        }
                    }
                }

                arr[1, 0] = (n - 7).ToString();
                //写入txt文档
                string Fname = PlanArray[m, 0].ToString();
                string ModelName = "双球体";
                monitorbox.Text += "模型：" + ModelName + "RH=" + Fname + "%     完成" + "\r\n";
                //保存地址为D盘自定义文件名
                FileStream fs = new FileStream("D:\\test\\" + Fname + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //初始化二维数组s 用来接收arr
                string[,] s = new string[500000, 7];
                //接收arr
                s = arr;

                for (int l = 0; l < 500000; ++l)
                {
                    for (int h = 0; h < 7; ++h)
                    {
                        //s的每个值和arr的每个值对应
                        s[l, h] = arr[l, h];

                        string output;

                        output = s[l, h];
                        //有个空格作为间隔，
                        sw.Write(output + " ");
                    }
                    sw.WriteLine();
                }
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
        }

        private void TripleBull()//球状三颗粒
        {
            double Rmax, DRmax;
            int n = 1;//计数用
            string[,] arr = new string[500000, 7];//定义一个字符数组

            ////录入数据////////////////////////////////////////////

            //获得最大半径           

            //生成形状

            arr[0, 0] = ">TARCEL:";
            arr[0, 1] = "shape;";
            n++;

            arr[1, 1] = "=";
            arr[1, 2] = "NAT";
            n++;
            arr[2, 1] = "1.000000";
            arr[2, 2] = "0.000000";
            arr[2, 3] = "0.000000";
            arr[2, 4] = "=";
            arr[2, 5] = "A_1 vector";
            n++;
            arr[3, 0] = "0.000000";
            arr[3, 1] = "1.000000";
            arr[3, 2] = "0.000000";
            arr[3, 3] = "=";
            arr[3, 4] = "A_2 vector";
            n++;
            arr[4, 0] = "1.000000";
            arr[4, 1] = "1.000000";
            arr[4, 2] = "1.000000";
            arr[4, 3] = "=";
            arr[4, 4] = "lattice spacings (d_x,d_y,d_z)/d";
            n++;
            arr[5, 0] = "-0.500000";
            arr[5, 1] = "-0.500000";
            arr[5, 2] = "-0.500000";
            arr[5, 3] = "=";
            arr[5, 4] = "lattice offset x0(1-3) ";
            arr[5, 5] = "=";
            arr[5, 6] = "(x_TF,y_TF,z_TF)/d for dipole 0 0 0";
            n++;
            arr[6, 0] = "JA";
            arr[6, 1] = "IX";
            arr[6, 2] = "IY";
            arr[6, 3] = "IZ";
            arr[6, 4] = "ICOMP(x,y,z)";

            for (int m = 0; m < num; m++)
            {
                double R1 = PlanArray[m, 2];
                double R2 = PlanArray[m, 3];//外径1
                double R3 = PlanArray[m, 4];
                double R4 = PlanArray[m, 5];//外径2
                double R5 = PlanArray[m, 6];
                double R6 = PlanArray[m, 7];//外径3

                //获取最大的半径值 
                if (R6 >= R4 && R6 >= R2)
                    Rmax = R6;
                else if (R4 >= R6 && R4 >= R2)
                    Rmax = R4;
                else
                    Rmax = R2;

                DRmax = 2 * Rmax;

                //模型生成程序
                for (int i = -(int)DRmax; i <= DRmax; i++)
                {
                    for (int j = -(int)Rmax; j <= Rmax; j++)
                    {
                        for (int k = -(int)Rmax; k <= Rmax; k++)
                        {
                            double CA = Math.Sqrt(Math.Pow(i - R1-R3, 2) + Math.Pow(j, 2) + Math.Pow(k, 2));
                            double CB = Math.Sqrt(Math.Pow(i , 2) + Math.Pow(j, 2) + Math.Pow(k, 2));
                            double CC = Math.Sqrt(Math.Pow(i +R3+R5,2) + Math.Pow(j, 2) + Math.Pow(k, 2));

                            if ((R1 < CA) && (CA <= R2) && (CB > R3))
                            {
                                arr[n, 0] = (n - 6).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "1";
                                arr[n, 5] = "1";
                                arr[n, 6] = "1";
                                n++;
                            }
                            else if ((R3 < CB) && (CB <= R4) && (CA > R1))
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "1";
                                arr[n, 5] = "1";
                                arr[n, 6] = "1";
                                n++;
                            }
                            else if((R5 < CC) && (CC <= R6) && (CA > R3))
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "1";
                                arr[n, 5] = "1";
                                arr[n, 6] = "1";
                                n++;
                            }
                            else if (CA <= R1)
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "2";
                                arr[n, 5] = "2";
                                arr[n, 6] = "2";
                                n++;
                            }
                            else if (CB <= R3)
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "3";
                                arr[n, 5] = "3";
                                arr[n, 6] = "3";
                                n++;
                            }
                            else if (CC <= R5)
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "4";
                                arr[n, 5] = "4";
                                arr[n, 6] = "4";
                                n++;
                            }
                        }
                    }
                }

                arr[1, 0] = (n - 7).ToString();
                //写入txt文档
                string Fname = PlanArray[m, 0].ToString();
                string ModelName = "三颗球体";
                monitorbox.Text += "模型：" + ModelName + "RH=" + Fname + "%     完成" + "\r\n";
                //保存地址为D盘自定义文件名
                FileStream fs = new FileStream("D:\\test\\" + Fname + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //初始化二维数组s 用来接收arr
                string[,] s = new string[500000, 7];
                //接收arr
                s = arr;

                for (int l = 0; l < 500000; ++l)
                {
                    for (int h = 0; h < 7; ++h)
                    {
                        //s的每个值和arr的每个值对应
                        s[l, h] = arr[l, h];

                        string output;

                        output = s[l, h];
                        //有个空格作为间隔，
                        sw.Write(output + " ");
                    }
                    sw.WriteLine();
                }
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();
            }
        }

        private void Cube()//立方体
        {
            int n = 1;//计数用
            string[,] arr = new string[500000, 7];//定义一个字符数组

            ////录入数据////////////////////////////////////////////

            double a,b,c,A,B,C;

            //生成形状

            arr[0, 0] = ">TARCEL:";
            arr[0, 1] = "shape;";
            n++;

            arr[1, 1] = "=";
            arr[1, 2] = "NAT";
            n++;
            arr[2, 1] = "1.000000";
            arr[2, 2] = "0.000000";
            arr[2, 3] = "0.000000";
            arr[2, 4] = "=";
            arr[2, 5] = "A_1 vector";
            n++;
            arr[3, 0] = "0.000000";
            arr[3, 1] = "1.000000";
            arr[3, 2] = "0.000000";
            arr[3, 3] = "=";
            arr[3, 4] = "A_2 vector";
            n++;
            arr[4, 0] = "1.000000";
            arr[4, 1] = "1.000000";
            arr[4, 2] = "1.000000";
            arr[4, 3] = "=";
            arr[4, 4] = "lattice spacings (d_x,d_y,d_z)/d";
            n++;
            arr[5, 0] = "-0.500000";
            arr[5, 1] = "-0.500000";
            arr[5, 2] = "-0.500000";
            arr[5, 3] = "=";
            arr[5, 4] = "lattice offset x0(1-3) ";
            arr[5, 5] = "=";
            arr[5, 6] = "(x_TF,y_TF,z_TF)/d for dipole 0 0 0";
            n++;
            arr[6, 0] = "JA";
            arr[6, 1] = "IX";
            arr[6, 2] = "IY";
            arr[6, 3] = "IZ";
            arr[6, 4] = "ICOMP(x,y,z)";

            for (int m = 1; m < num; m++)
            {

                a = PlanArray[m, 2];
                b = PlanArray[m, 3];
                c = PlanArray[m, 4];
                A = PlanArray[m, 5];
                B = PlanArray[m, 6];
                C = PlanArray[m, 7];

                for (int i = -(int)(A/2); i <= (A/2); i++)
                {
                    for (int j = -(int)(B/2); j <= (B/2); j++)
                    {
                        for (int k = -(int)(C/2); k <= (C/2); k++)
                        {
                            if ((Math.Abs(i) <= (a/2)) && (Math.Abs(j)<=(b/2))&&(Math.Abs(k) <= (c/ 2)))
                            {
                                arr[n, 0] = (n - 6).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "2";
                                arr[n, 5] = "2";
                                arr[n, 6] = "2";
                                n++;
                            }
                            else 
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "1";
                                arr[n, 5] = "1";
                                arr[n, 6] = "1";
                                n++;
                            }

                        }
                    }
                }
                arr[1, 0] = (n - 7).ToString();
                //写入txt文档
                string Fname = PlanArray[m, 0].ToString();
                string ModelName = "立方体";
                monitorbox.Text += "模型："+ ModelName+ "RH=" + Fname + "%     完成" + "\r\n";
                //保存地址为D盘自定义文件名
                FileStream fs = new FileStream("D:\\test\\" + Fname + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //初始化二维数组s 用来接收arr
                string[,] s = new string[500000, 7];
                //接收arr
                s = arr;

                for (int l = 0; l < 500000; ++l)
                {
                    for (int h = 0; h < 7; ++h)
                    {
                        //s的每个值和arr的每个值对应
                        s[l, h] = arr[l, h];

                        string output;

                        output = s[l, h];
                        //有个空格作为间隔，
                        sw.Write(output + " ");
                    }
                    sw.WriteLine();
                }
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();

            }
        }

        private void Cylinder()//圆柱体
        {
            int n = 1;//计数用
            string[,] arr = new string[500000, 7];//定义一个字符数组

            ////录入数据////////////////////////////////////////////

            double r, h1,R,H;

            //生成形状

            arr[0, 0] = ">TARCEL:";
            arr[0, 1] = "shape;";
            n++;

            arr[1, 1] = "=";
            arr[1, 2] = "NAT";
            n++;
            arr[2, 1] = "1.000000";
            arr[2, 2] = "0.000000";
            arr[2, 3] = "0.000000";
            arr[2, 4] = "=";
            arr[2, 5] = "A_1 vector";
            n++;
            arr[3, 0] = "0.000000";
            arr[3, 1] = "1.000000";
            arr[3, 2] = "0.000000";
            arr[3, 3] = "=";
            arr[3, 4] = "A_2 vector";
            n++;
            arr[4, 0] = "1.000000";
            arr[4, 1] = "1.000000";
            arr[4, 2] = "1.000000";
            arr[4, 3] = "=";
            arr[4, 4] = "lattice spacings (d_x,d_y,d_z)/d";
            n++;
            arr[5, 0] = "-0.500000";
            arr[5, 1] = "-0.500000";
            arr[5, 2] = "-0.500000";
            arr[5, 3] = "=";
            arr[5, 4] = "lattice offset x0(1-3) ";
            arr[5, 5] = "=";
            arr[5, 6] = "(x_TF,y_TF,z_TF)/d for dipole 0 0 0";
            n++;
            arr[6, 0] = "JA";
            arr[6, 1] = "IX";
            arr[6, 2] = "IY";
            arr[6, 3] = "IZ";
            arr[6, 4] = "ICOMP(x,y,z)";

            for (int m = 1; m < num; m++)
            {
                r = PlanArray[m, 2];
                h1 = PlanArray[m, 3];
                R = PlanArray[m, 4];
                H = PlanArray[m, 5];
                for (int i = -(int)R; i <= R; i++)
                {
                    for (int j = -(int)R; j <= R; j++)
                    {
                        for (int k = -(int)H; k <= H; k++)
                        {
                            double tempR = Math.Sqrt(Math.Pow(i, 2) + Math.Pow(j, 2));

                            if (r < tempR && tempR <= R)
                            {
                                arr[n, 0] = (n - 6).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "1";
                                arr[n, 5] = "1";
                                arr[n, 6] = "1";
                                n++;
                            }
                            else if (r >= tempR)
                            {
                                arr[n, 0] = (n - 7).ToString();
                                arr[n, 1] = i.ToString();
                                arr[n, 2] = j.ToString();
                                arr[n, 3] = k.ToString();
                                arr[n, 4] = "2";
                                arr[n, 5] = "2";
                                arr[n, 6] = "2";
                                n++;
                            }

                        }
                    }
                }
                arr[1, 0] = (n - 7).ToString();
                //写入txt文档
                string Fname = PlanArray[m, 0].ToString();
                string ModelName = "立方体";
                monitorbox.Text += "模型：" + ModelName + "RH=" + Fname + "%     完成" + "\r\n";
                //保存地址为D盘自定义文件名
                FileStream fs = new FileStream("D:\\test\\" + Fname + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                //初始化二维数组s 用来接收arr
                string[,] s = new string[500000, 7];
                //接收arr
                s = arr;

                for (int l = 0; l < 500000; ++l)
                {
                    for (int h = 0; h < 7; ++h)
                    {
                        //s的每个值和arr的每个值对应
                        s[l, h] = arr[l, h];

                        string output;

                        output = s[l, h];
                        //有个空格作为间隔，
                        sw.Write(output + " ");
                    }
                    sw.WriteLine();
                }
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fs.Close();

            }
        }

        //参数生成函数

        private void Parameter1()
        {
            int n = 1;//计数用
            string[,] arr = new string[500000, 7];//定义一个字符数组
            for (int m = 1; m < num; m++)
            {
                arr[0, 0] = "' ========== Parameter file for v7.3 ==================='";
                arr[1, 0] = "'**** Preliminaries ****' ";
                arr[2, 0] = "'NOTORQ' = CMDTRQ*6 (DOTORQ, NOTORQ) -- either do or skip torque calculations";
                arr[3, 0] = "'PBCGS2' = CMDSOL*6 (PBCGS2, PBCGST, GPBICG, QMRCCG, PETRKP) -- CCG method ";
                arr[4, 0] = "'GPFAFT' = CMETHD*6 (GPFAFT, FFTMKL) -- FFT method";
                arr[5, 0] = "'GKDLDR' = CALPHA*6 (GKDLDR, LATTDR, FLTRCD) -- DDA method ";
                arr[6, 0] = "'NOTBIN' = CBINFLAG (NOTBIN, ORIBIN, ALLBIN) -- binary output? ";
                arr[7, 0] = "'**** Initial Memory Allocation ****' ";
                arr[8, 0] = "200 200 200 = dimensioning allowance for target generation ";
                arr[9, 0] = "'**** Target Geometry and Composition ****' ";
                arr[10, 0] = "'FRMFILPBC' = CSHAPE*9 shape directive";
                arr[11, 0] = "0 0 '../shapefile/" + PlanArray[m, 0].ToString() + ".txt'";
                arr[12, 0] = "2         = NCOMP = number of dielectric materials";
                arr[13, 0] = "'../diel/"+PlanArray[1,4].ToString()+".txt' = file with refractive index 1";
                arr[14, 0] = "'../diel/"+ PlanArray[1, 5].ToString() + ".txt' = file with refractive index 2";
                arr[15, 0] = "'**** Additional Nearfield calculation? ****' ";
                arr[16, 0] = "0 = NRFLD (=0 to skip nearfield calc., =1 to calculate nearfield E) ";
                arr[17, 0] = "0.0 0.0 0.0 0.0 0.0 0.0 (fract. extens. of calc. vol. in -x,+x,-y,+y,-z,+z)";
                arr[18, 0] = "'**** Error Tolerance ****'";
                arr[19, 0] = "1.00e-5 = TOL = MAX ALLOWED (NORM OF |G>=AC|E>-ACA|X>)/(NORM OF AC|E>)";
                arr[20, 0] = "'**** Maximum number of iterations ****'";
                arr[21, 0] = "10000     = MXITER";
                arr[22, 0] = "'**** Integration limiter for PBC calculations ****'";
                arr[23, 0] = "1.00e-2 = GAMMA (1e-2 is normal, 3e-3 for greater accuracy)";
                arr[24, 0] = "'**** Angular resolution for calculation of <cos>, etc. ****'";
                arr[25, 0] = "0.5	= ETASCA (number of angles is proportional to [(3+x)/ETASCA]^2 )";
                arr[26, 0] = "'**** Wavelengths (micron) ****'";
                arr[27, 0] = "0.55 0.55 1 'INV' = wavelengths (1st,last,howmany,how=LIN,INV,LOG,TAB)";
                arr[28, 0] = "'**** Refractive index of ambient medium ****'";
                arr[29, 0] = "1.0000 = NAMBIENT";
                arr[30, 0] = "'**** Effective Radii (micron) **** '";
                arr[31, 0] = PlanArray[m, 1].ToString();
                arr[31, 1] = PlanArray[m, 1].ToString() + " 1 'LIN' = eff. radii (1st,last,howmany,how=LIN,INV,LOG,TAB)";
                arr[32, 0] = "'**** Define Incident Polarizations ****'";
                arr[33, 0] = "(0,0) (1.,0.) (0.,0.) = Polarization state e01 (k along x axis)";
                arr[34, 0] = "2 = IORTH  (=1 to do only pol. state e01; =2 to also do orth. pol. state)";
                arr[35, 0] = "'**** Specify which output files to write ****'";
                arr[36, 0] = "1 = IWRKSC (=0 to suppress, =1 to write .sca file for each target orient.";
                arr[37, 0] = "'**** Specify Target Rotations ****'";
                arr[38, 0] = "0.    0.   1  = BETAMI, BETAMX, NBETA  (beta=rotation around a1)";
                arr[39, 0] = "0.    0.   1  = THETMI, THETMX, NTHETA (theta=angle between a1 and k)";
                arr[40,0] = "0.    0.   1  = PHIMIN, PHIMAX, NPHI (phi=rotation angle of a1 around k)";
                arr[41, 0] = "'**** Specify first IWAV, IRAD, IORI (normally 0 0 0) ****'";
                arr[42, 0] = "0   0   0    = first IWAV, first IRAD, first IORI (0 0 0 to begin fresh)";
                arr[43, 0] = "'**** Select Elements of S_ij Matrix to Print ****'";
                arr[44, 0] = "9	= NSMELTS = number of elements of S_ij to print (not more than 9)";
                arr[45, 0] = "11 12 21 22 31 33 44 34 43	= indices ij of elements to print";
                arr[46, 0] = "'**** Specify Scattered Directions ****'";
                arr[47, 0] = "'LFRAME' = CMDFRM (LFRAME, TFRAME for Lab Frame or Target Frame)";
                arr[48, 0] = "1 = NPLANES = number of scattering planes";
                arr[49, 0] = "0.  0. 180. 1 = phi, theta_min, theta_max (deg) for plane A";


                //写入txt文档
                string Fname = "ddcast.par";
                string directoryPath = @"D:\test\" + PlanArray[m, 0].ToString();//定义一个路径变量
                if (!Directory.Exists(directoryPath))//如果路径不存在
                {
                    Directory.CreateDirectory(directoryPath);//创建一个路径的文件夹
                }
                monitorbox.Text += "参数文件：RH=" + Fname + "%     完成" + "\r\n";
                StreamWriter sw = new StreamWriter(Path.Combine(directoryPath, Fname));
                //初始化二维数组s 用来接收arr
                string[,] s = new string[52, 2];
                //接收arr
                s = arr;

                for (int l = 0; l < 52; ++l)
                {
                    for (int h = 0; h < 2; ++h)
                    {
                        //s的每个值和arr的每个值对应
                        s[l, h] = arr[l, h];

                        string output;

                        output = s[l, h];
                        //有个空格作为间隔，
                        sw.Write(output + " ");
                    }
                    sw.WriteLine();
                }
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                //fs.Close();
            }
        }

        private void Parameter2()
        {
            int n = 1;//计数用
            string[,] arr = new string[500000, 7];//定义一个字符数组
            for (int m = 1; m < num; m++)
            {
                arr[0, 0] = "' ========== Parameter file for v7.3 ==================='";
                arr[1, 0] = "'**** Preliminaries ****' ";
                arr[2, 0] = "'NOTORQ' = CMDTRQ*6 (DOTORQ, NOTORQ) -- either do or skip torque calculations";
                arr[3, 0] = "'PBCGS2' = CMDSOL*6 (PBCGS2, PBCGST, GPBICG, QMRCCG, PETRKP) -- CCG method ";
                arr[4, 0] = "'GPFAFT' = CMETHD*6 (GPFAFT, FFTMKL) -- FFT method";
                arr[5, 0] = "'GKDLDR' = CALPHA*6 (GKDLDR, LATTDR, FLTRCD) -- DDA method ";
                arr[6, 0] = "'NOTBIN' = CBINFLAG (NOTBIN, ORIBIN, ALLBIN) -- binary output? ";
                arr[7, 0] = "'**** Initial Memory Allocation ****' ";
                arr[8, 0] = "200 200 200 = dimensioning allowance for target generation ";
                arr[9, 0] = "'**** Target Geometry and Composition ****' ";
                arr[10, 0] = "'FRMFILPBC' = CSHAPE*9 shape directive";
                arr[11, 0] = "0 0 '../shapefile/"+PlanArray[m, 0].ToString() + ".txt'";
                arr[12, 0] = "3         = NCOMP = number of dielectric materials";
                arr[13, 0] = "'../diel/Water.txt' = file with refractive index 1";
                arr[14, 0] = "'../diel/material1.txt' = file with refractive index 2";
                arr[15, 0] = "'../diel/material2.txt' = file with refractive index 3";
                arr[16, 0] = "'**** Additional Nearfield calculation? ****' ";
                arr[17, 0] = "0 = NRFLD (=0 to skip nearfield calc., =1 to calculate nearfield E) ";
                arr[18, 0] = "0.0 0.0 0.0 0.0 0.0 0.0 (fract. extens. of calc. vol. in -x,+x,-y,+y,-z,+z)";
                arr[19, 0] = "'**** Error Tolerance ****'";
                arr[20, 0] = "1.00e-5 = TOL = MAX ALLOWED (NORM OF |G>=AC|E>-ACA|X>)/(NORM OF AC|E>)";
                arr[21, 0] = "'**** Maximum number of iterations ****'";
                arr[22, 0] = "10000     = MXITER";
                arr[23, 0] = "'**** Integration limiter for PBC calculations ****'";
                arr[24, 0] = "1.00e-2 = GAMMA (1e-2 is normal, 3e-3 for greater accuracy)";
                arr[25, 0] = "'**** Angular resolution for calculation of <cos>, etc. ****'";
                arr[26, 0] = "0.5	= ETASCA (number of angles is proportional to [(3+x)/ETASCA]^2 )";
                arr[27, 0] = "'**** Wavelengths (micron) ****'";
                arr[28, 0] = "0.55 0.55 1 'INV' = wavelengths (1st,last,howmany,how=LIN,INV,LOG,TAB)";
                arr[29, 0] = "'**** Refractive index of ambient medium ****'";
                arr[30, 0] = "1.0000 = NAMBIENT";
                arr[31, 0] = "'**** Effective Radii (micron) **** '";
                arr[32, 0] = PlanArray[m, 1].ToString();
                arr[32, 1] = PlanArray[m, 1].ToString() + " 1 'LIN' = eff. radii (1st,last,howmany,how=LIN,INV,LOG,TAB)";
                arr[33, 0] = "'**** Define Incident Polarizations ****'";
                arr[34, 0] = "(0,0) (1.,0.) (0.,0.) = Polarization state e01 (k along x axis)";
                arr[35, 0] = "2 = IORTH  (=1 to do only pol. state e01; =2 to also do orth. pol. state)";
                arr[36, 0] = "'**** Specify which output files to write ****'";
                arr[37, 0] = "1 = IWRKSC (=0 to suppress, =1 to write .sca file for each target orient.";
                arr[38, 0] = "'**** Specify Target Rotations ****'";
                arr[39, 0] = "0.    0.   1  = BETAMI, BETAMX, NBETA  (beta=rotation around a1)";
                arr[40, 0] = "0.    0.   1  = THETMI, THETMX, NTHETA (theta=angle between a1 and k)";
                arr[41, 0] = "0.    0.   1  = PHIMIN, PHIMAX, NPHI (phi=rotation angle of a1 around k)";
                arr[42, 0] = "'**** Specify first IWAV, IRAD, IORI (normally 0 0 0) ****'";
                arr[43, 0] = "0   0   0    = first IWAV, first IRAD, first IORI (0 0 0 to begin fresh)";
                arr[44, 0] = "'**** Select Elements of S_ij Matrix to Print ****'";
                arr[45, 0] = "9	= NSMELTS = number of elements of S_ij to print (not more than 9)";
                arr[46, 0] = "11 12 21 22 31 33 44 34 43	= indices ij of elements to print";
                arr[47, 0] = "'**** Specify Scattered Directions ****'";
                arr[48, 0] = "'LFRAME' = CMDFRM (LFRAME, TFRAME for Lab Frame or Target Frame)";
                arr[49, 0] = "1 = NPLANES = number of scattering planes";
                arr[50, 0] = "0.  0. 180. 1 = phi, theta_min, theta_max (deg) for plane A";


                //写入txt文档
                string Fname = "ddcast.par";
               // string ModelName = "立方体";
                string directoryPath = @"D:\test\"+PlanArray[m,0].ToString();//定义一个路径变量
                if (!Directory.Exists(directoryPath))//如果路径不存在
                {
                    Directory.CreateDirectory(directoryPath);//创建一个路径的文件夹
                }
                monitorbox.Text += "参数文件：RH=" + Fname + "%     完成" + "\r\n";
                //保存地址为D盘自定义文件名
                //FileStream fs = new FileStream("D:\\test\\" + Fname + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(Path.Combine(directoryPath, Fname));
                //初始化二维数组s 用来接收arr
                string[,] s = new string[52, 2];
                //接收arr
                s = arr;

                for (int l = 0; l < 52; ++l)
                {
                    for (int h = 0; h < 2; ++h)
                    {
                        //s的每个值和arr的每个值对应
                        s[l, h] = arr[l, h];

                        string output;

                        output = s[l, h];
                        //有个空格作为间隔，
                        sw.Write(output + " ");
                    }
                    sw.WriteLine();
                }
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                //fs.Close();
            }
        }

        private void Parameter3()
        {
            
            string[,] arr = new string[500000, 7];//定义一个字符数组
            for (int m = 1; m < num; m++)
            {
                arr[0, 0] = "' ========== Parameter file for v7.3 ==================='";
                arr[1, 0] = "'**** Preliminaries ****' ";
                arr[2, 0] = "'NOTORQ' = CMDTRQ*6 (DOTORQ, NOTORQ) -- either do or skip torque calculations";
                arr[3, 0] = "'PBCGS2' = CMDSOL*6 (PBCGS2, PBCGST, GPBICG, QMRCCG, PETRKP) -- CCG method ";
                arr[4, 0] = "'GPFAFT' = CMETHD*6 (GPFAFT, FFTMKL) -- FFT method";
                arr[5, 0] = "'GKDLDR' = CALPHA*6 (GKDLDR, LATTDR, FLTRCD) -- DDA method ";
                arr[6, 0] = "'NOTBIN' = CBINFLAG (NOTBIN, ORIBIN, ALLBIN) -- binary output? ";
                arr[7, 0] = "'**** Initial Memory Allocation ****' ";
                arr[8, 0] = "200 200 200 = dimensioning allowance for target generation ";
                arr[9, 0] = "'**** Target Geometry and Composition ****' ";
                arr[10, 0] = "'FRMFILPBC' = CSHAPE*9 shape directive";
                arr[11, 0] = "0 0 '../shapefile/" + PlanArray[m, 0].ToString() + ".txt'";
                arr[12, 0] = "4         = NCOMP = number of dielectric materials";
                arr[13, 0] = "'../diel/Water.txt' = file with refractive index 1";
                arr[14, 0] = "'../diel/material1.txt' = file with refractive index 2";
                arr[15, 0] = "'../diel/material2.txt' = file with refractive index 3";
                arr[16, 0] = "'../diel/material3.txt' = file with refractive index 4";
                arr[17, 0] = "'**** Additional Nearfield calculation? ****' ";
                arr[18, 0] = "0 = NRFLD (=0 to skip nearfield calc., =1 to calculate nearfield E) ";
                arr[19, 0] = "0.0 0.0 0.0 0.0 0.0 0.0 (fract. extens. of calc. vol. in -x,+x,-y,+y,-z,+z)";
                arr[20, 0] = "'**** Error Tolerance ****'";
                arr[21, 0] = "1.00e-5 = TOL = MAX ALLOWED (NORM OF |G>=AC|E>-ACA|X>)/(NORM OF AC|E>)";
                arr[22, 0] = "'**** Maximum number of iterations ****'";
                arr[23, 0] = "10000     = MXITER";
                arr[24, 0] = "'**** Integration limiter for PBC calculations ****'";
                arr[24, 0] = "1.00e-2 = GAMMA (1e-2 is normal, 3e-3 for greater accuracy)";
                arr[26, 0] = "'**** Angular resolution for calculation of <cos>, etc. ****'";
                arr[27, 0] = "0.5	= ETASCA (number of angles is proportional to [(3+x)/ETASCA]^2 )";
                arr[28, 0] = "'**** Wavelengths (micron) ****'";
                arr[29, 0] = "0.55 0.55 1 'INV' = wavelengths (1st,last,howmany,how=LIN,INV,LOG,TAB)";
                arr[30, 0] = "'**** Refractive index of ambient medium ****'";
                arr[31, 0] = "1.0000 = NAMBIENT";
                arr[32, 0] = "'**** Effective Radii (micron) **** '";
                arr[33, 0] = PlanArray[m, 1].ToString();
                arr[33, 1] = PlanArray[m, 1].ToString() + " 1 'LIN' = eff. radii (1st,last,howmany,how=LIN,INV,LOG,TAB)";
                arr[34, 0] = "'**** Define Incident Polarizations ****'";
                arr[35, 0] = "(0,0) (1.,0.) (0.,0.) = Polarization state e01 (k along x axis)";
                arr[36, 0] = "2 = IORTH  (=1 to do only pol. state e01; =2 to also do orth. pol. state)";
                arr[37, 0] = "'**** Specify which output files to write ****'";
                arr[38, 0] = "1 = IWRKSC (=0 to suppress, =1 to write .sca file for each target orient.";
                arr[39, 0] = "'**** Specify Target Rotations ****'";
                arr[40, 0] = "0.    0.   1  = BETAMI, BETAMX, NBETA  (beta=rotation around a1)";
                arr[41, 0] = "0.    0.   1  = THETMI, THETMX, NTHETA (theta=angle between a1 and k)";
                arr[42, 0] = "0.    0.   1  = PHIMIN, PHIMAX, NPHI (phi=rotation angle of a1 around k)";
                arr[43, 0] = "'**** Specify first IWAV, IRAD, IORI (normally 0 0 0) ****'";
                arr[44, 0] = "0   0   0    = first IWAV, first IRAD, first IORI (0 0 0 to begin fresh)";
                arr[45, 0] = "'**** Select Elements of S_ij Matrix to Print ****'";
                arr[46, 0] = "9	= NSMELTS = number of elements of S_ij to print (not more than 9)";
                arr[47, 0] = "11 12 21 22 31 33 44 34 43	= indices ij of elements to print";
                arr[48, 0] = "'**** Specify Scattered Directions ****'";
                arr[49, 0] = "'LFRAME' = CMDFRM (LFRAME, TFRAME for Lab Frame or Target Frame)";
                arr[50, 0] = "1 = NPLANES = number of scattering planes";
                arr[51, 0] = "0.  0. 180. 1 = phi, theta_min, theta_max (deg) for plane A";


                //写入txt文档
                string Fname = "ddcast.par";
                // string ModelName = "立方体";
                string directoryPath = @"D:\test\" + PlanArray[m, 0].ToString();//定义一个路径变量
                if (!Directory.Exists(directoryPath))//如果路径不存在
                {
                    Directory.CreateDirectory(directoryPath);//创建一个路径的文件夹
                }
                monitorbox.Text += "参数文件：RH=" + Fname + "%     完成" + "\r\n";
                //保存地址为D盘自定义文件名
                //FileStream fs = new FileStream("D:\\test\\" + Fname + ".txt", FileMode.Create);
                StreamWriter sw = new StreamWriter(Path.Combine(directoryPath, Fname));
                //初始化二维数组s 用来接收arr
                string[,] s = new string[52, 2];
                //接收arr
                s = arr;

                for (int l = 0; l < 52; ++l)
                {
                    for (int h = 0; h < 2; ++h)
                    {
                        //s的每个值和arr的每个值对应
                        s[l, h] = arr[l, h];

                        string output;

                        output = s[l, h];
                        //有个空格作为间隔，
                        sw.Write(output + " ");
                    }
                    sw.WriteLine();
                }
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                //fs.Close();
            }
        }

        //折射率生成函数

        private void RI()
        {
            string[,] arr = new string[500000, 7];//定义一个字符数组
            for (int i = 4; i < 6; i++)
            {
                
                string Fname=PlanArray[1,i].ToString()+".txt" ;
                arr[0, 0] = "m="+ PlanArray[1, i].ToString();
                arr[1, 0] = "1 2 3 0 0 = columns for wave, Re(n), Im(n), eps1, eps2";
                arr[2, 0] = "   LAMBDA     Re(N)     Im(N)";
                arr[3, 0] = "  0.000001  " + PlanArray[1, i].ToString() + "  0.00000";
                arr[4, 0] = "  1.000000  " + PlanArray[1, i].ToString() + "  0.00000";
                arr[5, 0] = "  100000.0  " + PlanArray[1, i].ToString() + "  0.00000";

                string directoryPath = @"D:\test\diel\";//定义一个路径变量
                if (!Directory.Exists(directoryPath))//如果路径不存在
                {
                    Directory.CreateDirectory(directoryPath);//创建一个路径的文件夹
                }
                monitorbox.Text += "参数文件：RH=" + Fname + "%     完成" + "\r\n";
                StreamWriter sw = new StreamWriter(Path.Combine(directoryPath, Fname));
                //初始化二维数组s 用来接收arr
                string[,] s = new string[52, 2];
                //接收arr
                s = arr;

                for (int l = 0; l < 52; ++l)
                {
                    for (int h = 0; h < 2; ++h)
                    {
                        //s的每个值和arr的每个值对应
                        s[l, h] = arr[l, h];

                        string output;

                        output = s[l, h];
                        //有个空格作为间隔，
                        sw.Write(output + " ");
                    }
                    sw.WriteLine();
                }
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
            }
        }
    }
}
