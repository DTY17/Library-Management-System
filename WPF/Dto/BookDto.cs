using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Dto
{
    internal class BookDto
    {
        public BookDto() { }
        public string Id { get; set; }
        public string Book { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public string Available { get; set; }
    }
}
