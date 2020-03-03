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
    static class Globals
    {
        public static List<Country> countries;
        public static List<Fee> outDocFee;
        public static List<Fee> outPackFee;
        public static List<ExtraFee> outExtra;
    }

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            taxdatelabel.Text = Properties.Settings.Default.taxdate;
            textBox2.Text = Properties.Settings.Default.texttax;
            initCountryList();
            readOutDocFee();
            readOutExtra();
            readOutPackFee();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != Properties.Settings.Default.texttax)
            {
                Properties.Settings.Default.texttax = textBox2.Text;
                taxdatelabel.Text = DateTime.Now.ToString("ddd dd MMM, yyyy") + "," + DateTime.Now.ToString("HH:mm ss tt");
                Properties.Settings.Default.taxdate = taxdatelabel.Text;
            }

            Properties.Settings.Default.Save();
            //*************************************************************************************
            int exist = 0;
            for (int i = 0; i < Globals.countries.Count; i++)
            {
                int districtNum = Globals.countries[i].DistrictNum;
                if (comboBox1.Text == Globals.countries[i].CountryName)
                {
                    exist = 1;
                    float weight = Convert.ToSingle(textBox1.Text);
                    float tax = Convert.ToSingle(textBox2.Text) / 100 + 1;
                    float fee = 0;
                    if (comboBox2.Text == "郵件")
                    {
                        for (int j = 0; j < Globals.outDocFee.Count; j++)
                        {
                            if (weight - Globals.outDocFee[j].weight < 0.5)
                            {
                                fee = Globals.outDocFee[j].price[districtNum - 1] * tax * (float)1.05;
                                label4.Text = Convert.ToString(fee);
                                break;
                            }
                        }
                    }
                    else if (comboBox2.Text == "包裹")
                    {
                        if (weight <= 30)
                        {
                            for (int j = 0; j < Globals.outPackFee.Count; j++)
                            {
                                if (weight - Globals.outPackFee[j].weight < 0.5 && weight - Globals.outPackFee[j].weight != 0)
                                {
                                    fee = Globals.outPackFee[j].price[districtNum - 1] * tax * (float)1.05;
                                    label4.Text = Convert.ToString(fee);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < Globals.outExtra.Count; j++)
                            {
                                if (weight >= Globals.outExtra[j].minWeight && weight <= Globals.outExtra[j].maxWeight)
                                {
                                    int kg = (int)weight;
                                    if (weight - (int)weight > 0)
                                    {
                                        kg = (int)weight + 1;
                                    }
                                    fee = kg * Globals.outExtra[j].price[districtNum - 1] * tax * (float)1.05;
                                    label4.Text = Convert.ToString(fee);
                                    break;
                                }
                            }
                        }
                    }
                    break;
                }
                else
                {
                    exist = 0;
                }
            }
            if (exist == 0)
            {
                MessageBox.Show("國家輸入錯誤!");
            }
        }

        private void initCountryList()
        {
            Globals.countries = new List<Country>();
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("國家分區.csv");
            while ((line = file.ReadLine()) != null)
            {
                List<String> countryInfo = line.Split(',').ToList();
                int num = Convert.ToInt32(countryInfo[1]);
                Globals.countries.Add(new Country(countryInfo[0], num));
            }
            file.Close();
            for (int i = 0; i < Globals.countries.Count; i++)
            {
                comboBox1.Items.Add(Globals.countries[i].CountryName);
            }
        }
        private void readOutDocFee()
        {
            System.IO.StreamReader file = new System.IO.StreamReader("出口文件.csv");
            String line = "";
            Globals.outDocFee = new List<Fee>();
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(",,", ",");
                Globals.outDocFee.Add(new Fee(line.Split(',').ToList()));
            }
            file.Close();
        }
        private void readOutPackFee()
        {
            System.IO.StreamReader file = new System.IO.StreamReader("出口包裹.csv");
            String line = "";
            Globals.outPackFee = new List<Fee>();
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(",,", ",");
                Globals.outPackFee.Add(new Fee(line.Split(',').ToList()));
            }
            file.Close();
        }
        private void readOutExtra()
        {
            System.IO.StreamReader file = new System.IO.StreamReader("出口30.csv");
            String line = "";
            Globals.outExtra = new List<ExtraFee>();
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(",,", ",");
                Globals.outExtra.Add(new ExtraFee(line.Split(',').ToList()));
            }
            file.Close();
        }
    }
}
