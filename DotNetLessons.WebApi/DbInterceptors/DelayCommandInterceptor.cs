using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;

namespace DotNetLessons.WebApi.DbInterceptors;

public class DelayCommandInterceptor : DbCommandInterceptor
{
    public override InterceptionResult<DbDataReader> ReaderExecuting(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result
    )
    {
        if (SgouldChangeQuery(eventData.CommandSource))
        {
            command.CommandText = CreateCommandWithDelay(command.CommandText);
        }

        return result;
    }

    public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
        DbCommand command,
        CommandEventData eventData,
        InterceptionResult<DbDataReader> result,
        CancellationToken cancellationToken = default
    )
    {
        if (SgouldChangeQuery(eventData.CommandSource))
        {
            command.CommandText = CreateCommandWithDelay(command.CommandText);
        }

        return await Task.FromResult(result);
    }

    private static bool SgouldChangeQuery(CommandSource commandSource)
    {
        return new List<CommandSource> { CommandSource.SaveChanges, CommandSource.LinqQuery }.Contains(commandSource);
    }

    private static string CreateCommandWithDelay(string command)
    {
        return "WAITFOR DELAY '00:00:00.2';" + Environment.NewLine + command;
    }
}
