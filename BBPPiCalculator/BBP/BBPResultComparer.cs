using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP
{
    /// <summary>
    /// Compare BBPResult objects to sort a list of BBPResult objects.
    /// </summary>
    public class BBPResultComparer : IComparer<BBPResult>
    {      
        public int Compare(BBPResult x, BBPResult y)
        {
            return x.Digit.CompareTo(y.Digit);
        }
    }
}
