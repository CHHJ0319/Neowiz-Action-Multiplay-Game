using System;
using Unity.Netcode;

namespace Data
{
    public enum ElementType
    {
        Red, Green, Blue, Yellow, Magenta, Cyan, Random
    }

    public struct NetworkElementType : INetworkSerializable, IEquatable<NetworkElementType>
    {
        public ElementType Type;

        public bool Equals(NetworkElementType other) => Type == other.Type;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Type);
        }

        public static implicit operator ElementType(NetworkElementType netType) => netType.Type;
        public static implicit operator NetworkElementType(ElementType type) => new NetworkElementType { Type = type };
    }
}

