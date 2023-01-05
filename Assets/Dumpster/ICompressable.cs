public interface ICompressable
{
    public byte[] ToData();
    public void FromData(byte[] data);
}