using System;
using System.Collections.Generic;
using System.Text;

namespace IASK.Common.Models
{
    public class ETInput : CheckerInput
    {
        /// <summary>
        /// Требование результата
        /// </summary>
        public bool Finalize { get; set; }
    }
}
