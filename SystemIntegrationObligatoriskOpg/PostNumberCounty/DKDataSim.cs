using Data;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SystemIntegrationObligatoriskOpg
{
    public sealed class DKDataSim : SingletonBase<DKDataSim>
    {
        private Dictionary<int, string> _counties;
        private Dictionary<string, string> _cprBirthCountry;

        private DKDataSim()
        {
            var postnumberData = new List<PostNummerData>();
            var regionData = new List<KommuneData>();

            if (MakeRequest("https://dawa.aws.dk/postnumre", HTTPVerb.GET, out string postNummerData))
            {
                postnumberData = JsonManager.Instance.DesearilizeJsonCollection<PostNummerData>(postNummerData.Replace("æ", "ae"));
            }

            if (MakeRequest("https://dawa.aws.dk/kommuner", HTTPVerb.GET, out string region))
            {
                regionData = JsonManager.Instance.DesearilizeJsonCollection<KommuneData>(region.Replace("æ", "ae"));
            }

            _counties = new Dictionary<int, string>();
            _cprBirthCountry = new Dictionary<string, string>();

            _cprBirthCountry.Add("271067-1113", "Norway");
            _cprBirthCountry.Add("190202-1118", "Denmark");

            for (int i = 0; i < postnumberData.Count; i++)
            {
                _counties.Add(int.Parse(postnumberData[i].nr), regionData.Find(r => r.navn == postnumberData[i].kommuner[0].navn).region.navn);
            }
        }

        private bool MakeRequest(string targetPoint, HTTPVerb targetAction, out string data)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(targetPoint);

            request.Method = targetAction.ToString();

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                try
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        if (responseStream != null)
                        {
                            using (StreamReader reader = new StreamReader(responseStream))
                            {
                                data = reader.ReadToEnd();
                            }
                        }
                        else
                        {
                            data = "";
                            return false;
                        }
                    }
                }
                catch
                {
                    data = "";
                    return false;
                }
            }

            return true;
        }

        public string GetCounty(int postNumber)
        {
            if (!_counties.ContainsKey(postNumber))
            {
                return "Postnumber has no county";
            }

            return _counties[postNumber];
        }

        public string GetBirthCountry(string cpr)
        {
            if (!_cprBirthCountry.ContainsKey(cpr))
            {
                return "Cpr doesn't exist";
            }

            return _cprBirthCountry[cpr];
        }
    }
}