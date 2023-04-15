using System.Collections.Generic;

namespace Hola.Api.Models.Dic
{
    public class pronunciationItem
    {
        public Dialect[] dialects { get; set; }
        public string phoneticNotation { get; set; }
        public string phoneticSpelling { get; set; }
        public string audioFile { get; set; }
    }

    public class Dialect
    {
        public string[] Dialects { get; set; }
    }

    public class ResultFromOxford
    {
        public string Id { get; set; }
        public List<ResultItemFromDictionItem> Results { get; set; }
    }

    public class ResultItemFromDictionItem
    {
        public string id { get; set; }
        public string language { get; set; }
        public List<lexicalEntrieItem> lexicalEntries { get; set; }
    }

    public class lexicalEntrieItem
    {
        public List<entrieItem> entries { get; set; }
       public LexicalCategory lexicalCategory { get; set; }
    }

    public class entrieItem
    {
       public List<pronunciationItem> pronunciations { get; set; }
        public List<SensesItem> senses { get; set; }
    }

    public class SensesItem
    {
        public List<string> definitions { get; set; }
    }

    public class LexicalCategory
    {
        public string text { get; set; }
    }
}

