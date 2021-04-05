namespace Args.Marshalers
{
    using System;
    using System.Collections.Generic;

    public class BooleanArgumentMarshaler : IArgumentMarshaler
    {
        private bool booleanValue;

        public void Set(IEnumerator<string> currentArgument)
        {
            booleanValue = true;
        }

        public static bool GetValue(IArgumentMarshaler am)
        {
            if (am != null && am.GetType() == typeof(BooleanArgumentMarshaler))
            {
                return ((BooleanArgumentMarshaler) am).booleanValue;
            }

            return false;
        }
    }
}