using System;

namespace NVorbis.Contracts.Ogg
{
    interface IPacketReader
    {
        Il2CppSystem.Memory<byte> GetPacketData(int pagePacketIndex);

        void InvalidatePacketCache(IPacket packet);
    }
}
