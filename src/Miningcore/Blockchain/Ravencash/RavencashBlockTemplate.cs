public class RavencashBlockTemplate
{
    public uint Version { get; set; }
    public string PreviousBlockHash { get; set; }
    public string[] Transactions { get; set; }
    public uint Height { get; set; }
    public uint Bits { get; set; }
    public double NetworkDifficulty { get; set; }
    public uint Time { get; set; }
}
