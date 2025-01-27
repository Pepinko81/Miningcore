public class WalahashServer : StratumServer
{
    private readonly IConnectionManager connectionManager;
    private readonly ILogger logger;

    protected override void OnConnect(StratumConnection connection)
    {
        connection.SetProtocolVersion(StratumProtocolVersion.V2);

        connection.OnRequest(WalahashStratumMethods.Mining_Subscribe, OnSubscribe);
        connection.OnRequest(WalahashStratumMethods.Mining_Authorize, OnAuthorize);
        connection.OnRequest(WalahashStratumMethods.Mining_Submit, OnSubmit);
    }

    private async ValueTask OnSubscribe(StratumConnection connection, JsonRpcRequest request)
    {
        var context = connection.ContextAs<WalahashWorkerContext>();
        var extraNonce = GenerateExtraNonce();

        await connection.NotifyAsync(WalahashStratumMethods.Mining_Notify, new object[]
        {
            extraNonce,
            context.Difficulty,
            context.WorkerParameters
        });
    }

    private async ValueTask OnAuthorize(StratumConnection connection, JsonRpcRequest request)
    {
        var context = connection.ContextAs<WalahashWorkerContext>();
        var parameters = request.ParamsAs<string[]>();

        context.WorkerAddress = parameters[0];
        context.IsAuthorized = ValidateAddress(parameters[0]);
        context.LastActivity = DateTime.UtcNow;

        await connection.RespondAsync(request.Id, context.IsAuthorized);
    }

    private async ValueTask OnSubmit(StratumConnection connection, JsonRpcRequest request)
    {
        var context = connection.ContextAs<WalahashWorkerContext>();
        var share = await jobManager.ProcessShareAsync(connection, request);

        await shareProcessor.ProcessShareAsync(share, connection);
        await BroadcastBlockIfNeeded(share);
        await connection.RespondAsync(request.Id, true);
    }
}
