public class HoohashPoolConfig : PoolConfig
{
    public int HashingIterations { get; set; }
    public string NetworkType { get; set; }
    public Dictionary<string, string> PoolParameters { get; set; }
    public int MinimumDifficulty { get; set; }
    public int MaximumDifficulty { get; set; }
    public int DifficultyAdjustmentInterval { get; set; }
}
