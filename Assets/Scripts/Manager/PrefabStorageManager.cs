using UnityEngine;

namespace Manager
{
    public class PrefabStorageManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_bubbleBulletController;
        public GameObject BubbleBulletController => m_bubbleBulletController;
        
        [SerializeField] private GameObject m_bubbleItemController;
        public GameObject BubbleItemController => m_bubbleItemController;
    }
}
