using System;
using System.Collections.Generic;
using Args.Exceptions;
using Args.Marshalers;
using NSubstitute;
using NUnit.Framework;

namespace Args.Tests
{
    [TestFixture]
    public class IntArgumentMarshalerTest
    {
        private IEnumerator<string> enumerable;
        private IArgumentMarshaler integerArgumentMarshaler;

        [SetUp]
        public void Setup()
        {
            integerArgumentMarshaler = new IntArgumentMarshaler();
            enumerable = Substitute.For<IEnumerator<string>>();
        }


        [Test]
        public void ThrowAnErrorWhenInitialisedWithAMissingIntegerValue()
        {
            //arrange
            GivenCurrentArgument(null);

            //act

            //assert
            ShouldeBeEqualErrorCode(ErrorCodes.MISSING_INTEGER);
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithAnInvalidIntegerValue()
        {
            //arrange
            GivenCurrentArgument("Not A Number");

            //act

            //assert
            ShouldeBeEqualErrorCode(ErrorCodes.INVALID_INTEGER);
        }


        private void ShouldeBeEqualErrorCode(ErrorCodes errorCode)
        {
            Assert.AreEqual(errorCode,
                Assert.Throws<ArgsException>(() => integerArgumentMarshaler.Set(enumerable)).GetErrorCode());
        }

        private void GivenCurrentArgument(string currentArgument)
        {
            enumerable.Current.Returns(currentArgument);
            enumerable.MoveNext().Returns(true);
        }
    }
}