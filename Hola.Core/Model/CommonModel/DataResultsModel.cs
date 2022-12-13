using System.Collections;
using System.Collections.Generic;
namespace Hola.Core.Model.CommonModel
{
    public class DataResultsModel 
    {
        public IList Items { get; set; }
        public int TotalCount { get; set; }
        public int UnreadCount { get; set; }
        public DataResultsModel()
        {

        }
        public DataResultsModel(IList list)
        {
            Items = list;
            TotalCount = list.Count;
        }
        public DataResultsModel(IList list, int total)
        {
            Items = list;
            TotalCount = total;
        }
        public static DataResultsModel ReturnValues(IList list)
        {
            return new DataResultsModel(list);
        }
    }

    public class DataModel
    {
        public IList Items { get; set; }
        public int TotalCount { get; set; }
    }
    public class DataResultsModelPageList : IDataResultsModel
    {
        public IEnumerable Items { get ; set ; }
        public int TotalCount { get ; set ; }
    }
    public interface IDataResultsModel
    {
        public IEnumerable Items { get; set; }
        public int TotalCount { get; set; }
    }

    public class PaddingResponseModel
    {
        public IList Items { get; set; }
        public int PageNumber { get; set; }
        public int PageLimit { get; set; }
        public int TotalCount { get; set; }
    }

    public class PaddingResponseModelObjectType
    {
        public object Items { get; set; }
        public int PageNumber { get; set; }
        public int PageLimit { get; set; }
        public object TotalCount { get; set; }
    }
}


public class DataResultsModelaPICrossHelper<T> where T : class
{
    public IList<T> Items { get; set; }
    public int TotalCount { get; set; }
    public int UnreadCount { get; set; }
}
