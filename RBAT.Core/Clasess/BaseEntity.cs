using System;
using System.ComponentModel.DataAnnotations;

namespace RBAT.Core.Clasess
{
    public class BaseEntity
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        [StringLength(256)]
        public string UserName { get; set; }
    }
}
