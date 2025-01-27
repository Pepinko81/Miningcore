public class WalahashPoolConfig : PoolConfig
{
    public int HashingIterations { get; set; }
    public string NetworkType { get; set; }
    public Dictionary<string, string> ExtraPoolParameters { get; set; }
}
