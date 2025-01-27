public class HoohashServer : StratumServer
{
    private readonly IConnectionManager connectionManager;

    protected override void OnConnect(StratumConnection connection)
    {
        connection.SetProtocolVersion(StratumProtocolVersion.V2);

        connection.OnRequest(HoohashStratumMethods.Mining_Subscribe, OnSubscribe);
        connection.OnRequest(HoohashStratumMethods.Mining_Authorize, OnAuthorize);
        connection.OnRequest(HoohashStratumMethods.Mining_Submit, OnSubmit);
    }

    private async ValueTask OnSubscribe(StratumConnection connection, JsonRpcRequest request)
    {
        var context = connection.ContextAs<HoohashWorkerContext>();
        var extraNonce = connection.ConnectionId.ToString("x8");

        await connection.NotifyAsync(HoohashStratumMethods.Mining_Notify, new object[]
        {
            extraNonce,
            context.Difficulty
        });
    }

    private async ValueTask OnAuthorize(StratumConnection connection, JsonRpcRequest request)
    {
        var context = connection.ContextAs<HoohashWorkerContext>();
        var parameters = request.ParamsAs<string[]>();

        context.WorkerAddress = parameters[0];
        context.IsAuthorized = true;

        await connection.RespondAsync(request.Id, true);
    }

    private async ValueTask OnSubmit(StratumConnection connection, JsonRpcRequest request)
    {
        var context = connection.ContextAs<HoohashWorkerContext>();
        var share = await jobManager.ProcessShareAsync(connection, request);

        await shareProcessor.ProcessShareAsync(share, connection);
        await connection.RespondAsync(request.Id, true);
    }
}
