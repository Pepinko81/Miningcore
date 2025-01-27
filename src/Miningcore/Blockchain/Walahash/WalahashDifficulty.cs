public class WalahashDifficultyAdjuster
{
    private readonly ILogger logger;
    private const int TargetBlockTime = 150; // seconds

    public double GetNextDifficulty(WalahashWorkerContext context, double networkDifficulty)
    {
        var timeDelta = (DateTime.UtcNow - context.LastShare).TotalSeconds;
        var variance = timeDelta / TargetBlockTime;

        var nextDifficulty = context.Difficulty;

        if(variance < 0.8)
            nextDifficulty *= 1.2;
        else if(variance > 1.2)
            nextDifficulty *= 0.8;

        // Clamp difficulty
        nextDifficulty = Math.Min(Math.Max(nextDifficulty, poolConfig.MinimumDifficulty), networkDifficulty);

        return nextDifficulty;
    }
}
