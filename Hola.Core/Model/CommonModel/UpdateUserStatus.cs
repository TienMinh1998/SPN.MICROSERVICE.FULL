using System.Collections.Generic;

namespace Hola.Core.Model.CommonModel
{
    public class UpdateUserStatus
    {
        public List<string> RecipientIds { get; set; }
        public string Type { get; set; }
    }
}
