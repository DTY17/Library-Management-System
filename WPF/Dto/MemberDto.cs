using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Dto
{
    internal class MemberDto
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? NIC { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Address { get; set; }
    }
}

