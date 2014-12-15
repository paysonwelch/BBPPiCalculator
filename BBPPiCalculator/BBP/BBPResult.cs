using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP
{
    /// <summary>
    /// Simple structure for passing back the result and it's position.
    /// This is used to maintain the correct order of the result during
    /// parallel calculations.
    /// </summary>
    public class BBPResult
    {
        public int Digit { get; set; }
        public string HexDigits { get; set; }
    }
}
