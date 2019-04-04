using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemIntegrationObligatoriskOpg
{
    public class KommuneData
    {
        public DateTime aendret { get; set; }
        public int geo_version { get; set; }
        public DateTime geo_aendret { get; set; }
        public List<double> bbox { get; set; }
        public List<double> visueltcenter { get; set; }
        public string href { get; set; }
        public string dagi_id { get; set; }
        public string kode { get; set; }
        public string navn { get; set; }
        public bool udenforkommuneinddeling { get; set; }
        public string regionskode { get; set; }
        public Region region { get; set; }
    }
}