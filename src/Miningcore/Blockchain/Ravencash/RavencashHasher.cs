public class RavencashHasher
{
    public byte[] Hash(byte[] input, uint height)
    {
        return KowPoWHash.Hash(input, height);
    }
}
