using System;
using System.Threading;
using System.IO;

var oddEvent = new EventWaitHandle(true, EventResetMode.AutoReset, "OddEvent");
var evenEvent = new EventWaitHandle(false, EventResetMode.AutoReset, "EvenEvent");

for (int i = 2; i <= 10; i += 2)
{
    evenEvent.WaitOne(); 
    Console.WriteLine($"Even: {i}");
    Thread.Sleep(500);
    oddEvent.Set(); 
}












