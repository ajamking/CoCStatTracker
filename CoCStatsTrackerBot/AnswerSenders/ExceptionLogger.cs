using Serilog;

namespace CoCStatsTrackerBot;

public static class ExceptionLogger
{
    static ExceptionLogger()
    {
        Log.Logger = new LoggerConfiguration()
         .WriteTo.File(Program.ExceptionLogsPath)
         .CreateLogger();
    }

    public static void LogException(this Exception ex, string userName, long chatId, string userMessage, string dopMessage = "")
    {
        var stackTrace = ex.StackTrace.Split("\\").ToList();

        stackTrace.RemoveRange(0, stackTrace.IndexOf("CoCStatsTracker"));

        var newStackTrace = string.Join('\\', stackTrace);

        Log.Error($"\n{new string('-', 36)}\n" +
            $"На сообщение {userMessage} от {userName} [{chatId}]\n" +
            $"Произошла ошибка: {ex.Message}\n" +
            $"Дополнительное сообщение: {dopMessage}" +
            $"\nСтактрейс: {newStackTrace}\n");

        Log.CloseAndFlush();
    }
}