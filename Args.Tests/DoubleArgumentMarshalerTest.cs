using System;
using System.Collections.Generic;
using Args.Exceptions;
using Args.Marshalers;
using NSubstitute;
using NUnit.Framework;

namespace Args.Tests
{
    [TestFixture]
    public class DoubleArgumentMarshalerTests
    {
        private DoubleArgumentMarshaler doubleArgumentMarshaler1;
        private IEnumerator<string> enumerable;

        [SetUp]
        public void Setup()
        {
            doubleArgumentMarshaler1 = new DoubleArgumentMarshaler();
            enumerable = Substitute.For<IEnumerator<string>>();
        }


        [Test]
        public void ThrowAnErrorWhenInitialisedWithAInvalidDoubleValue()
        {
            //arrange
            enumerable.Current.Returns("Not a number");
            enumerable.MoveNext().Returns(true);

            //act

            //assert
            Assert.AreEqual(ErrorCodes.INVALID_DOUBLE,
                Assert.Throws<ArgsException>(() => doubleArgumentMarshaler1.Set(enumerable)).GetErrorCode());
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithAMissingDoubleValue()
        {
            try
            {
                new Args("x##", new[] {"-x"});
                throw new Exception();
            }
            catch (ArgsException e)
            {
                Assert.AreEqual(ErrorCodes.MISSING_DOUBLE, e.GetErrorCode());
                Assert.AreEqual('x', e.GetErrorArgumentId());
            }
        }
    }
}