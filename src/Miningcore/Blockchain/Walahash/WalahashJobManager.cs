public class WalahashJobManager : JobManagerBase<WalahashJob>
{
    private readonly IConnectionManager connectionManager;
    private readonly IBlockchainManager blockchainManager;

    public override void Configure(PoolConfig poolConfig, ClusterConfig clusterConfig)
    {
        currentJob = CreateNewJob();
        StartBlockPolling();
    }

    public override async ValueTask<Share> SubmitShareAsync(StratumConnection worker, Share share, string[] request)
    {
        // Implement share submission logic
        return share;
    }

    protected override async Task<(bool IsNew, bool Force)> UpdateJob(bool force)
    {
        // Implement job update logic
        return (true, false);
    }

    protected override async Task BroadcastJob(WalahashJob job, bool force)
    {
        var connections = connectionManager.Connections;

        foreach(var connection in connections)
        {
            var context = connection.ContextAs<WalahashWorkerContext>();

            if(context.IsAuthorized)
            {
                await connection.NotifyAsync(WalahashStratumMethods.Mining_Notify, new object[]
                {
                    job.JobId,
                    job.BlockTemplate,
                    job.Target,
                    force
                });
            }
        }
    }

    private async Task StartBlockPolling()
    {
        while(!cts.Token.IsCancellationRequested)
        {
            try
            {
                var blockTemplate = await blockchainManager.GetBlockTemplateAsync();

                if(blockTemplate.Height > currentJob.BlockHeight)
                {
                    currentJob = CreateNewJob(blockTemplate);
                    await BroadcastJob(currentJob, true);
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);
            }

            await Task.Delay(poolConfig.BlockRefreshInterval);
        }
    }
}
