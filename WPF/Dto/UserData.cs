using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Dto
{
    internal class UserData
    {
        private static UserData _instance;
        private string role;
        private UserData() { }

        public static UserData Instance()
        {
            if(_instance == null)
            {
                UserData._instance = new UserData();
            }
            return _instance;
        }

        public string Role { get; set; }
    }
}
