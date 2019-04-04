using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemIntegrationObligatoriskOpg
{
    public class PostNummerData
    {
        public string href { get; set; }
        public string nr { get; set; }
        public string navn { get; set; }
        public object stormodtageradresser { get; set; }
        public List<double> bbox { get; set; }
        public List<double> visueltcenter { get; set; }
        public List<Kommune> kommuner { get; set; }
        public DateTime aendret { get; set; }
        public DateTime geo_aendret { get; set; }
        public int geo_version { get; set; }
        public string dagi_id { get; set; }
    }
}