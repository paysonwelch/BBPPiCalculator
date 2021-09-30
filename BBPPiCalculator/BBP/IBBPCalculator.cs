namespace BBP
{
    /// <summary>
    /// Underlying interface for BBP algos which will allow us to use C# generics 
    /// and recycle the pipeline code to calculate different numbers on the same 
    /// grid.
    /// 
    /// The overall BBP algorithm structure is:
    /// http://upload.wikimedia.org/math/7/5/c/75c6b64a7bfaed0d79f6c3f43aa84a64.png       
    /// </summary>
    interface IBBPCalculator
    {
        BBPResult Calc(long n);
    }
}
