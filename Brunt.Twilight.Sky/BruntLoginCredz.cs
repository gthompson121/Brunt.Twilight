using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brunt.Twilight.Sky
{
    public class BruntLoginCredz
    {
        public string ID { get; set; }
        public string PASS { get; set; }

        public List<KeyValuePair<string, string>> GetMessageValues()
        {
            return new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("ID", ID),
                new KeyValuePair<string, string>("PASS", PASS)
            };
        }
    }
}
