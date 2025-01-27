public class WalahashJob
{
    public bool ValidateShare(string nonce, string workerAddress, out BigInteger actualDifficulty)
    {
        var hashBytes = BuildHashInput(nonce, workerAddress);
        var hash = WalahashHasher.Hash(hashBytes);
        actualDifficulty = new BigInteger(hash);
        return actualDifficulty <= targetDifficulty;
    }
}
