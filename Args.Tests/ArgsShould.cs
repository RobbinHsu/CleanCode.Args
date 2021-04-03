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
        public void ThrowAnErrorWhenInitializedWithNoSchemaAndArgs()
        {
            try
            {
                new Args("", new string[] { "-x" });
                throw new Exception();
            }
            catch(ArgsException e)
            {
                Assert.AreEqual(ErrorCodes.UNEXPECTED_ARGUMENT, e.GetErrorCode());
                Assert.AreEqual('x', e.GetErrorArgumentId());
            }
        }

        [Test]
        public void ThrowAnErrorWhenInitializedWithNoSchemaAndMultipleArgs()
        {
            try
            {
                new Args("", new string[] { "-x", "-y" });
                throw new Exception();
            }
            catch(ArgsException e)
            {
                Assert.AreEqual(ErrorCodes.UNEXPECTED_ARGUMENT, e.GetErrorCode());
                Assert.AreEqual('x', e.GetErrorArgumentId());
            }
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithASchemaWithoutLetters()
        {
            try
            {
                new Args("*", new string[] {});
                throw new Exception();
            }
            catch(ArgsException e)
            {
                Assert.AreEqual(ErrorCodes.INVALID_ARGUMENT_NAME, e.GetErrorCode());
                Assert.AreEqual('*', e.GetErrorArgumentId());
            }
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithInvalidSchemaTypes()
        {
            try
            {
                new Args("f~", new string[] {});
                throw new Exception();
            }
            catch(ArgsException e)
            {
                Assert.AreEqual(ErrorCodes.INVALID_ARGUMENT_FORMAT, e.GetErrorCode());
                Assert.AreEqual('f', e.GetErrorArgumentId());
            }
        }

        [Test]
        public void ReturnTheCorrectValueWhenPassedABoolean()
        {
            var args = new Args("x", new string[] { "-x" });

            Assert.AreEqual(true, args.GetBoolean('x'));
        }

        [Test]
        public void ReturnTheCorrectValueWhenPassedAString()
        {
            var stringValue = "test-value";
            var args = new Args("x*", new string[] { "-x", stringValue });

            Assert.AreEqual(stringValue, args.GetString('x'));
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithAStringAndNoCorrespondingArgValue()
        {
            try
            {
                new Args("x*", new string[] { "-x" });
                throw new Exception();
            }
            catch (ArgsException e)
            {
                Assert.AreEqual(ErrorCodes.MISSING_STRING, e.GetErrorCode());
                Assert.AreEqual('x', e.GetErrorArgumentId());
            }
        }

        [Test]
        public void BeInitialisableWithSpacesInTheSchema()
        {
            var args = new Args("x, y", new string[] { "-xy" });

            Assert.True(args.Has('x'));
            Assert.True(args.Has('y'));

            Assert.AreEqual(true, args.GetBoolean('x'));
            Assert.AreEqual(true, args.GetBoolean('y'));
        }

        [Test]
        public void ReturnTheCorrectValueWhenPassedAnInteger()
        {
            var intValue = 42;
            var args = new Args("x#", new string[] { "-x", intValue.ToString()});

            Assert.AreEqual(intValue, args.GetInt('x'));
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithAnInvalidIntegerValue()
        {
            string invalidIntegerParameter = "Forty-two";
            try
            {
                new Args("x#", new string[] { "-x", invalidIntegerParameter });
                throw new Exception();
            }
            catch (ArgsException e)
            {
                Assert.AreEqual(ErrorCodes.INVALID_INTEGER, e.GetErrorCode());
                Assert.AreEqual('x', e.GetErrorArgumentId());
                Assert.AreEqual(invalidIntegerParameter, e.GetErrorParameter());
            }
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithAMissingIntegerValue()
        {
            try
            {
                new Args("x#", new string[] { "-x" });
                throw new Exception();
            }
            catch (ArgsException e)
            {
                Assert.AreEqual(ErrorCodes.MISSING_INTEGER, e.GetErrorCode());
                Assert.AreEqual('x', e.GetErrorArgumentId());
            }
        }

        [Test]
        public void ReturnTheCorrectValueWhenPassedADouble()
        {
            var doubleValue = 3.1419d;
            var args = new Args("x##", new string[] { "-x", doubleValue.ToString()});

            Assert.AreEqual(doubleValue, args.GetDouble('x'));
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithAInvalidDoubleValue()
        {
            string invalidDoubleParameter = "Three point something";
            try
            {
                new Args("x##", new string[] { "-x", invalidDoubleParameter });
                throw new Exception();
            }
            catch (ArgsException e)
            {
                Assert.AreEqual(ErrorCodes.INVALID_DOUBLE, e.GetErrorCode());
                Assert.AreEqual('x', e.GetErrorArgumentId());
                Assert.AreEqual(invalidDoubleParameter, e.GetErrorParameter());
            }
        }

        [Test]
        public void ThrowAnErrorWhenInitialisedWithAMissingDoubleValue()
        {
            try
            {
                new Args("x##", new string[] { "-x" });
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
