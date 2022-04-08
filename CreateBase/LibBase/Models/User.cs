using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibBase
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public static implicit operator User(ValueTask<User> v)
        {
            throw new NotImplementedException();
        }
    }
}
