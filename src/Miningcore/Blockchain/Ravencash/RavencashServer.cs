public class RavencashServer : StratumServer
{
    protected override void OnConnect(StratumConnection connection)
    {
        connection.SetProtocolVersion(StratumProtocolVersion.V2);

        connection.OnRequest("mining.subscribe", OnSubscribe);
        connection.OnRequest("mining.authorize", OnAuthorize);
        connection.OnRequest("mining.submit", OnSubmit);
    }

    private async ValueTask OnSubmit(StratumConnection connection, JsonRpcRequest request)
    {
        var context = connection.ContextAs<RavencashWorkerContext>();
        var share = await jobManager.ProcessShareAsync(connection, request);
        await shareProcessor.ProcessShareAsync(share, connection);
    }
}
