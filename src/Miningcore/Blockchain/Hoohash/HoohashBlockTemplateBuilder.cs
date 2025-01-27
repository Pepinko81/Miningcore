public class HoohashBlockTemplateBuilder
{
    private readonly IBlockchainManager blockchain;
    private readonly ILogger logger;

    public HoohashJob CreateNewJob(HoohashBlockTemplate template)
    {
        var job = new HoohashJob
        {
            JobId = Guid.NewGuid().ToString("N"),
            Height = template.Height,
            BlockTemplate = BuildBlockHeader(template),
            Target = CalculateTarget(template.NetworkDifficulty),
            PreviousBlockHash = template.PreviousBlockHash,
            Timestamp = template.Timestamp,
            Transactions = template.Transactions
        };

        return job;
    }

    private byte[] BuildBlockHeader(HoohashBlockTemplate template)
    {
        using(var stream = new MemoryStream())
        using(var writer = new BinaryWriter(stream))
        {
            writer.Write(template.Version);
            writer.Write(HexStringToByteArray(template.PreviousBlockHash));
            writer.Write(HexStringToByteArray(template.MerkleRoot));
            writer.Write(template.Timestamp);
            writer.Write(template.Bits);

            return stream.ToArray();
        }
    }

    private byte[] CalculateTarget(double networkDifficulty)
    {
        var difficulty1 = BigInteger.Parse("00000000ffff0000000000000000000000000000000000000000000000000000", NumberStyles.HexNumber);
        var target = difficulty1 / new BigInteger(networkDifficulty);
        return target.ToByteArray();
    }
}
