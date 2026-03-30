using Unity.Netcode;

namespace Data
{
    [System.Serializable]
    public struct PlayerType : INetworkSerializable
    {
        public PlayerRole role;
        public ElementType color;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref role);
            serializer.SerializeValue(ref color);
        }
    }

    public enum PlayerRole
    {
        Shooter, Supporter
    }
}
