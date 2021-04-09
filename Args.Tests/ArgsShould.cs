using System.Collections.Generic;
using Args.Marshalers;
using NSubstitute;

namespace Args.Tests
{
    using Exceptions;
    using NUnit.Framework;
    using System;

    [TestFixture]
    class ArgsShould
    {
        [Test]
        public void BeInitialisableWithNoSchemaOrArgs()
        {
            var args = new Args(string.Empty, new string[] { });

            Assert.NotNull(args);
        }

        [Test]
        public void BeInitialisableWithSpacesInTheSchema()
        {
            var args = new Args("x, y", new[] {"-xy"});

            Assert.True(args.Has('x'));
            Assert.True(args.Has('y'));

            Assert.AreEqual(true, args.GetBoolean('x'));
            Assert.AreEqual(true, args.GetBoolean('y'));
        }

        [Test]
        public void ReturnTheCorrectValueWhenPassedABoolean()
        {
            var args = new Args("x", new[] {"-x"});

            Assert.AreEqual(true, args.GetBoolean('x'));
        }

        [Test]
        public void ReturnTheCorrectValueWhenPassedADouble()
        {
            var doubleValue = 3.1419d;
            var args = new Args("x##", new[] {"-x", doubleValue.ToString()});

            Assert.AreEqual(doubleValue, args.GetDouble('x'));
        }


        [Test]
        public void ReturnTheCorrectValueWhenPassedAnInteger()
        {
            var intValue = 42;
            var args = new Args("x#", new[] {"-x", intValue.ToString()});

            Assert.AreEqual(intValue, args.GetInt('x'));
        }

        [Test]
        public void ReturnTheCorrectValueWhenPassedAString()
        {
            var stringValue = "test-value";
            var args = new Args("x*", new[] {"-x", stringValue});

            Assert.AreEqual(stringValue, args.GetString('x'));
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