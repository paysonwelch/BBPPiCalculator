using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBP
{
    /// <summary>       
    /// Used to generate the [n]th digit of Pi in base16 (hex). This algorithm creates
    /// a hex 'slice' that is part of the hexidecimal stream if you were to represent 
    /// Pi as a hexidecimal number.
    /// 
    /// The formulaic code was borrowed from: 
    /// http://latkin.org/blog/2012/11/03/the-bailey-borwein-plouffe-algorithm-in-c-and-f/
    /// 
    /// And the original formula by the author above was borrowed from Bailey (of BBP): 
    /// http://www.experimentalmath.info/bbp-codes/piqpr8.c
    /// 
    /// It conforms to this mathematical formula: 
    /// http://upload.wikimedia.org/math/d/5/6/d56287522ec15b6d47c9402f24a9ba89.png
    /// 
    /// A description of the algorithm can be found in this very informative whitepaper (Bailey):
    /// http://crd-legacy.lbl.gov/~dhbailey/dhbpapers/bbp-alg.pdf
    /// 
    /// Modifications:
    ///     - Converted the functional logic to a standardized class structure.
    ///     - Added more in depth commentary
    ///     - Added parallelization for certain tasks
    ///     
    /// Verified the hexidecimal output by cross referencing this whitepaper:
    /// http://crd-legacy.lbl.gov/~dhbailey/dhbpapers/bbp-alg.pdf
    /// 
    /// Pi in Hexidecimal: 
    /// http://calccrypto.wikidot.com/math:pi-hex
    /// 
    /// Hex positions for debugging
    /// 0:          243F6A8885A308D313198A2
    /// 1,000,000:  6C65E52CB459350050E4BB1
    /// 
    /// </summary>

    public class PiDigit
    {
        #region Vars
        private const int NumHexDigits = 16;                        
        private const double Epsilon = 1e-17;                        
        private const int NumTwoPowers = 25;                         
        private double[] twoPowers = new double[NumTwoPowers];       
        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public PiDigit()
        {
            this.initializeTwoPowers();
        }

        /// <summary>
        /// Calculates the [n]th hexidecimal digit of Pi.
        /// </summary>
        /// <param name="n">The digit of Pi which you wish to solve for.</param>
        /// <returns>Returns ten hexidecimal values of Pi from the offset (n).</returns>
        public string Calc(int n)
        {
            double pid, s1, s2, s3, s4;     // summations           
            string hexDigits;               // the hexidecimal digits

            // calc the summations
            s1 = Series(1, n);
            s2 = Series(4, n);
            s3 = Series(5, n);
            s4 = Series(6, n);
            
            pid = 4d * s1 - 2d * s2 - s3 - s4;          // transform the summations
            pid = pid - (int)pid + 1d;                  // create the fraction
            hexDigits = HexString(pid, NumHexDigits);   // convert the fraction to the hex digit slice
            return hexDigits.Substring(0, 10);          // return 10 hex digits starting at [n] position
        }

        /// <summary>
        /// Pre-calculate a power of two table to be used in our calculations.
        /// </summary>
        private void initializeTwoPowers()
        {
            this.twoPowers[0] = 1d;         
            Parallel.For(1, NumTwoPowers, i =>           
            {
                this.twoPowers[i] = 2d * this.twoPowers[i - 1];
            });
        }

        /// <summary>
        /// Converts the fraction to a hexidecimal string. Multiply by 16
        /// and take the whole number on each round and then convert it to 
        /// it's hex equivalent.
        /// </summary>
        /// <param name="x">The fraction of the summations.</param>
        /// <param name="numDigits">Number of digits to render to hex.</param>
        /// <returns>Returns a hex string of length numDigits</returns>
        private string HexString(double x, int numDigits)
        {
            string hexChars = "0123456789ABCDEF";
            StringBuilder sb = new StringBuilder(numDigits);
            double y = Math.Abs(x);
            for (int i = 0; i < numDigits; i++)            
            {
                y = 16d * (y - Math.Floor(y));
                sb.Append(hexChars[(int)y]);
            }
            return sb.ToString();
        }
               
        /// <summary>
        /// This routine evaluates the series  sum_k 16^(n-k)/(8*k+m)
        /// using the modular exponentiation technique.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private double Series(int m, int n)
        {
            double denom, pow, sum = 0d, term;

            // Sum the series up to n.              
            for(int k=0; k<n;k++)
            {
                denom = 8 * k + m;
                pow = n - k;
                term = ModPow16(pow, denom);
                sum = sum + term / denom;
                sum = sum - (int)sum;
            }

            //  Compute a few terms where k &gt;= n.
            for (int k = n; k <= n + 100; k++)
            {
                denom = 8 * k + m;
                term = Math.Pow(16d, (double)(n - k)) / denom;
                if (term < Epsilon)
                {
                    break;
                }
                sum = sum + term;
                sum = sum - (int)sum;
            }

            return sum;
        }

        /// <summary>
        /// Internal helper method.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private double ModPow16(double p, double m)
        {
            int i;
            double pow1, pow2, result;
            if (m == 1d)
            {
                return 0d;
            }

            // Find the greatest power of two less than or equal to p.
            for (i = 0; i < NumTwoPowers; i++)                    
            {
                if (twoPowers[i] > p)
                {
                    break;
                }
            }
            pow2 = twoPowers[i - 1];
            pow1 = p;
            result = 1d;

            // Perform binary exponentiation algorithm modulo m.                 
            for (int j = 1; j <= i; j++)
            {
                if (pow1 >= pow2)
                {
                    result = 16d * result;
                    result = result - (int)(result / m) * m;
                    pow1 = pow1 - pow2;
                }
                pow2 = 0.5 * pow2;
                if (pow2 >= 1d)
                {
                    result = result * result;
                    result = result - (int)(result / m) * m;
                }
            }

            return result;
        }
    }
}
