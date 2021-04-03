namespace Args
{
    using Exceptions;
    using Marshalers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Args
    {
        private readonly ISet<char> argsFound;
        private IEnumerator<string> currentArgument;
        private readonly Dictionary<char, IArgumentMarshaler> marshalers;

        public Args(string schema, string[] args)
        {
            marshalers = new Dictionary<char, IArgumentMarshaler>();
            argsFound = new HashSet<char>();

            ParseSchema(schema);
            ParseArgumentStrings(args.ToList());
        }

        private void ParseSchema(string schema)
        {
            var schemaElements = schema.Split(new string[] {","}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var element in schemaElements)
            {
                ParseSchemaElement(element.Trim());
            }
        }

        private void ParseSchemaElement(string element)
        {
            var elementId = element[0];
            var elementTail = element.Substring(1);
            ValidateSchemaElementId(elementId);

            if (elementTail.Length == 0)
            {
                marshalers.Add(elementId, new BooleanArgumentMarshaler());
            }
            else if (elementTail.Equals("*"))
            {
                marshalers.Add(elementId, new StringArgumentMarshaler());
            }
            else if (elementTail.Equals("#"))
            {
                marshalers.Add(elementId, new IntArgumentMarshaler());
            }
            else if (elementTail.Equals("##"))
            {
                marshalers.Add(elementId, new DoubleArgumentMarshaler());
            }
            else if (elementTail.Equals("[*]"))
            {
                marshalers.Add(elementId, new StringArrayArgumentMarshaler());
            }
            else
            {
                throw new ArgsException(ErrorCodes.INVALID_ARGUMENT_FORMAT, elementId, elementTail);
            }
        }

        private void ValidateSchemaElementId(char elementId)
        {
            if (char.IsLetter(elementId) == false)
            {
                throw new ArgsException(ErrorCodes.INVALID_ARGUMENT_NAME, elementId, null);
            }
        }

        private void ParseArgumentStrings(List<string> argsList)
        {
            currentArgument = argsList.GetEnumerator();

            for (currentArgument = argsList.GetEnumerator(); currentArgument.MoveNext() == true;)
            {
                var argString = currentArgument.Current;
                if (argString.StartsWith("-"))
                {
                    ParseArgumentCharacters(argString.Substring(1));
                }
                else
                {
                    break;
                }
            }
        }

        private void ParseArgumentCharacters(string argChars)
        {
            for (var i = 0; i < argChars.Length; i++)
            {
                ParseArgumentCharacter(argChars[i]);
            }
        }

        private void ParseArgumentCharacter(char argChar)
        {
            if (marshalers.TryGetValue(argChar, out var m) == false)
            {
                throw new ArgsException(ErrorCodes.UNEXPECTED_ARGUMENT, argChar, null);
            }
            else
            {
                argsFound.Add(argChar);
                try
                {
                    m.Set(currentArgument);
                }
                catch (ArgsException e)
                {
                    e.SetErrorArgumentId(argChar);
                    throw e;
                }
            }
        }

        public bool Has(char arg)
        {
            return argsFound.Contains(arg);
        }

        public int NextArgument()
        {
            var currentItem = currentArgument.Current.GetHashCode();
            var index = 0;
            foreach (var arg in argsFound)
            {
                if (arg.GetHashCode() == currentItem.GetHashCode())
                {
                    return index;
                }

                index++;
            }

            return -1;
        }

        public bool GetBoolean(char arg)
        {
            return BooleanArgumentMarshaler.GetValue(marshalers[arg]);
        }

        public string GetString(char arg)
        {
            return StringArgumentMarshaler.GetValue(marshalers[arg]);
        }

        public int GetInt(char arg)
        {
            return IntArgumentMarshaler.GetValue(marshalers[arg]);
        }

        public double GetDouble(char arg)
        {
            return DoubleArgumentMarshaler.getValue(marshalers[arg]);
        }

        public string[] GetStringArray(char arg)
        {
            return StringArrayArgumentMarshaler.getValue(marshalers[arg]);
        }
    }
}