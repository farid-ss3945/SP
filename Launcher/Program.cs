using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string operation = args[0];
        if (!double.TryParse(args[1], out double a) || !double.TryParse(args[2], out double b))
        {
            Log(args[4], args[3], $"Error: incorrect arguments '{args[1]}' и '{args[2]}'");
            return;
        }

        bool debug = bool.Parse(args[3]);
        string logFile = args[4];

        try
        {
            double result = operation switch
            {
                "add" => a + b,
                "sub" => a - b,
                "mul" => a * b,
                "div" => b != 0 ? a / b : throw new DivideByZeroException(),
                _ => throw new ArgumentException("Unknown Operation")
            };

            Console.WriteLine($"Result: {result}");
            Log(logFile, debug.ToString(), $"Operation {operation} with {a} and {b} = {result}");
            Console.WriteLine("Press any button to quit...");
        }
        catch (Exception ex)
        {
            Log(logFile, debug.ToString(), $"Error: {ex.Message}");
        }
        Console.WriteLine("Press any button to quit...");
        Thread.Sleep(50000);
    }

    static void Log(string file, string debug, string message)
    {
        if (bool.TryParse(debug, out bool isDebug) && isDebug)
        {
            string logMessage = $"[{DateTime.Now}] {message}";
            Console.WriteLine(logMessage);
            File.AppendAllText(file, logMessage + Environment.NewLine);
        }
    }
    

}