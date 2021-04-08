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
        private DoubleArgumentMarshaler doubleArgumentMarshaler;
        private IEnumerator<string> enumerable;

        [SetUp]
        public void Setup()
        {
            doubleArgumentMarshaler = new DoubleArgumentMarshaler();
            enumerable = Substitute.For<IEnumerator<string>>();
        }


        [Test]
        public void ThrowAnErrorWhenInitialisedWithAInvalidDoubleValue()
        {
            //arrange
            GivenCurrentArgument("Not a number");

            //act

            //assert
            ShouldeBeEqualErrorCode(ErrorCodes.INVALID_DOUBLE);
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithAMissingDoubleValue()
        {
            //arrange
            GivenCurrentArgument(null);

            //act

            //assert
            ShouldeBeEqualErrorCode(ErrorCodes.MISSING_DOUBLE);
        }

        private void ShouldeBeEqualErrorCode(ErrorCodes errorCode)
        {
            Assert.AreEqual(errorCode,
                Assert.Throws<ArgsException>(() => doubleArgumentMarshaler.Set(enumerable)).GetErrorCode());
        }

        private void GivenCurrentArgument(string currentArgument)
        {
            enumerable.Current.Returns(currentArgument);
            enumerable.MoveNext().Returns(true);
        }
    }
}