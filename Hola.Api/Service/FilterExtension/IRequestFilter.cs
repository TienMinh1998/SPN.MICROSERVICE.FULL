using Hola.Api.DTO.Request;
using System.Collections.Generic;

namespace Hola.Api.Service.FilterExtension
{
 
    public interface  IRequestFilter
    {
        public List<FilterItem> SearchFilter { get; set; }
    }

    public interface IOrderByRequest
    {
        public List<OrderByItem> OrderByItems { get; set; }
    }
    public class FilterItem
    {
        public string FieldName { get; set; }
        public List<string> Value { get; set; }
    }
    public class OrderByItem
    {
        public string FieldName { get; set; }
        public bool Asc { get; set; }
    }
}
