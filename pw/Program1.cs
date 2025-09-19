using System;
using System.Diagnostics;
using System.Threading;

var oddEvent = new EventWaitHandle(true, EventResetMode.AutoReset, "OddEvent");
var evenEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "EvenEvent");

for (int i = 1; i <= 10; i += 2)
{
    oddEvent.WaitOne(); 
    Console.WriteLine($"Odd: {i}");
    Thread.Sleep(500);
    evenEvent.Set();
}


