using System;

namespace Hola.Api.Models.Categories
{
    public class AddCategoryModel
    {
        public string Name { get; set; }
        public string Define { get; set; }
        public string Image { get; set; }
        public int fk_userid { get; set; }
    }
}