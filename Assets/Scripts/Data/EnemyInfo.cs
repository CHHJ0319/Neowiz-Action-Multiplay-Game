using System;

namespace Data
{
    [Serializable]
    public struct EnemyInfo
    {
        public EnemyType type;
        public int lives;
    }

    public enum EnemyType
    {
        Single, Multy
    }
}

