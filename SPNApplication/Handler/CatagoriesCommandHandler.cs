using MediatR;
using SPNApplication.Commnands;
using SPNApplication.Models;
using SPNApplication.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPNApplication.Handler
{
    public class CatagoriesCommandHandler : IRequestHandler<AddCatagoriesCommand, bool>
    {
        private readonly ICatagoryRepository _repository;

        public CatagoriesCommandHandler(ICatagoryRepository repository)
        {
            _repository = repository;
        }
        public Task<bool> Handle(AddCatagoriesCommand request, CancellationToken cancellationToken)
        {
            // request OK
            AddCategoryModel model = new AddCategoryModel
            {
                Define = request.Define,
                Image = request.Image,
                Name = request.Name,
            };
            return _repository.AddCategory(model, request.userid);
        }
    }
}
