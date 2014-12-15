﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BBP;

/******************************************************************************
 * BBP Pi Calculator
 * 2014.12.14
 * Payson Welch; paysonwelch.com, freeideas.org, github.com/paysonwelch
 * paysonwelch@gmail.com
 * 
 * Abstract
 * --------------------------
 * This application is a proof of concept to demonstrate the capability of the 
 * Bailey–Borwein–Plouffe formula which can be used to solve for [n]th binary 
 * digit in base16 of pi.
 * http://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula
 * 
 * Executive Summary
 * --------------------------------------------------------
 * Originally I was considering using the BBP formula to create a new Proof of
 * Work (POW) system for crypto currencies. With the advent of Proof of Stake
 * however and other anonymizing features of currencies the BBP-Pi POW system
 * would not be enough in itself to create a highly successful crypto currency.
 * 
 * A BBP-Pi POW currency would be more of a novelty of interest to reserachers
 * mostly. There is also PrimeCoin which uses a POW system for hunting chains
 * of prime numbers. So although this is an interesting idea it needs more
 * thought before simply creating a POW based on BBP.
 * 
 * This applicaiton has been structured in such a way that it could easily be
 * modified for grid computing. This application currently will not focus on 
 * grid payload distribution however, but will focus on breaking work items
 * into small specific units which can be easily inserted into a grid pipeline
 * in the future.
 * http://en.wikipedia.org/wiki/Primecoin
 *  
 * Bailey–Borwein–Plouffe 
 * --------------------------------------------------------
 * The BBP formula was accidentally discovered in 1995 but it changed the 
 * landscape of our understanding of how to calculate certain numbers such
 * as Pi and e. It uses a series of summations to calculate a hexidecimal 
 * stream of the floating point portion of Pi.
 * 
 * Compatibility and Crypto Currency Considerations
 * --------------------------------------------------------
 * This applicaiton is not compatible with large numbers, so be cautious about
 * overflow.
 * 
 * The GMP library has interesting number generators which could also be 
 * introduced for POW systems.
 * https://gmplib.org/pi-with-gmp.html
 * 
 * References: 
 * BBP Algorith for Pi, The (Whitepaper) http://crd-legacy.lbl.gov/~dhbailey/dhbpapers/bbp-alg.pdf
 * Pi in hexidecimal: http://calccrypto.wikidot.com/math:pi-hex
 * 
 * Parallelization, Test system: 2x Xeon E5520 CPUs, 24GB RAM
 * - Un-Parallelized runtime for 1000 slices (10000 digits) is 11.5 seconds.
 * - Parallelized runtime, 1.25 seconds for 1000 slices
 * ***************************************************************************/

namespace BBPPiCalculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            #region Vars
            DateTime startTime = DateTime.UtcNow;                 // track the start time          
            int digitStart = 0;                                   // start digit
            int digitEnd = 10000;                                 // end digit
            List<BBPResult> PiDigits = new List<BBPResult>();     // collection of results
            #endregion

            #if !PARALLELIZE                                      // parallelization is disabled
            for (int i = digitStart; i < digitEnd/10; i++)        // to turn on set the PARALLELIZE conditional compilation symbol
            #endif
            #if PARALLELIZE                                       // parallelization is turned on
            Parallel.For(digitStart, digitEnd / 10, i =>
            #endif
            {
                PiDigit pd = new PiDigit();                       // generate the next slice of 10 
                BBPResult result = pd.Calc(i * 10);               // store the result so we can sort it later (eventually consistent)                              
                PiDigits.Add(result);                             // the list is used in case we are using parallelization which will process slices out of order                                        
                                                                               
            #if !PARALLELIZE
            }
            #endif
            #if PARALLELIZE
            });
            #endif
                        
            // sort the results - if parallization is enabled the results are out of order
            // if these results were to be stored in an eventually consistent database such as MongoDB
            // then we woudln't need to sort it in the application.
            PiDigits.Sort(new BBPResultComparer());
            foreach (BBPResult br in PiDigits)
            {
                Console.Write(br.HexDigits + " " + (br.Digit) + "\r\n");
            }

            // Display some statistics 
            TimeSpan span = DateTime.UtcNow - startTime;
            Console.WriteLine("\r\n\r\nTask finished in {0} seconds.", span.TotalSeconds);

            return;
        }     
    }
}
