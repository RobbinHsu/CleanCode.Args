namespace Args.Marshalers
{
    using System.Collections.Generic;

    public interface IArgumentMarshaler
    {
        void Set(IEnumerator<string> currentArgument);
    }
}