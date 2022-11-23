using Hola.Api.DTO.Request;
using System.Collections.Generic;

namespace Hola.Api.Service.FilterExtension
{
    public interface ISearchFilterService
    {
        IEnumerable<T> FilterByListFilter<T>(IRequestFilter inputSearch,IEnumerable<T> _list_origin);

        IEnumerable<T> Orderby<T>(IEnumerable<T> _list_origin, IOrderByRequest orderByItems);
    }
}