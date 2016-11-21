using System.IO;
using System.Reflection;

namespace imgbruh.Models.NameGeneration
{
    public class StringArrayHelper
    {
        internal const string FileFolderNamespace = "imgbruh.Models.NameGeneration.Files";

        public static string[] GetFromSingleColumnedCsv(string fileName)
        {
            var @namespace = FileFolderNamespace + "." + fileName;
            var stringArray = new string[] { };
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(@namespace))
            using (var reader = new StreamReader(stream))
            {
                string csv = reader.ReadToEnd();
                stringArray = csv.Split(new string[] { "\r\n" }, System.StringSplitOptions.None);
                // do something with the CSV
            }
            return stringArray;
        }
    }
}