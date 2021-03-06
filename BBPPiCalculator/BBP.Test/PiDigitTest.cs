﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BBP;

namespace BBP.Test
{
    /// <summary>
    /// The unit testing is necessary since there is lots of parallelization happening.
    /// During the optimization process we want to ensure that the data matches what
    /// we expect.
    /// </summary>
    [TestClass]
    public class PiDigitTest
    {
        /// <summary>
        /// Match the hex stream chars 0-9
        /// </summary>
        [TestMethod]
        public void PiDigits_0()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(0);
            Assert.AreEqual(result.HexDigits, "243F6A8885");
        }

        /// <summary>
        /// Match the hex stream chars 10-19
        /// </summary>
        [TestMethod]
        public void PiDigits_10()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(10);
            Assert.AreEqual(result.HexDigits, "A308D31319");
        }

        /// <summary>
        /// Match the hex stream chars 20-29
        /// </summary>
        [TestMethod]
        public void PiDigits_20()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(20);
            Assert.AreEqual(result.HexDigits, "8A2E037073");
        }

        /// <summary>
        /// Match the hex stream chars 30-39
        /// </summary>
        [TestMethod]
        public void PiDigits_30()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(30);
            Assert.AreEqual(result.HexDigits, "44A4093822");
        }

        /// <summary>
        /// Match the hex stream chars 40-49
        /// </summary>
        [TestMethod]
        public void PiDigits_40()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(40);
            Assert.AreEqual(result.HexDigits, "299F31D008");
        }

        /// <summary>
        /// Match the hex stream chars 50-59
        /// </summary>
        [TestMethod]
        public void PiDigits_50()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(50);
            Assert.AreEqual(result.HexDigits, "2EFA98EC4E");
        }

        /// <summary>
        /// Match the hex stream chars 60-69
        /// </summary>
        [TestMethod]
        public void PiDigits_60()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(60);
            Assert.AreEqual(result.HexDigits, "6C89452821");
        }

        /// <summary>
        /// Match the hex stream chars 70-79
        /// </summary>
        [TestMethod]
        public void PiDigits_70()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(70);
            Assert.AreEqual(result.HexDigits, "E638D01377");
        }

        /// <summary>
        /// Match the hex stream chars 80-89
        /// </summary>
        [TestMethod]
        public void PiDigits_80()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(80);
            Assert.AreEqual(result.HexDigits, "BE5466CF34");
        }

        /// <summary>
        /// Match the hex stream chars 90-99
        /// </summary>
        [TestMethod]
        public void PiDigits_90()
        {
            PiDigit pd = new PiDigit();
            BBPResult result = pd.Calc(90);
            Assert.AreEqual(result.HexDigits, "E90C6CC0AC");
        }
    }
}
