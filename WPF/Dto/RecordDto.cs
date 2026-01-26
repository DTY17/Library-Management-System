using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF.Dto
{
    internal class RecordDto
    {
        public int? Id { get; set; }
        public string? Member { get; set; }
        public string? Book { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; } 
        public string? Status { get; set; }
    }
}
