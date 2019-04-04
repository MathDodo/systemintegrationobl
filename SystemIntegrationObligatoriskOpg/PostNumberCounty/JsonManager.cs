using Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace SystemIntegrationObligatoriskOpg
{
    public sealed class JsonManager : SingletonBase<JsonManager>
    {
        private readonly JsonSerializerSettings _defaultSerializerSettings =
          new JsonSerializerSettings
          {
              TypeNameHandling = TypeNameHandling.Objects
          };

        private JsonManager()
        {
        }

        public List<T> DesearilizeJsonCollection<T>(string jsonData)
        {
            JArray array = (JArray)JsonConvert.DeserializeObject(jsonData, _defaultSerializerSettings);
            return array.ToObject<List<T>>();
        }
    }
}