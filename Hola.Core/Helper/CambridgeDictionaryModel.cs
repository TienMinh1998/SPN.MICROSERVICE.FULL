using System.Collections.Generic;

namespace Hola.Core.Helper
{
    public class CambridgeDictionaryModel
    {
        public string Phonetic { get; set; }
        public string Mp3 { get; set; }
        public string Type { get; set; }
        public string Definition { get; set; }
        public string Example { get; set; }
    }

    public class CambridgeDictionaryVietNamModel
    {
        public List<string> Meaning { get; set; } = new List<string>();
    }


}