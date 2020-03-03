using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHL_calc.Properties;
namespace DHL_calc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            taxdatelabel.Text = Properties.Settings.Default.taxdate;
            textBox2.Text = Properties.Settings.Default.texttax;
            Country[] country = new Country[234];
            string line;
            int counter = 0;
            System.IO.StreamReader file = new System.IO.StreamReader("國家分區.csv");
            while ((line = file.ReadLine()) != null)
            {
                string temp = "";
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == ',')
                    {
                        break;
                    }
                    else
                    {
                        temp += line[i];
                    }
                }
                line = line.Replace(temp + ",", "");
                int num = Convert.ToInt32(line);
                country[counter] = new Country(temp, num);
                counter++;
            }
            file.Close();
           
            for (int i = 0; i <counter; i++)
            {
                comboBox1.Items.Add(country[i].CountryName);
            }
            counter = 0;
        }




        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
       
private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != Properties.Settings.Default.texttax)
            {
                Properties.Settings.Default.texttax = textBox2.Text;
                taxdatelabel.Text = DateTime.Now.ToString("ddd dd MMM, yyyy") +","+DateTime.Now.ToString("HH:mm ss tt") ;
                Properties.Settings.Default.taxdate = taxdatelabel.Text;
            }

            Properties.Settings.Default.Save();
            //**********************************************************************************
            Country[] country = new Country[234];
            string line;
            int counter = 0;
            System.IO.StreamReader file = new System.IO.StreamReader("國家分區.csv");
            while ((line = file.ReadLine()) != null)
            {
                string temp = "";
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == ',')
                    {
                        break;
                    }
                    else
                    {
                        temp += line[i];
                    }
                }
                line = line.Replace(temp + ",", "");
                int num = Convert.ToInt32(line);
                country[counter] = new Country(temp, num);
                counter++;
            }
            file.Close();
            counter = 0;
            
            //**********************************************************************************
            float[,] outDocFee = new float[4, 11];
            file = new System.IO.StreamReader("出口文件.csv");
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(",,", ",");
                int flag = 0;
                string temp = "";
                int counter2 = 1;
                for (int i = 0; i < line.Length; i++)
                {

                    if (line[i] != ',')
                    {
                        temp += line[i];
                    }
                    if (i == line.Length - 1)
                    {
                        flag++;
                        outDocFee[counter, counter2] = Convert.ToSingle(temp);
                        // Console.WriteLine(outDocFee[counter, counter2]);
                        temp = "";
                        counter2++;
                    }
                    else if (flag != 0 && line[i] == ',')
                    {
                        flag++;
                        outDocFee[counter, counter2] = Convert.ToSingle(temp);
                        //  Console.WriteLine(outDocFee[counter, counter2]);
                        temp = "";
                        counter2++;
                    }
                    else if (flag == 0 && line[i] == ',')
                    {
                        flag++;
                        outDocFee[counter, 0] = Convert.ToSingle(temp);
                        // Console.WriteLine(outDocFee[counter, 0]);
                        temp = "";
                    }
                }

                counter++;
            }
            file.Close();
            counter = 0;
            //**********************************************************************************
            float[,] outPackFee = new float[60, 11];
            file = new System.IO.StreamReader("出口包裹.csv");
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(",,", ",");
                int flag = 0;
                string temp = "";
                int counter2 = 1;
                for (int i = 0; i < line.Length; i++)
                {

                    if (line[i] != ',')
                    {
                        temp += line[i];
                    }
                    if (i == line.Length - 1)
                    {
                        flag++;
                        outPackFee[counter, counter2] = Convert.ToSingle(temp);
                        //  Console.WriteLine(outPackFee[counter, counter2]);
                        temp = "";
                        counter2++;
                    }
                    else if (flag != 0 && line[i] == ',')
                    {
                        flag++;
                        outPackFee[counter, counter2] = Convert.ToSingle(temp);
                        // Console.WriteLine(outPackFee[counter, counter2]);
                        temp = "";
                        counter2++;
                    }
                    else if (flag == 0 && line[i] == ',')
                    {
                        flag++;
                        outPackFee[counter, 0] = Convert.ToSingle(temp);
                        //  Console.WriteLine(outPackFee[counter, 0]);
                        temp = "";
                    }
                }
                counter++;
            }
            file.Close();
            counter = 0;
            //*************************************************************************************
            float[,] outextra = new float[3, 12];
            file = new System.IO.StreamReader("出口30.csv");
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(",,", ",");
                int flag = 0;
                string temp = "";
                int counter2 = 1;
                for (int i = 0; i < line.Length; i++)
                {

                    if (line[i] != ',')
                    {
                        temp += line[i];
                    }
                    if (i == line.Length - 1)
                    {
                        flag++;
                        outextra[counter, counter2] = Convert.ToSingle(temp);
                        //  Console.WriteLine(outextra[counter, counter2]);
                        temp = "";
                        counter2++;
                    }
                    else if (flag != 0 && line[i] == ',')
                    {
                        flag++;
                        outextra[counter, counter2] = Convert.ToSingle(temp);
                        // Console.WriteLine(outextra[counter, counter2]);
                        temp = "";
                        counter2++;
                    }
                    else if (flag == 0 && line[i] == ',')
                    {
                        flag++;
                        outextra[counter, 0] = Convert.ToSingle(temp);
                        //  Console.WriteLine(outextra[counter, 0]);
                        temp = "";
                    }
                }
                counter++;
            }
            file.Close();
            counter = 0;

            //*************************************************************************************
            int exist = 0;
                for (int i=0;i<234;i++)
            {
                if(comboBox1.Text==country[i].CountryName)
                {
                    exist = 1;
                    float weight = Convert.ToSingle(textBox1.Text);
                    float tax = Convert.ToSingle(textBox2.Text)/100+1;
                    float fee = 0;
                   if(comboBox2.Text=="郵件")
                    {
                        for(int j=0;j<4;j++)
                        {
                            if((weight-outDocFee[j,0]<0.5))
                            {
                                fee = outDocFee[j, country[i].DistrictNum]*tax*(float)1.05;
                                label4.Text = Convert.ToString(fee);
                                break;
                            }
                            
                        }

                    }
                   else if(comboBox2.Text=="包裹")
                    {
                        if(weight<=30)
                        {
                            for(int j=0;j<60;j++)
                            {
                                if ((weight - outPackFee[j, 0] < 0.5)&& weight - outPackFee[j, 0]!=0)
                                {

                                    fee = outPackFee[j+1, country[i].DistrictNum] * tax* (float)1.05;
                                    label4.Text = Convert.ToString(fee);
                                    break;
                                }
                                else
                                {
                                    if ((weight - outPackFee[j, 0] < 0.5) && weight - outPackFee[j, 0] == 0)
                                    {

                                        fee = outPackFee[j , country[i].DistrictNum] * tax * (float)1.05;
                                        label4.Text = Convert.ToString(fee);
                                        break;
                                    }
                                }
                            }
                        }
                        else if(weight>=30.1&&weight<=70)
                        {
                            int kg = (int)weight;
                            if (weight - (int)weight > 0)
                            {
                                kg = (int)weight + 1;
                            }
                            fee = kg * outextra[0, country[i].DistrictNum+1] * tax* (float)1.05;
                                label4.Text = Convert.ToString(fee);
                                break;
                            
                        }
                        else if (weight >= 70.1 && weight <= 300)
                        {
                            int kg = (int)weight;
                            if (weight - (int)weight > 0)
                            {
                                kg = (int)weight + 1;
                            }
                            fee = kg * outextra[1, country[i].DistrictNum+1] * tax* (float)1.05;
                            label4.Text = Convert.ToString(fee);
                                break;
                            
                        }
                        else if (weight >= 300.1 && weight <= 99999)
                        {
                            int kg = (int)weight;
                            if (weight-(int)weight>0)
                            {
                                kg = (int)weight+1;
                            }
                                fee = kg*outextra[2, country[i].DistrictNum+1] * tax* (float)1.05;
                            label4.Text = Convert.ToString(fee);
                            
                            break;

                        }
                    }
                    break;
                }
              else
                {
                    exist = 0;
                }

            }
           
         if(exist==0)
            {
                MessageBox.Show("國家輸入錯誤!");
            }
        }

       
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }
    }
}
