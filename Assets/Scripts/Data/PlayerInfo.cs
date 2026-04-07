using Unity.Netcode;

namespace Data
{
    [System.Serializable]
    public struct PlayerInfo : INetworkSerializable
    {
        public string playerName;
        public Data.CharacterType character;
        public Data.PlayerRole role;
        public Data.ElementType color;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref playerName);
            serializer.SerializeValue(ref character);
            serializer.SerializeValue(ref role);
            serializer.SerializeValue(ref color);
        }
    }

    public enum PlayerRole
    {
        Shooter, Supporter
    }

    public enum CharacterType
    {
        One, Two, Three, Four
    }
}
