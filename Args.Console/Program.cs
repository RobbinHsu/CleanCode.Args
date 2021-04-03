namespace Args.Console
{
    using System;
    using Exceptions;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var arg = new Args("l,p#,d*,a[*]", args);
                var logging = arg.GetBoolean('l');
                var port = arg.GetInt('p');
                var directory = arg.GetString('d');
                var files = arg.GetStringArray('a');

                ExecuteApplication(logging, port, directory, files);
            }
            catch (ArgsException e)
            {
                Console.WriteLine($"Argument error: {e.ErrorMessage()}");
            }
        }

        private static void ExecuteApplication(bool logging, int port, string directory, string[] files)
        {
            Console.WriteLine("Executed application with:");
            Console.WriteLine($"bool logging param = {logging}");
            Console.WriteLine($"int port param = {port}");
            Console.WriteLine($"string directory param = {directory}");
            Console.WriteLine($"string array files param = {string.Join(" ", files)}");
            Console.ReadKey();
        }
    }
}