public class HoohashShareProcessor
{
    private readonly IShareRepository shareRepo;
    private readonly ILogger logger;
    private readonly IMapper mapper;

    public async Task ProcessShareAsync(Share share, IConnection connection)
    {
        var networkDifficulty = await GetNetworkDifficultyAsync();
        var shareValue = CalculateShareValue(share.Difficulty, networkDifficulty);

        var persistentShare = new Share
        {
            PoolId = share.PoolId,
            BlockHeight = share.BlockHeight,
            Difficulty = share.Difficulty,
            NetworkDifficulty = networkDifficulty,
            Miner = share.Miner,
            Worker = share.Worker,
            UserAgent = connection.UserAgent,
            IpAddress = connection.RemoteEndpoint.Address.ToString(),
            ShareValue = shareValue,
            Created = DateTime.UtcNow
        };

        await shareRepo.InsertAsync(persistentShare);
        logger.Info($"Processed share from {share.Worker} with difficulty {share.Difficulty}");
    }

    private decimal CalculateShareValue(double shareDifficulty, double networkDifficulty)
    {
        return (decimal)(shareDifficulty / networkDifficulty * BlockReward);
    }
}
