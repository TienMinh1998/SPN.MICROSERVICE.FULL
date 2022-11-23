using System;

namespace Hola.Core.Model.DBModel.p2p
{
    public class P2pHistoryOffer
    {
        public int OfferId { get; set; }
        public int OldStatus { get; set; }
        public int NewStatus { get; set; }  
        public string OldNote { get; set; }
        public string NewNote { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public long OldAmount { get; set; }
        public long NewAmount { get; set; }
    }
}