
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hola.Api.Service.FilterExtension
{
    public static class FilterExtension
    {
        /// <summary>
        /// FildBy FieldName, FieldName get From ListResponse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_list_origin">List</param>
        /// <param name="inputSearch"></param>
        /// <returns>IEnumerable</returns>
        public static IEnumerable<T> FindBy<T>(this IEnumerable<T> _list_origin, IRequestFilter inputSearch)
        {
            try
            {
                var resultList = _list_origin;
                if (inputSearch.SearchFilter==null) return resultList;
                List<string> _filter = typeof(T).GetProperties().Select(x => x.Name).ToList();
                foreach (var item1 in inputSearch.SearchFilter)
                {
                    if (item1.FieldName.Contains("Date") || item1.FieldName.Contains("date"))
                    {
                        // Convert String => DateTime  
                        List<string> my_list = item1.Value.Select(date => DateTime.Parse(date).ToString()).ToList();
                        resultList = resultList.Where(x => my_list.Contains(BuildCondition(x, item1.FieldName,_filter)));
                    }
                    else
                    {
                        resultList = resultList.Where(x => item1.Value.Contains(BuildCondition(x, item1.FieldName,_filter)));
                    }
                }
                return resultList;
            }
            catch (Exception ex)
            {
                return _list_origin;
            }
        }
        public static IEnumerable<T> OrderbEx<T>(this IEnumerable<T> _list_origin, IOrderByRequest orderRequest)
        {
            var resultList = _list_origin;
            if (orderRequest.OrderByItems == null) return resultList;
            List<string> _filter = typeof(T).GetProperties().Select(x => x.Name).ToList();
            foreach (var item in orderRequest.OrderByItems)
            {
                if (item.Asc == true)
                {
                    resultList = resultList.OrderBy(x => BuildCondition(x, item.FieldName, _filter));
                }
                else
                {
                    resultList = resultList.OrderByDescending(x => BuildCondition(x, item.FieldName,_filter));
                }
            }
            return resultList;
        }
        private static string BuildCondition<T>(T generic_entity, string propertyName, List<string> _filter)
        {
            string entity_response = string.Empty;
            try
            {
                string standProperty = GetNameProperty(propertyName,_filter);
                string typeName = generic_entity.GetType().GetProperty(standProperty).PropertyType.Name;
                entity_response = generic_entity.GetType().GetProperty(standProperty).GetValue(generic_entity).ToString();
                return entity_response;
            }
            catch (System.Exception ex)
            {
                return String.Empty;
            }
        }
        private static string GetNameProperty(string value, List<string> _filter)
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
