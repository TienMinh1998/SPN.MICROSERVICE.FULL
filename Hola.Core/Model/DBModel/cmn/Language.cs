namespace Hola.Core.Model.DBModel.cmn
{
    public class Language
    {
        public long LanguageId { get; set; }
        public string LanguageNameEng { get; set; }
        public string LanguageNameNative { get; set; } 
        public string TwoLetterISOLanguageName  { get; set; }
        public string ThreeLetterISOLanguageName { get; set; }
    }
}