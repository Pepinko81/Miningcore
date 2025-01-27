public class WalahashBlockTemplateBuilder
{
    private readonly IBlockchainManager blockchain;

    public WalahashJob CreateNewJob(WalahashBlockTemplate template)
    {
        var job = new WalahashJob
        {
            JobId = Guid.NewGuid().ToString("N"),
            Height = template.Height,
            BlockTemplate = SerializeBlockHeader(template),
            Target = DifficultyToTarget(template.Difficulty),
            PreviousBlockHash = template.PreviousBlockHash,
            Time = template.Time
        };

        return job;
    }

    private byte[] SerializeBlockHeader(WalahashBlockTemplate template)
    {
        using(var stream = new MemoryStream())
        using(var writer = new BinaryWriter(stream))
        {
            writer.Write(template.Version);
            writer.Write(HexToBytes(template.PreviousBlockHash));
            writer.Write(HexToBytes(template.MerkleRoot));
            writer.Write(template.Time);
            writer.Write(template.Bits);

            return stream.ToArray();
        }
    }
}
