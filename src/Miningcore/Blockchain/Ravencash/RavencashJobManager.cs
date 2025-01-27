public class RavencashJobManager : JobManagerBase<RavencashJob>
{
    private readonly IBlockchainManager blockchain;

    protected override async Task UpdateJob(bool forceUpdate)
    {
        var blockTemplate = await blockchain.GetBlockTemplateAsync();
        var job = CreateJob(blockTemplate);

        if(job.Height > currentJob?.Height || forceUpdate)
        {
            currentJob = job;
            await BroadcastJob(job, forceUpdate);
        }
    }
}
