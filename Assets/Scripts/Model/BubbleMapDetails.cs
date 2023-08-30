using UnityEngine;

namespace Model
{
    public class BubbleMapDetails : MonoBehaviour
    {
        public BubbleSpawnIntervalType BubbleSpawnIntervalType;
        public float SpawnIntervalForcedTimer;

        public int FixedRowCount = 5;
        public int FixedColumnCount = 5;
    }

    public enum BubbleSpawnIntervalType
    {
        Forced,
        Random
    }
}