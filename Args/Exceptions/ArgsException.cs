namespace Args.Exceptions
{
    using System;

    public class ArgsException : Exception
    {
        private char errorArgumentId = '\0';
        private string errorParameter = null;
        private ErrorCodes errorCode = ErrorCodes.OK;

        public ArgsException() { }

        public ArgsException(string message) : base(message) { }

        public ArgsException(ErrorCodes errorCode)
        {
            this.errorCode = errorCode;
        }

        public ArgsException(ErrorCodes errorCode, string errorParameter)
        {
            this.errorCode = errorCode;
            this.errorParameter = errorParameter;
        }

        public ArgsException(ErrorCodes errorCode, char errorArgumentId, string errorParameter)
        {
            this.errorCode = errorCode;
            this.errorArgumentId = errorArgumentId;
            this.errorParameter = errorParameter;
        }

        public char GetErrorArgumentId()
        {
            return errorArgumentId;
        }

        public void SetErrorArgumentId(char errorArgumentId)
        {
            this.errorArgumentId = errorArgumentId;
        }

        public string GetErrorParameter()
        {
            return this.errorParameter;
        }

        public void SetErrorParameter(string errorParameter)
        {
            this.errorParameter = errorParameter;
        }

        public ErrorCodes GetErrorCode()
        {
            return this.errorCode;
        }

        public void SetErrorCode(ErrorCodes errorCode)
        {
            this.errorCode = errorCode;
        }

        public string ErrorMessage()
        {
            switch (errorCode)
            {
                case ErrorCodes.OK:
                    return "TILT: Should not get here.";
                case ErrorCodes.UNEXPECTED_ARGUMENT:
                    return $"Argument -{errorArgumentId} unexpected.";
                case ErrorCodes.MISSING_STRING:
                    return $"Could not find a string parameter for -{errorArgumentId}.";
                case ErrorCodes.INVALID_INTEGER:
                    return $"Argument -{errorArgumentId} expects an integer but was '{errorParameter}'.";
                case ErrorCodes.MISSING_INTEGER:
                    return $"Could not find an integer parameter for -{errorArgumentId}.";
                case ErrorCodes.INVALID_DOUBLE:
                    return $"Argument -{errorArgumentId} expects a double but was '{errorParameter}'.";
                case ErrorCodes.MISSING_DOUBLE:
                    return $"Could not find a double parameter for -{errorArgumentId}.";
                case ErrorCodes.INVALID_ARGUMENT_NAME:
                    return $"'{errorArgumentId}' is not a valid argument name.";
                case ErrorCodes.INVALID_ARGUMENT_FORMAT:
                    return $"'{errorParameter}' is not a valid argument format.";
            }

            return string.Empty;
        }
    }
}
