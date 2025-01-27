public class HoohashHasher
{
    public byte[] Hash(byte[] input)
    {
        using (var hash = new SHA512Managed())
        {
            return hash.ComputeHash(input);
        }
    }
}
