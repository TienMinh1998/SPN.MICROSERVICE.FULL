using SPNApplication.Models;

namespace SPNApplication.Repositories
{
    public interface ICatagoryRepository
    {
        Task<bool> AddCategory(AddCategoryModel addCategory, int userid);
    }
}