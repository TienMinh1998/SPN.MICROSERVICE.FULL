
using DatabaseCore.Domain.Entities.Base;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class Permission : BaseEntity<int>
    {
        public string PermissionKey { get; set; }
        public string PermissionName { get; set; }
    }
}
