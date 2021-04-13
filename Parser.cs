using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace client_marieteam
{
    class Parser
    {
        public static List<string> Parsing(string res)
        {
            string parser = "#NEWPAGE";
            List<string> listparsed = new List<string>();
            while (res.Contains(parser))
            {
                int idx = res.IndexOf(parser);
                listparsed.Add(res.Substring(0, idx));
                res = res.Substring(idx+ parser.Length);
            }

            return listparsed;
        }
    }
}
