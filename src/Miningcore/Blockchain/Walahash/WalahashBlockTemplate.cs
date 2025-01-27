public class WalahashBlockTemplate
{
    public uint Version { get; set; }
    public string PreviousBlockHash { get; set; }
    public string MerkleRoot { get; set; }
    public uint Time { get; set; }
    public uint Height { get; set; }
    public uint Bits { get; set; }
    public double Difficulty { get; set; }
}
