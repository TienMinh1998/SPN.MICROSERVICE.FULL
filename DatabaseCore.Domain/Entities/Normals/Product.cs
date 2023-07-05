using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCore.Domain.Entities.Normals
{
    public class Product
    {
        public int Id { get; set; }               // id sản phẩm
        public string Name { get; set; }          // tên sản phẩm
        public string Description { get; set; }   // mô tả sản phẩm
        public double Price { get; set; }         // giá sản phẩm
        public string ImageUrl { get; set; }      // ảnh sản phẩm
        public DateTime CreatedDate { get; set; } // Ngày tạo
        public bool IsDelete { get; set; }         // trạng thái đã xóa chưa
        public int Type { get; set; }              // Loại sản phẩm
    }
}
