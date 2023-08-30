using UnityEngine;

namespace Model
{
    [System.Serializable]
    public enum BubbleColor
    {
        Blank,
        Random,
        Brown,
        White,
        Green,
        Red,
        Blue
    }
    
    public class BubbleInfo : MonoBehaviour
    {
        public int RowCount;
        public int ColumnCount;
        public bool IsActive;
        public BubbleColor initialBubbleColor;
    }
    
    public class Levels : MonoBehaviour {

        public Levels[] allLevels;

    }
}
