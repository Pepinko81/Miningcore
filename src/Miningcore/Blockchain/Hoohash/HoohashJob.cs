public class HoohashJob
{
    private readonly BigInteger targetDifficulty;
    private readonly byte[] blockTemplate;

    public bool ValidateShare(string nonce, string workerAddress, out BigInteger actualDifficulty)
    {
        var hashInput = BuildHashInput(nonce, workerAddress);
        var hash = HoohashHasher.Hash(hashInput);
        actualDifficulty = new BigInteger(hash);

        return actualDifficulty <= targetDifficulty;
    }

    private byte[] BuildHashInput(string nonce, string workerAddress)
    {
        using(var stream = new MemoryStream())
        {
            stream.Write(blockTemplate);
            stream.Write(Encoding.UTF8.GetBytes(nonce));
            stream.Write(Encoding.UTF8.GetBytes(workerAddress));
            return stream.ToArray();
        }
    }
}
