public class HoohashDifficultyAdjuster
{
    private readonly ILogger logger;
    private const int TargetShareTime = 120; // seconds
    private const double RetargetInterval = 300; // 5 minutes

    public double GetNextDifficulty(HoohashWorkerContext context, double networkDifficulty)
    {
        var shareTimespan = (DateTime.UtcNow - context.LastShare).TotalSeconds;
        var ratio = shareTimespan / TargetShareTime;

        var nextDifficulty = context.Difficulty;

        // Adjust difficulty based on share submission rate
        if(ratio < 0.7)
            nextDifficulty *= 1.3;
        else if(ratio > 1.3)
            nextDifficulty *= 0.7;

        // Apply bounds
        nextDifficulty = Math.Min(Math.Max(nextDifficulty, poolConfig.MinimumDifficulty), networkDifficulty);

        logger.Debug($"Adjusted difficulty to {nextDifficulty} for worker {context.WorkerAddress}");

        return nextDifficulty;
    }
}
