using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/******************************************************************************
 * BBP Pi Calculator
 * 2014.12.14
 * Payson Welch; paysonwelch.com, freeideas.org
 * 
 * Abstract
 * --------------------------
 * This application is a proof of concept to demonstrate the capability of the 
 * Bailey–Borwein–Plouffe formula which can be used to solve for [n]th decimal
 * in the pi series.
 * http://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula
 * 
 * Executive Summary
 * --------------------------
 * Originally I was considering using the BBP formula to create a new Proof of
 * Work (POW) system for crypto currencies. With the advent of Proof of Stake 
 * however and other anonymizing features of currencies the BBP-Pi POW system
 * would not be enough in itself to create a highly successful crypto currency.
 * A BBP-Pi POW currency would be more of a novelty of interest to reserachers
 * mostly. There is also PrimeCoin which uses a POW system for hunting chains
 * of prime numbers. So although this is an interesting idea it needs more
 * thought before simply createing a POW based on BBP.
 * 
 * This applicaiton has been structured in such a way that it could easily be
 * modified for grid computing. This application currently will not focus on 
 * grid payload distribution however, but will focus on breaking work items
 * into small specific units which can be easily inserted into a grid pipeline
 * in the future.
 * http://en.wikipedia.org/wiki/Primecoin
 *  
 * Bailey–Borwein–Plouffe 
 * --------------------------
 * The BBP formula was accidentally discovered in 1995 by researchers. It is a 
 * spigot algorithm which can be used to calculate any digit of Pi.
 * 
 * Compatibility
 * --------------------------
 * This proof of concept will not take into account large numbers. As the 
 * dataset grows the max number we wil be able to use in the calculations is
 * 18,446,744,073,709,551,615 which is the value of an unsigned int64. For 
 * crypto currencies it would be possible to use the GNU bignum library.
 * 
 * ***************************************************************************/

namespace BBPPiCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
    }
}
