namespace PMS.Infrastructure.Common.Chat.Common
{
    public interface IPackable
    {
        ByteBuf marshal(ByteBuf outBuffer);
    }
}