

using PMS.Infrastructure.Common.Chat.Common;

namespace PMS.Infrastructure.Common.Chat
{
    public class AccessToken2
    {
        private const string Version = "007";
        private const int VersionLength = 3;

        private readonly string _appCert = "";
        private string _appId = "";
        private int _expire;
        private int _issueTs;
        private int _salt;
        private readonly Dictionary<ushort, Service> _services = new Dictionary<ushort, Service>();

        public AccessToken2()
        {
        }

        public AccessToken2(string appId, string appCert, int expire)
        {
            _appCert = appCert;
            _appId = appId;
            _expire = expire;
            _issueTs = Utils.GetTimestamp();
            _salt = Utils.RandomInt();
        }

        public void AddService(Service service)
        {
            _services.Add((ushort)service.GetServiceType(), service);
        }

        private const short ServiceTypeRtc = 1;
        private const short ServiceTypeRtm = 2;
        private const short ServiceTypeFpa = 4;
        private const short ServiceTypeChat = 5;
        private const short ServiceTypeEducation = 7;

        private static Service GetService(short serviceType)
        {
            if (serviceType == ServiceTypeRtc)
            {
                return new ServiceRtc();
            }

            if (serviceType == ServiceTypeRtm)
            {
                return new ServiceRtm();
            }

            if (serviceType == ServiceTypeFpa)
            {
                return new ServiceFpa();
            }

            if (serviceType == ServiceTypeChat)
            {
                return new ServiceChat();
            }

            if (serviceType == ServiceTypeEducation)
            {
                return new ServiceEducation();
            }

            throw new ArgumentException("unknown service type:", serviceType.ToString());
        }

        public static string GetUidStr(int uid)
        {
            if (uid == 0)
            {
                return "";
            }

            return (uid & 0xFFFFFFFFL).ToString();
        }

        private static string GetVersion()
        {
            return Version;
        }

        private byte[] GetSign()
        {
            byte[] signing = DynamicKeyUtil.encodeHMAC(BitConverter.GetBytes(_issueTs), _appCert.getBytes(), "SHA256");
            return DynamicKeyUtil.encodeHMAC(BitConverter.GetBytes(_salt), signing, "SHA256");
        }

        public string Build()
        {
            if (!Utils.IsUuid(_appId) || !Utils.IsUuid(_appCert))
            {
                return "";
            }

            ByteBuf buf = new ByteBuf().put(_appId.getBytes()).put((uint)_issueTs).put((uint)_expire).put((uint)_salt)
                .put((ushort)_services.Count);
            byte[] signing = GetSign();

            foreach (var it in _services)
            {
                it.Value.Pack(buf);
            }

            byte[] signature = DynamicKeyUtil.encodeHMAC(signing, buf.asBytes(), "SHA256");

            ByteBuf bufferContent = new ByteBuf();
            bufferContent.put(signature);
            bufferContent.copy(buf.asBytes());

            return GetVersion() + Utils.Base64Encode(Utils.Compress(bufferContent.asBytes()));
        }

        public bool Parse(string token)
        {
            if (string.Compare(GetVersion(), token.Substring(0, VersionLength), StringComparison.Ordinal) != 0)
            {
                return false;
            }

            var data = Utils.Decompress(Utils.Base64Decode(token.Substring(VersionLength)));

            var buff = new ByteBuf(data);
            var signature = buff.readBytes().getString();
            _appId = buff.readBytes().getString();
            _issueTs = (int)buff.readInt();
            _expire = (int)buff.readInt();
            _salt = (int)buff.readInt();
            var servicesNum = (short)buff.readShort();

            for (short i = 0; i < servicesNum; i++)
            {
                short serviceType = (short)buff.readShort();
                Service service = GetService(serviceType);
                service.Unpack(buff);
                _services.Add((ushort)serviceType, service);
            }

            return true;
        }

        public enum PrivilegeRtcEnum
        {
            PrivilegeJoinChannel = 1,
            PrivilegePublishAudioStream = 2,
            PrivilegePublishVideoStream = 3,
            PrivilegePublishDataStream = 4
        }

        public enum PrivilegeRtmEnum
        {
            PrivilegeLogin = 1
        }

        public enum PrivilegeFpaEnum
        {
            PrivilegeLogin = 1
        }

        public enum PrivilegeChatEnum
        {
            PrivilegeChatUser = 1,
            PrivilegeChatApp = 2
        }

        public enum PrivilegeEducationEnum
        {
            PrivilegeRoomUser = 1,
            PrivilegeUser = 2,
            PrivilegeApp = 3
        }

        public class Service
        {
            private short _type;
            private Dictionary<ushort, uint> _privileges = new Dictionary<ushort, uint>();

            protected Service()
            {
            }

            public Service(short serviceType)
            {
                _type = serviceType;
            }

