using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Template.macro;

namespace Template
{
    public class Template
    {

        public static int maxConsecutiveBlankLines = 2;

        private String body;

        public Template(string body) {
            this.body = body;
        }

        public String[] GetLines() {
            return body.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
        }

        public override string ToString() {
            return body;
        }

        public static Template FromFile(String file) {
            return new Template(File.ReadAllText(file));
        }

        public String Populate() {
            String[] lines = GetLines();
            foreach (KeyValuePair<string, Func<string>> pair in MacroManager.Instance().RegisteredMacros) {
                String query = "{" + pair.Key + "}";
                for (int i = 0; i < lines.Length; i++)
                    lines[i] = lines[i].Replace(query, pair.Value());
            }
            StringBuilder sb = new StringBuilder();
            int consecutiveBlankLines = 0;
            foreach (string line in lines) {
                if (line.Length == 0) {
                    consecutiveBlankLines++;
                    if(consecutiveBlankLines >= maxConsecutiveBlankLines)
                        continue;
                }else{
                    consecutiveBlankLines = 0;
                }
                sb.Append(line).Append("\n");
            }

            return sb.ToString();
        }

    }
}