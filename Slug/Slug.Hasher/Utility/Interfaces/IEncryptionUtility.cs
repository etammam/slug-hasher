namespace Slug.Hasher.Utility.Interfaces
{
    public interface IEncryptionUtility
    {
        string Decrypt(string input);
        string Encrypt(string input);
        byte[] CombineBytes(byte[] buffer1, byte[] buffer2);
    }
}