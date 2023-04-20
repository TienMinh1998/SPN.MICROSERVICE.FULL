using System.Collections.Generic;

namespace Hola.Api.Models.Dic
{
    public class pronunciationItem
    {
        public string audioFile { get; set; }
        public string phoneticSpelling { get; set; }
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
        public List<synonymsItem> synonyms { get; set; }
    }

    public class LexicalCategory
    {
        public string text { get; set; }
    }

    public class synonymsItem
    {
        public string text { get; set; }
        public string language { get; set; }
    }

}

