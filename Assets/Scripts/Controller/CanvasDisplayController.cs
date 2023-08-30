using TMPro;
using UnityEngine;

namespace Controller
{
    public class CanvasDisplayController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_bubbleCounterText;

        public void UpdateBubbleCounterText(int p_counter)
        {
            m_bubbleCounterText.text = p_counter.ToString();
        }
    }
}
