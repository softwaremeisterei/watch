using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lib
{
    /// <summary>
    /// Parse arguments (Syntax: -arg=value --arg=value tail tail tail)
    /// </summary>
    public class Arguments
    {
        private readonly Regex OptPattern = new Regex(@"--?(?'Opt'[a-zA-Z]+)=(?'Val'.*)");
        private Dictionary<string, string> dic = new Dictionary<string, string>();
        private List<string> parameters = new List<string>();

        public Arguments(string[] args)
        {
            Parse(args);
        }

        public string Consume(string opt)
        {
            string result = null;
            string OPT = opt.ToLower();

            if (dic.ContainsKey(OPT))
            {
                result = dic[OPT];
                dic.Remove(OPT);
            }

            return result;
        }

        public string[] Opts()
        {
            return dic.Keys.ToList().ToArray();
        }

        public string[] Tail()
        {
            return parameters.ToArray();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var keyValue in dic)
            {
                sb.AppendLine(string.Format("-{0}={1}", keyValue.Key, keyValue.Value));
            }

            if (parameters.Any())
            {
                sb.AppendLine(string.Format("Params: {0}", string.Join(" ", parameters)));
            }

            return sb.ToString();
        }

        private void Parse(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];

                if (OptPattern.IsMatch(arg))
                {
                    var match = OptPattern.Match(arg);
                    var opt = match.Groups["Opt"].Value;
                    var value = match.Groups["Val"].Value;
                    dic.Add(opt.ToLower(), value);
                }
                else
                {
                    parameters.Add(arg);
                }
            }
        }
    }
}