            public void AddPrivilegeRtc(PrivilegeRtcEnum privilege, int expire)
            {
                _privileges.Add((ushort)privilege, (uint)expire);
            }

            public void AddPrivilegeRtm(PrivilegeRtmEnum privilege, int expire)
            {
                _privileges.Add((ushort)privilege, (uint)expire);
            }

            public void AddPrivilegeFpa(PrivilegeFpaEnum privilege, int expire)
            {
                _privileges.Add((ushort)privilege, (uint)expire);
            }

            public void AddPrivilegeChat(PrivilegeChatEnum privilege, int expire)
            {
                _privileges.Add((ushort)privilege, (uint)expire);
            }

            public void AddPrivilegeEducation(PrivilegeEducationEnum privilege, int expire)
            {
                _privileges.Add((ushort)privilege, (uint)expire);
            }

            public Dictionary<ushort, uint> GetPrivileges()
            {
                return _privileges;
            }

            public short GetServiceType()
            {
                return _type;
            }

            protected void SetServiceType(short type)
            {
                _type = type;
            }

            public virtual ByteBuf Pack(ByteBuf buf)
            {
                return buf.put((ushort)_type).putIntMap(_privileges);
            }

            public virtual void Unpack(ByteBuf byteBuf)
            {
                _privileges = byteBuf.readIntMap();
            }
        }

        private class ServiceRtc : Service
        {
            private string _channelName = null!;
            private string _uid = null!;

            public ServiceRtc()
            {
                SetServiceType(ServiceTypeRtc);
            }

            public ServiceRtc(string channelName, string uid)
            {
                SetServiceType(ServiceTypeRtc);
                _channelName = channelName;
                _uid = uid;
            }

            public string GetChannelName()
            {
                return _channelName;
            }

            public string GetUid()
            {
                return _uid;
            }

            public override ByteBuf Pack(ByteBuf buf)
            {
                return base.Pack(buf).put(_channelName.getBytes()).put(_uid.getBytes());
            }

            public override void Unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
                _channelName = byteBuf.readBytes().getString();
                _uid = byteBuf.readBytes().getString();
            }
        }

        private class ServiceRtm : Service
        {
            private string _userId = null!;

            public ServiceRtm()
            {
                SetServiceType(ServiceTypeRtm);
            }

            public ServiceRtm(string userId)
            {
                SetServiceType(ServiceTypeRtm);
                _userId = userId;
            }

            public string GetUserId()
            {
                return _userId;
            }

            public override ByteBuf Pack(ByteBuf buf)
            {
                return base.Pack(buf).put(_userId.getBytes());
            }

            public override void Unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
                _userId = byteBuf.readBytes().getString();
            }
        }

        private class ServiceFpa : Service
        {
            public ServiceFpa()
            {
                SetServiceType(ServiceTypeFpa);
            }

            public ByteBuf pack(ByteBuf buf)
            {
                return base.Pack(buf);
            }

            public void unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
            }
        }

        public class ServiceChat : Service
        {
            private string _userId;

            public ServiceChat()
            {
                SetServiceType(ServiceTypeChat);
                _userId = "";
            }

            public ServiceChat(string userId)
            {
                SetServiceType(ServiceTypeChat);
                _userId = userId;
            }

            public string GetUserId()
            {
                return _userId;
            }

            public override ByteBuf Pack(ByteBuf buf)
            {
                return base.Pack(buf).put(_userId.getBytes());
            }

            public override void Unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
                _userId = byteBuf.readBytes().getString();
            }
        }

        private class ServiceEducation : Service
        {
            private string _roomUuid;
            private string _userUuid;
            private short _role;

            public ServiceEducation()
            {
                SetServiceType(ServiceTypeEducation);
                _roomUuid = "";
                _userUuid = "";
                _role = -1;
            }

            public ServiceEducation(string roomUuid, string userUuid, short role)
            {
                SetServiceType(ServiceTypeEducation);
                _roomUuid = roomUuid;
                _userUuid = userUuid;
                _role = role;
            }

            public ServiceEducation(string userUuid)
            {
                SetServiceType(ServiceTypeEducation);
                _roomUuid = "";
                _userUuid = userUuid;
                _role = -1;
            }

            public string GetRoomUuid()
            {
                return _roomUuid;
            }

            public string GetUserUuid()
            {
                return _userUuid;
            }

            public short GetRole()
            {
                return _role;
            }

            public override ByteBuf Pack(ByteBuf buf)
            {
                return base.Pack(buf).put(_roomUuid.getBytes()).put(_userUuid.getBytes()).put((ushort)_role);
            }

            public override void Unpack(ByteBuf byteBuf)
            {
                base.Unpack(byteBuf);
                _roomUuid = byteBuf.readBytes().getString();
                _userUuid = byteBuf.readBytes().getString();
                _role = (short)byteBuf.readShort();
            }
        }
    }
}