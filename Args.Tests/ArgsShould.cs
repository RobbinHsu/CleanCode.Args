using Args.Exceptions;
using NUnit.Framework;

namespace Args.Tests
{
    [TestFixture]
    class ArgsShould
    {
        [Test]
        public void NoSchemaOrArgs()
        {
            var args = new Args(string.Empty, new string[] { });

            Assert.NotNull(args);
        }

        [Test]
        public void SpacesInTheSchema()
        {
            var args = new Args("x, y", new[] {"-xy"});

            Assert.True(args.Has('x'));
            Assert.True(args.Has('y'));

            Assert.AreEqual(true, args.GetBoolean('x'));
            Assert.AreEqual(true, args.GetBoolean('y'));
        }


        [Test]
        public void WhenInvalidSchemaTypes()
        {
            //arrange
            var argDelegate = GivenArgs("f~", new string[] { });

            //act

            //assert
            ShouldBeEqual(argDelegate, ErrorCodes.INVALID_ARGUMENT_FORMAT);
        }

        [Test]
        public void WhenNoSchemaAndArgs()
        {
            //arrange
            var argDelegate = GivenArgs("", new[] {"-x"});

            //act

            //assert
            ShouldBeEqual(argDelegate, ErrorCodes.UNEXPECTED_ARGUMENT);
        }

        [Test]
        public void WhenNoSchemaAndMultipleArgs()
        {
            //arrange
            var argDelegate = GivenArgs("", new[] {"-x", "-y"});

            //act

            //assert
            ShouldBeEqual(argDelegate, ErrorCodes.UNEXPECTED_ARGUMENT);
        }

        [Test]
        public void WhenPassedABoolean()
        {
            var args = new Args("x", new[] {"-x"});

            Assert.AreEqual(true, args.GetBoolean('x'));
        }

        [Test]
        public void WhenPassedADouble()
        {
            var doubleValue = 3.1419d;
            var args = new Args("x##", new[] {"-x", doubleValue.ToString()});

            Assert.AreEqual(doubleValue, args.GetDouble('x'));
        }


        [Test]
        public void WhenPassedAnInteger()
        {
            var args = new Args("x#", new[] {"-x", 42.ToString()});

            Assert.AreEqual(42, args.GetInt('x'));
        }

        [Test]
        public void WhenPassedAString()
        {
            var args = new Args("x*", new[] {"-x", "test-value"});

            Assert.AreEqual("test-value", args.GetString('x'));
        }

        [Test]
        public void WhenSchemaIsNotLetters()
        {
            //arrange
            var argDelegate = GivenArgs("*", null);

            //act

            //assert
            ShouldBeEqual(argDelegate, ErrorCodes.INVALID_ARGUMENT_NAME);
        }

        private static void ShouldBeEqual(TestDelegate argDelegate, ErrorCodes invalidArgumentName)
        {
            Assert.That(Assert.Throws<ArgsException>(argDelegate).GetErrorCode(),
                Is.EqualTo(invalidArgumentName));
        }

        private static TestDelegate GivenArgs(string schema, string[] args)
        {
            TestDelegate argDelegate = delegate { new Args(schema, args); };
            return argDelegate;
        }
    }
}