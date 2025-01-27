public class WalahashShareProcessor
{
    private readonly IShareRepository shareRepo;
    private readonly IMapper mapper;

    public async Task ProcessShareAsync(Share share, IConnection connection)
    {
        var difficulty = share.Difficulty;
        var networkDifficulty = await GetNetworkDifficultyAsync();

        var shareMultiplier = difficulty / networkDifficulty;
        var reward = CalculateReward(shareMultiplier);

        await shareRepo.InsertAsync(new Share
        {
            PoolId = share.PoolId,
            BlockHeight = share.BlockHeight,
            Difficulty = difficulty,
            NetworkDifficulty = networkDifficulty,
            Miner = share.Miner,
            Worker = share.Worker,
            UserAgent = connection.UserAgent,
            IpAddress = connection.RemoteEndpoint.Address.ToString(),
            Source = share.Source,
            Created = DateTime.UtcNow
        });
    }
}
