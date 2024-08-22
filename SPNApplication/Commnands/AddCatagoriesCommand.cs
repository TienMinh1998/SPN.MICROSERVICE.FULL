using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MediatR;


namespace SPNApplication.Commnands
{
    public class AddCatagoriesCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public string Define { get; set; }
        public string Image { get; set; }
        public int userid { get; set; }
    }

}
