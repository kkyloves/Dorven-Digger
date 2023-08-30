using System;
using Model;
using UnityEngine;

namespace Manager
{
    public class SpriteStorageManager : MonoBehaviour
    {
        public Sprite GreenGem;
        public Sprite BrownGem;
        public Sprite WhiteGem;
        public Sprite RedGem;
        public Sprite BlueGem;
        
        public Sprite GetGemSpriteByColor(BubbleColor p_color)
        {
            switch (p_color)
            {
                case BubbleColor.Brown: return BrownGem;
                case BubbleColor.White: return WhiteGem;
                case BubbleColor.Green: return GreenGem;
                case BubbleColor.Red: return RedGem;
                case BubbleColor.Blue: return BlueGem;
                default: return RedGem;
            }
        }

    }
}
