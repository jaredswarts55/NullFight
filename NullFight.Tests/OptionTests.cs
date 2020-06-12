using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NullFight.Exceptions;
using static NullFight.FunctionalExtensions;

namespace NullFight.Tests
{
    [TestClass]
    public class OptionTests
    {
        [TestMethod]
        public void MapValue_WithOptionValue_ReturnsMappedValue()
        {
            var option = Some("1");
            var mappedOption = option.MapValue(int.Parse);
            Assert.AreEqual(1, mappedOption.Value);
        }

        [TestMethod]
        public void Match_WithOptionValue_ReturnsMappedValue()
        {
            var option = Some("1");
            var value = option.Match(int.Parse, () => 2);
            Assert.AreEqual(1, value);
        }

        [TestMethod]
        public void Match_WithOptionNone_ReturnsMappedValue()
        {
            Option<string> option = None();
            var value = option.Match(int.Parse, () => 2);
            Assert.AreEqual(2, value);
        }
    }
}
