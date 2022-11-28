using DatabaseCore.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class Target :BaseEntity<int>           // CreateDate, IsDeleted
    {
        public int UserId { get; set; }             // Xem là mục tiêu của người nào
        public string Content { get; set; }         // Tên của mục tiêu 
        public DateTime StartDate { get; set; }     // Ngày bắt đầu của mục tiêu
        public int TotalDays { get; set; }
    }
}
