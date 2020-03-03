using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHL_calc
{
    class Country
    {
        public String CountryName;
        public  int DistrictNum;
        public Country(String Name,int num)
        {
            CountryName = Name;
            DistrictNum = num;
        }
    }
}
