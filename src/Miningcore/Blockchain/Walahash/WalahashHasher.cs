public class WalahashHasher
{
    public byte[] Hash(byte[] input)
    {
        using (var hash = new SHA256Managed())
        {
            var result = hash.ComputeHash(input);
            return hash.ComputeHash(result); // Double SHA256
        }
    }
}
