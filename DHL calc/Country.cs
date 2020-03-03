﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHL_calc
{
    class Country
    {
        public String CountryName;
        public int DistrictNum;
        public Country(String Name, int num)
        {
            CountryName = Name;
            DistrictNum = num;
        }
    }
    class Fee
    {
        public double weight;
        public List<double> price = new List<double>();
        public Fee(List<String> input)
        {
            weight = Convert.ToSingle(input[0]);

            for (int i = 1; i < input.Count; i++)
            {
                price.Add(Convert.ToSingle(input[i]));
            }
        }
    }
    class ExtraFee
    {
        public double maxWeight,minWeight;
        public List<double> price = new List<double>();
        public ExtraFee(List<String> input)
        {
            minWeight = Convert.ToSingle(input[0]);
            maxWeight = Convert.ToSingle(input[1]);
            for (int i = 2; i < input.Count; i++)
            {
                price.Add(Convert.ToSingle(input[i]));
            }
        }

    }

}
