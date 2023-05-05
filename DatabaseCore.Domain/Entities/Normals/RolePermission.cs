using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseCore.Domain.Entities.Base;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class RolePermission : BaseEntity<int>
    {
        public int FK_PermissionID { get; set; }
        public int FK_RoleID { get; set; }
    }
}
