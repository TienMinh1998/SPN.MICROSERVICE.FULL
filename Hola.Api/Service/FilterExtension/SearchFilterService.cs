using Hola.Api.DTO.Request;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hola.Api.Service.FilterExtension
{
    public class SearchFilterService : ISearchFilterService
    {
        private List<string> _filter; // Chưa chuyển về viết thường
        public IEnumerable<T> FilterByListFilter<T>(IRequestFilter inputSearch, IEnumerable<T> _list_origin)
        {
            try
            {
                var resultList = _list_origin;
                List<string> _filter  = typeof(T).GetProperties().Select(x => x.Name).ToList();
                foreach (var item1 in inputSearch.SearchFilter)
                {
                    if (item1.FieldName.Contains("Date") || item1.FieldName.Contains("date"))
                    {
                        // Convert String => DateTime  
                        List<string> my_list = item1.Value.Select(date => DateTime.Parse(date).ToString()).ToList();
                        resultList = resultList.Where(x => my_list.Contains(BuildCondition(x, item1.FieldName)));
                    }
                    else
                    {
                        resultList = resultList.Where(x => item1.Value.Contains(BuildCondition(x, item1.FieldName)));
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                return _list_origin;
            }
          
        }
        public IEnumerable<T> Orderby<T>(IEnumerable<T> _list_origin, IOrderByRequest orderRequest)
        {
            var resultList = _list_origin;
            _filter = typeof(T).GetProperties().Select(x => x.Name).ToList();
            foreach (var item in orderRequest.OrderByItems)
            {
                if (item.Asc==true)
                {
                    resultList = resultList.OrderBy(x => BuildCondition(x, item.FieldName));
                }
                else
                {
                    resultList = resultList.OrderByDescending(x => BuildCondition(x, item.FieldName));
                }
            }
            return resultList;
        }
        private string BuildCondition<T>(T generic_entity, string propertyName)
        {
            string entity_response = string.Empty;
            try
            {
                string standProperty = GetNameProperty(propertyName);
                string typeName = generic_entity.GetType().GetProperty(standProperty).PropertyType.Name;
                entity_response = generic_entity.GetType().GetProperty(standProperty).GetValue(generic_entity).ToString();
                return entity_response;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
        private string GetNameProperty(string value)
        {
            foreach (var item in _filter)
            {
                if (value.ToLower() == item.ToLower())
                {
                    return item;
                }
            }
            return string.Empty;
        }
    }


}
