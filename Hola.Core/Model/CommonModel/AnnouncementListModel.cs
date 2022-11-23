using System.Collections.Generic;

namespace Hola.Core.Model.CommonModel
{
    public class AnnouncementListModel
    {
        public List<AnnouncementModel> AllAnnouncement { get; set; }
        public int TotalCount { get; set; }
    }
}
