namespace Args.Marshalers
{
    using Exceptions;
    using System;
    using System.Collections.Generic;

    public class StringArrayArgumentMarshaler : IArgumentMarshaler
    {
        private string[] stringArrayValue = { };

        public void Set(IEnumerator<string> currentArgument)
        {
            var parameter = string.Empty;
            try
            {
                if (currentArgument.MoveNext() == false)
                {
                    throw new ArgsException(ErrorCodes.MISSING_STRING);
                }

                parameter = currentArgument.Current;
                stringArrayValue = parameter.Split(new[] {","}, StringSplitOptions.None);
            }
            catch (ArgumentException)
            {
                throw new ArgsException(ErrorCodes.INVALID_ARGUMENT_FORMAT, parameter);
            }
        }

        public static string[] getValue(IArgumentMarshaler am)
        {
            if (am != null && am.GetType() == typeof(StringArrayArgumentMarshaler))
            {
                return ((StringArrayArgumentMarshaler) am).stringArrayValue;
            }

            return new string[] { };
        }
    }
}