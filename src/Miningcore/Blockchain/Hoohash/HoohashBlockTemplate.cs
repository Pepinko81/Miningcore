public class HoohashBlockTemplate
{
    public uint Version { get; set; }
    public string PreviousBlockHash { get; set; }
    public string MerkleRoot { get; set; }
    public uint Timestamp { get; set; }
    public uint Height { get; set; }
    public uint Bits { get; set; }
    public double NetworkDifficulty { get; set; }
    public string[] Transactions { get; set; }
}
