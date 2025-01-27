public class RavencashShareProcessor
{
    private readonly IShareRepository shareRepo;

    public async Task ProcessShareAsync(Share share, IConnection connection)
    {
        var networkDifficulty = await GetNetworkDifficultyAsync();
        var shareValue = CalculateShareValue(share.Difficulty, networkDifficulty);

        await shareRepo.InsertAsync(new Share
        {
            PoolId = share.PoolId,
            BlockHeight = share.BlockHeight,
            Difficulty = share.Difficulty,
            NetworkDifficulty = networkDifficulty,
            Miner = share.Miner,
            Worker = share.Worker,
            ShareValue = shareValue,
            Created = DateTime.UtcNow
        });
    }
}
