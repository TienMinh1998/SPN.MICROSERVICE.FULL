using System;

namespace Hola.Api.Models
{
    public class HistoryModel
    {
        public int Id { get; set; }
        public int fk_userid { get; set; }
        public int count_word { get; set; }
        public double percent_day { get; set; }
        public string Note { get; set; }
        public DateTime created_on { get; set; }
    }
}
