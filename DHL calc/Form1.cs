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
            bussinessTaxText.Text = Properties.Settings.Default.businessTax;
            try
            {
                initCountryList();
                readOutDocFee();
                readOutExtra();
                readOutPackFee();
            }
            catch (Exception e)
            {
                MessageBox.Show("缺少運費檔案");
                this.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != Properties.Settings.Default.texttax)
            {
                Properties.Settings.Default.texttax = textBox2.Text;
                taxdatelabel.Text = DateTime.Now.ToString("ddd dd MMM, yyyy") + "," + DateTime.Now.ToString("HH:mm ss tt");
                Properties.Settings.Default.taxdate = taxdatelabel.Text;
                Properties.Settings.Default.businessTax = bussinessTaxText.Text;
            }
            Properties.Settings.Default.Save();
            //*************************************************************************************
            int i = Globals.countries.FindIndex(country => country.CountryName == comboBox1.Text);
            if (i == -1)
            {
                MessageBox.Show("國家輸入錯誤!");
                return;
            }

            int districtNum = Globals.countries[i].DistrictNum;

            Console.WriteLine("disNum:" + districtNum.ToString());
            double weight;
            double tax;
            double businessTax;
            bool canWeightConvert = double.TryParse(textBox1.Text, out weight);
            bool canTaxConvert = double.TryParse(textBox2.Text, out tax);
            bool canBusConvert = double.TryParse(bussinessTaxText.Text, out businessTax);
            string format = ".##";
            if (!canWeightConvert)
            {
                MessageBox.Show("請輸入重量");
                return;
            }
            if (!canTaxConvert)
            {
                MessageBox.Show("請輸入燃料稅");
                return;
            }
            if (!canBusConvert)
            {
                MessageBox.Show("請輸入營業稅");
                return;
            }
            tax = tax / 100 + 1;
            businessTax = businessTax / 100 + 1;
            double fee = 0;
            if (comboBox2.Text == "郵件")
            {
                double targetWeight = Math.Round(weight, 2, MidpointRounding.AwayFromZero);
                Fee shippingFeeList = Globals.outPackFee.Find(packFee => packFee.weight == targetWeight);
                if (shippingFeeList == null)
                {
                    label4.Text = "0";
                    return;
                }
                double targetShippingFee = shippingFeeList.price[districtNum - 1];
                fee = targetShippingFee * tax * businessTax;
                Console.WriteLine("DocFee: " + targetShippingFee.ToString());
                label4.Text = fee.ToString(format);
            }
            else if (comboBox2.Text == "包裹")
            {
                Console.WriteLine("<=" + Globals.outPackFee[Globals.outPackFee.Count - 1].weight.ToString());
                if (weight <= Globals.outPackFee[Globals.outPackFee.Count - 1].weight)
                {
                    double targetWeight = Math.Round(weight, 2, MidpointRounding.AwayFromZero);
                    Fee shippingFeeList = Globals.outPackFee.Find(packFee => packFee.weight == targetWeight);
                    if (shippingFeeList == null)
                    {
                        label4.Text = "0";
                        return;
                    }
                    double targetShippingFee = shippingFeeList.price[districtNum - 1];
                    fee = targetShippingFee * tax * businessTax;
                    Console.WriteLine("PackFee: " + targetShippingFee.ToString());
                    label4.Text = fee.ToString(format);
                }
                else
                {
                    ExtraFee targetFeeList = Globals.outExtra.Find(packFee => packFee.minWeight <= weight && packFee.maxWeight >= weight);
                    Console.WriteLine(targetFeeList.minWeight.ToString() + " <= " + targetFeeList.maxWeight.ToString());
                    int kg = (int)weight;
                    if (weight - (int)weight > 0)
                    {
                        kg = (int)weight + 1;
                    }
                    double targetFee = targetFeeList.price[districtNum - 1];
                    fee = kg * targetFee * tax * businessTax;
                    Console.WriteLine("PackFee: " + targetFee.ToString());
                    label4.Text = fee.ToString(format);
                }
            }
        }

        private void initCountryList()
        {
            Globals.countries = new List<Country>();
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader("./國家分區.csv");
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
            System.IO.StreamReader file = new System.IO.StreamReader("./出口文件.csv");
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
            System.IO.StreamReader file = new System.IO.StreamReader("./出口包裹.csv");
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
            System.IO.StreamReader file = new System.IO.StreamReader("./出口30.csv");
            String line = "";
            Globals.outExtra = new List<ExtraFee>();
            while ((line = file.ReadLine()) != null)
            {
                line = line.Replace(",,", ",");
                Globals.outExtra.Add(new ExtraFee(line.Split(',').ToList()));
            }
            file.Close();
        }
        static class Globals
        {
            public static List<Country> countries;
            public static List<Fee> outDocFee;
            public static List<Fee> outPackFee;
            public static List<ExtraFee> outExtra;
        }
    }
}
