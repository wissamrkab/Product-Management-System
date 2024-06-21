namespace PMS.Infrastructure.Common.Chat.Common
{
    public class PackContent(byte[] signature, uint crcChannelName, uint crcUid, byte[] rawMessage)
        : IPackable
    {
        public ByteBuf marshal(ByteBuf outBuffer)
        {
            return outBuffer.put(signature).put(crcChannelName).put(crcUid).put(rawMessage);
        }


        public void Unmarshal(ByteBuf inBuffer)
        {
            signature = inBuffer.readBytes();
            crcChannelName = inBuffer.readInt();
            crcUid = inBuffer.readInt();
            rawMessage = inBuffer.readBytes();
        }
    }
}