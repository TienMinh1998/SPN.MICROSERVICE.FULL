using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class UserManual
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Pk_UserManual_Id { get; set; }
        public int Fk_Grannar_Id { get; set; }
        public string Used { get; set; }          // Cách sử dụng
        public string Example { get; set; }        // Example :  Ví dụ cho cách sử dụng
        public string DetailExample { get; set; }  // DetailExample Giải thích chi tiết cách sử dụng Example
    }
}
