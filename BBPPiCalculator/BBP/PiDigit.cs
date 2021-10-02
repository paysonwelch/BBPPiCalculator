using System.Text;

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

    public class PiDigit : IBBPCalculator
    {
        #region Vars
        private const int NumHexDigits = 16;
        private const double Epsilon = 1e-17;
        private const int NumTwoPowers = 25;
        private static readonly double[] TwoPowers;
        private long nOffset = 0;
        #endregion

        static PiDigit()
        {
            // Pre-calculate a power of two table to be used in our calculations.
            var twoPowers = new double[NumTwoPowers];
            twoPowers[0] = 1d;
            Parallel.For(1, NumTwoPowers, i =>
            {
                twoPowers[i] = 2d * twoPowers[i - 1];
            });
            TwoPowers = twoPowers;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public PiDigit(long nOffset = 0)
        {
            if (nOffset < 0)
            {
                throw new ArgumentException(nameof(nOffset));
            }
            this.nOffset = nOffset;
        }

        public async IAsyncEnumerable<char> PiBytesAsync(long n = -1, int count = 10)
        {
            if (n == -1)
            {
                n = this.nOffset;
            } else
            {
                this.nOffset = n;
            }
            long offset = 0;
            int remaining = count;

            while (remaining > 0)
            {
                var result = Calculate(n + offset);
                offset += result.HexDigits.Length;
                for (int i = 0; i < result.HexDigits.Length; i++)
                {
                    this.nOffset = n + offset + i + 1;
                    yield return result.HexDigits[i];
                    if (remaining-- == 0)
                    {
                        yield break;
                    }
                }
            }
        }

        public IEnumerable<char> PiBytes(long n = -1, int count = 10)
        {
            if (n == -1)
            {
                n = this.nOffset;
            } else
            {
                this.nOffset = n;
            }

            long offset = 0;
            int remaining = count;

            while (remaining > 0)
            {
                var result = PiDigit.Calculate(n + offset);
                offset += result.HexDigits.Length;
                for (int i = 0; i < result.HexDigits.Length; i++)
                {
                    this.nOffset = n + offset + i + 1;
                    yield return result.HexDigits[i];
                    if (remaining-- == 0)
                    {
                        yield break;
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the [n]th hexidecimal digit of Pi.
        /// </summary>
        /// <param name="n">The digit of Pi which you wish to solve for.</param>
        /// <returns>Returns ten hexidecimal values of Pi from the offset (n).</returns>
        public static BBPResult Calculate(long n)
        {
            if (n < 0)
            {
                throw new ArgumentException(nameof(n));
            }

            double pid, s1, s2, s3, s4;     // summations           
            string hexDigits;               // the hexidecimal digits

            // calc the summations
            s1 = Series(1, n);
            s2 = Series(4, n);
            s3 = Series(5, n);
            s4 = Series(6, n);

            pid = 4d * s1 - 2d * s2 - s3 - s4;          // transform the summations
            pid = pid - (long)pid + 1d;                  // create the fraction
            hexDigits = HexString(pid, NumHexDigits);   // convert the fraction to the hex digit slice      

            return new BBPResult() { Digit = n, HexDigits = hexDigits.Substring(0, 10) };
        }

        /// <summary>
        /// Calculates the [n]th hexidecimal digit of Pi.
        /// </summary>
        /// <param name="n">The digit of Pi which you wish to solve for.</param>
        /// <returns>Returns ten hexidecimal values of Pi from the offset (n).</returns>
        public BBPResult Calc(long n)
        {
            this.nOffset = n + 1;
            return PiDigit.Calculate(n: n);
        }

        public BBPResult Next() =>
            this.Calc(this.nOffset);

        /// <summary>
        /// Converts the fraction to a hexidecimal string. Multiply by 16
        /// and take the whole number on each round and then convert it to 
        /// it's hex equivalent.
        /// </summary>
        /// <param name="x">The fraction of the summations.</param>
        /// <param name="numDigits">Number of digits to render to hex.</param>
        /// <returns>Returns a hex string of length numDigits</returns>
        private static string HexString(double x, int numDigits)
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
        private static double Series(long m, long n)
        {
            double denom, pow, sum = 0d, term;

            // Sum the series up to n.              
            for (long k = 0; k < n; k++)
            {
                denom = 8 * k + m;
                pow = n - k;
                term = ModPow16(pow, denom);
                sum = sum + term / denom;
                sum = sum - (long)sum;
            }

            //  Compute a few terms where k &gt;= n.
            for (long k = n; k <= n + 100; k++)
            {
                denom = 8 * k + m;
                term = Math.Pow(16d, (double)(n - k)) / denom;
                if (term < Epsilon)
                {
                    break;
                }
                sum = sum + term;
                sum = sum - (long)sum;
            }

            return sum;
        }

        /// <summary>
        /// Internal helper method.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private static double ModPow16(double p, double m)
        {
            long i;
            double pow1, pow2, result;
            if (m == 1d)
            {
                return 0d;
            }

            // Find the greatest power of two less than or equal to p.
            for (i = 0; i < NumTwoPowers; i++)
            {
                if (TwoPowers[i] > p)
                {
                    break;
                }
            }
            pow2 = TwoPowers[i - 1];
            pow1 = p;
            result = 1d;

            // Perform binary exponentiation algorithm modulo m.                 
            for (long j = 1; j <= i; j++)
            {
                if (pow1 >= pow2)
                {
                    result = 16d * result;
                    result = result - (long)(result / m) * m;
                    pow1 = pow1 - pow2;
                }
                pow2 = 0.5 * pow2;
                if (pow2 >= 1d)
                {
                    result = result * result;
                    result = result - (long)(result / m) * m;
                }
            }

            return result;
        }
    }
}
