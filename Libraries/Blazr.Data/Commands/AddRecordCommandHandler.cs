﻿/// ============================================================
/// Author: Shaun Curtis, Cold Elm Coders
/// License: Use And Donate
/// If you use it, donate something to a charity somewhere
/// ============================================================
namespace Blazr.Data;

public class AddRecordCommandHandler<TRecord, TDbContext>
    : ICQSHandler<AddRecordCommand<TRecord>, ValueTask<CommandResult>>
    where TDbContext : DbContext
    where TRecord : class, new()
{
    protected IDbContextFactory<TDbContext> factory;

    public AddRecordCommandHandler(IDbContextFactory<TDbContext> factory)
        =>  this.factory = factory;

    public async ValueTask<CommandResult> ExecuteAsync(AddRecordCommand<TRecord> command)
    {
        using var dbContext = factory.CreateDbContext();
        dbContext.Add<TRecord>(command.Record);
        return await dbContext.SaveChangesAsync() == 1
            ? CommandResult.Successful("Record Saved")
            : CommandResult.Failure("Error saving Record");
    }
}
