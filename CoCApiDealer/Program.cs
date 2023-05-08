using CoCApiDealer;
using CoCApiDealer.ApiRequests;
using CoCStatsTracker.ApiEntities;
using CoCStatsTracker.Builders;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Storage;
using System.Diagnostics.Metrics;
using System.Security.Cryptography.X509Certificates;
using CoCStatsTracker;
using System.Text.RegularExpressions;

public class Program
{
    /// <summary>
    /// #YPPGCCY8 - тег клана
    /// #2VGG92CL9 - тег игрока
    /// </summary>
    
    static async Task Main(string[] args)
    {
        var str = "12345";

        Console.WriteLine(str.Substring(0, 10));
    }

}
