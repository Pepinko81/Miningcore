public class HoohashJobManager : JobManagerBase<HoohashJob>
{
    private readonly HoohashBlockTemplateBuilder templateBuilder;
    private readonly HoohashDifficultyAdjuster difficultyAdjuster;
    private readonly IConnectionManager connectionManager;

    public override void Configure(PoolConfig poolConfig, ClusterConfig clusterConfig)
    {
        // Configure job parameters
        currentJob = CreateJob();
    }

    public override async Task<Share> ProcessShareAsync(StratumConnection connection, JsonRpcRequest request)
    {
        var context = connection.ContextAs<HoohashWorkerContext>();
        var share = await ValidateShareAsync(connection, request);

        // Update worker difficulty
        var newDifficulty = difficultyAdjuster.GetNextDifficulty(context, currentJob.NetworkDifficulty);
        if(newDifficulty != context.Difficulty)
        {
            context.Difficulty = newDifficulty;
            await connection.NotifyAsync(HoohashStratumMethods.Mining_SetDifficulty, new[] { newDifficulty });
        }

        return share;
    }

    protected override async Task UpdateJob(bool forceUpdate)
    {
        var blockTemplate = await blockchain.GetBlockTemplateAsync();
        var job = templateBuilder.CreateNewJob(blockTemplate);

        if(job.Height > currentJob?.Height || forceUpdate)
        {
            currentJob = job;
            await BroadcastJob(job, forceUpdate);
        }
    }

    protected override async Task BroadcastJob(HoohashJob job, bool force)
    {
        var connections = connectionManager.Connections;

        foreach(var connection in connections)
        {
            var context = connection.ContextAs<HoohashWorkerContext>();

            await connection.NotifyAsync(HoohashStratumMethods.Mining_Notify, new object[]
            {
                job.JobId,
                job.BlockTemplate,
                context.Difficulty
            });
        }
    }
}
