using System;
using Unity.Netcode;

namespace Data
{
    public enum ElementType
    {
        Red, Green, Blue, Random
    }

    public struct NetworkElementType : INetworkSerializable, IEquatable<NetworkElementType>
    {
        public ElementType Value;

        public bool Equals(NetworkElementType other) => Value == other.Value;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Value);
        }

        public static implicit operator ElementType(NetworkElementType netType) => netType.Value;
        public static implicit operator NetworkElementType(ElementType type) => new NetworkElementType { Value = type };
    }
}

