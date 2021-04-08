using System.Collections.Generic;
using Args.Exceptions;
using Args.Marshalers;
using NSubstitute;
using NUnit.Framework;

namespace Args.Tests
{
    [TestFixture]
    public class StringArgumentMarshalerTest
    {
        private IEnumerator<string> enumerable;
        private StringArgumentMarshaler stringArgumentMarshaler;

        [SetUp]
        public void Setup()
        {
            stringArgumentMarshaler = new StringArgumentMarshaler();
            enumerable = Substitute.For<IEnumerator<string>>();
        }

        [Test]
        public void WhenInitializedString()
        {
            GivenSingleCurrentArgument();

            ShouldeBeEqualErrorCode(ErrorCodes.MISSING_STRING);
        }

        private void ShouldeBeEqualErrorCode(ErrorCodes errorCode)
        {
            Assert.AreEqual(errorCode,
                Assert.Throws<ArgsException>(() => stringArgumentMarshaler.Set(enumerable)).GetErrorCode());
        }

        private void GivenSingleCurrentArgument()
        {
            enumerable.Current.Returns((string) null);
            enumerable.MoveNext().Returns(false);
        }
    }
}