namespace PMS.Infrastructure.Common.Chat.Common
{
    public class ByteBuffer
    {
        private const int MaxLength = 1024;
        
        private readonly byte[] _tempByteArray = new byte[MaxLength];
        
        private int _currentLength;
        
        private int _currentPosition;
        
        private byte[] _returnArray = null!;
        
        public ByteBuffer()
        {
            Initialize();
        }
        
        public ByteBuffer(byte[] bytes)
        {
            Initialize();
            PushByteArray(bytes);
        }
        
        public int Length => _currentLength;

        public int Position
        {
            get => _currentPosition;
            set => _currentPosition = value;
        }
        
        public byte[] ToByteArray()
        {
            _returnArray = new byte[_currentLength];
            Array.Copy(_tempByteArray, 0, _returnArray, 0, _currentLength);
            return _returnArray;
        }

        private void Initialize()
        {
            _tempByteArray.Initialize();
            _currentLength = 0;
            _currentPosition = 0;
        }
        
        public void PushByte(byte by)
        {
            _tempByteArray[_currentLength++] = by;
        }
        
        public void PushByteArray(byte[] byteArray)
        {
            byteArray.CopyTo(_tempByteArray, _currentLength);
            _currentLength += byteArray.Length;
        }
        
        public void PushUInt16(UInt16 num)
        {
            _tempByteArray[_currentLength++] = (byte)((num & 0x00ff) & 0xff);
            _tempByteArray[_currentLength++] = (byte)(((num & 0xff00) >> 8) & 0xff);
        }


        public void PushInt(UInt32 num)
        {
            _tempByteArray[_currentLength++] = (byte)((num & 0x000000ff) & 0xff);
            _tempByteArray[_currentLength++] = (byte)(((num & 0x0000ff00) >> 8) & 0xff);
            _tempByteArray[_currentLength++] = (byte)(((num & 0x00ff0000) >> 16) & 0xff);
            _tempByteArray[_currentLength++] = (byte)(((num & 0xff000000) >> 24) & 0xff);
        }
        
        public void PushLong(long num)
        {
            _tempByteArray[_currentLength++] = (byte)((num & 0x000000ff) & 0xff);
            _tempByteArray[_currentLength++] = (byte)(((num & 0x0000ff00) >> 8) & 0xff);
            _tempByteArray[_currentLength++] = (byte)(((num & 0x00ff0000) >> 16) & 0xff);
            _tempByteArray[_currentLength++] = (byte)(((num & 0xff000000) >> 24) & 0xff);
        }
        
        public byte PopByte()
        {
            byte ret = _tempByteArray[_currentPosition++];
            return ret;
        }
        
        public UInt16 PopUInt16()
        {
            if (_currentPosition + 1 >= _currentLength)
            {
                return 0;
            }
            
            UInt16 ret = (UInt16)(_tempByteArray[_currentPosition] | _tempByteArray[_currentPosition + 1] << 8);
            _currentPosition += 2;
            return ret;
        }
        
        public uint PopUInt()
        {
            if (_currentPosition + 3 >= _currentLength)
                return 0;
            uint ret = (uint)(_tempByteArray[_currentPosition] | _tempByteArray[_currentPosition + 1] << 8 |
                              _tempByteArray[_currentPosition + 2] << 16 |
                              _tempByteArray[_currentPosition + 3] << 24);
            _currentPosition += 4;
            return ret;
        }
        
        public long PopLong()
        {
            if (_currentPosition + 3 >= _currentLength)
                return 0;
            var ret = (long)(_tempByteArray[_currentPosition] << 24 | _tempByteArray[_currentPosition + 1] << 16 |
                             _tempByteArray[_currentPosition + 2] << 8 | _tempByteArray[_currentPosition + 3]);
            _currentPosition += 4;
            return ret;
        }
        
        public byte[] PopByteArray(int length)
        {
            if (_currentPosition + length > _currentLength)
            {
                return [];
            }

            var ret = new byte[length];
            Array.Copy(_tempByteArray, _currentPosition, ret, 0, length);

            _currentPosition += length;
            return ret;
        }

        public byte[] PopByteArray2(int length)
        {
            if (_currentPosition <= length)
            {
                return [];
            }

            var ret = new byte[length];
            Array.Copy(_tempByteArray, _currentPosition - length, ret, 0, length);
            _currentPosition -= length;
            return ret;
        }
    }
}