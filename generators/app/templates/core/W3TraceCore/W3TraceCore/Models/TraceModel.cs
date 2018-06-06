using System;
using System.IO;
using System.Xml;

namespace W3TraceCore.Models
{
    public class TraceModel
    {
        private static string root { get; set; }
        public string url { get; set; }
        public string tag { get; set; }
        public int debug { get; set; }
        public string path { get; set; }
        public int envio { get; set; }
        public int dias { get; set; }

        public static void SetRoot(string r)
        {
            root = r;
        }
        public static string GetRoot()
        {
            return root;
        }

        public TraceModel()
        {
            string path = Path.Combine(GetRoot(), "TRACE.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(path);
            XmlElement root = doc.DocumentElement;
            this.url = root.Attributes["url"] != null? root.Attributes["url"].Value : null;
            this.tag = root.Attributes["tag"] != null ? root.Attributes["tag"].Value : null;
            this.debug = root.Attributes["debug"] != null ? Convert.ToInt32(root.Attributes["debug"].Value) : 0;
            this.path = root.Attributes["path"] != null ? root.Attributes["path"].Value : null;
            this.envio = root.Attributes["envio"] != null ? Convert.ToInt32(root.Attributes["envio"].Value) : 0;
            this.dias = root.Attributes["dias"] != null ? Convert.ToInt32(root.Attributes["dias"].Value) : 0;
        }
    }
}