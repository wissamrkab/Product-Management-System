namespace PMS.Infrastructure.Common.Chat.Common
{
    public class PrivilegeMessage : IPackable
    {
        private uint _salt = (uint)Utils.RandomInt();
        private uint _ts = (uint)(Utils.GetTimestamp() + 24 * 3600);
        private Dictionary<ushort, uint> _messages = new();

        public ByteBuf marshal(ByteBuf outBuffer)
        {
            return outBuffer.put(_salt).put(_ts).putIntMap(_messages);
        }

        public void Unmarshal(ByteBuf inBuffer)
        {
            _salt = inBuffer.readInt();
            _ts = inBuffer.readInt();
            _messages = inBuffer.readIntMap();
        }
    }
}