using Hola.Api.Service.FilterExtension;
using System;
using System.Collections.Generic;

namespace Hola.Api.DTO.Request
{
    public class KycWebRequest : IRequestFilter, IOrderByRequest
    {
        public string SearchText { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        // Filter by UserType, CreateDated, ConntryId
        public string OrderBy { get; set; }
        public string OrderDirection { get; set; }
        public IList<int> CountryId { get; set; }
        public IList<int> UserType { get; set; }
        public IList<int> Stage { get; set; }
        public short? Status { get; set; }
        public string[] matchedDateFilter { get; set; }
        public List<FilterItem>? SearchFilter { get; set ; }
        public List<OrderByItem>? OrderByItems { get ; set ; }
    }
}
