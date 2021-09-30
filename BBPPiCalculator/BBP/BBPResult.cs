namespace BBP
{
    /// <summary>
    /// Simple structure for passing back the result and it's position.
    /// This is used to maintain the correct order of the result during
    /// parallel calculations.
    /// </summary>
    public class BBPResult
    {
        public long Digit { get; set; }
        public string HexDigits { get; set; }
    }
}
