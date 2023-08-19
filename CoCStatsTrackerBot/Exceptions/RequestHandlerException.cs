using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCStatsTrackerBot.Exceptions;

public class RequestHandlerException : Exception
{
    public RequestHandlerException(Exception e) : base()
    {
        Console.WriteLine("RequestHandlerException" + e.Message + "\n" + e.StackTrace);
    }
}
