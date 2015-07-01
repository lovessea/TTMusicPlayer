using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Weather
{
    /// <summary>
    /// 省
    /// </summary>
    public class Province
    {
        public string Name { get; set; }

        public List<City> Citys { get; set; }
    }
}
