using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolBackendAPIs.Data.Models
{
    public class ErrorResponse
    {
        public bool Error { get; set; }
        public string Message { get; set; }
        public string Field { get; set; }
    }
}
