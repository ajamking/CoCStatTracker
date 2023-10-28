using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoCStatsTrackerBot.Exceptions;

public class FunctionException : Exception
{
    public FunctionException(Exception e) : base()
    {
        Console.WriteLine("FunctionException" + e.Message + "\n" + e.StackTrace);
    }

}
