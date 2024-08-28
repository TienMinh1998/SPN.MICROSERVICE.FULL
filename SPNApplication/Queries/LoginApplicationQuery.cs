using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPNApplication.Queries
{
    public class LoginApplicationQuery : IRequest<string>
    {
        public string username { get; set; }
        public string passwork { get; set; }
        public int user_id { get; set; }
    }
}
