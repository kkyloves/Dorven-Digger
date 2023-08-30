using Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controller
{
    public class WinLoseController : MonoBehaviour
    {
        [SerializeField] private Button m_mainMenuButton;
        [SerializeField] private TextMeshProUGUI m_totalBubblePopText;

        private int m_bubblePopCounter;

        private CanvasDisplayController m_canvasDisplayController;
        private GameManager m_gameManager;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Init(CanvasDisplayController p_canvasDisplayController, GameManager p_gameManager)
        {
            m_canvasDisplayController = p_canvasDisplayController;
            m_gameManager = p_gameManager;

            m_mainMenuButton.onClick.AddListener(OnClickMainMenuButton);
            m_bubblePopCounter = 0;
            m_canvasDisplayController.UpdateBubbleCounterText(m_bubblePopCounter);
        }

        private void OnClickMainMenuButton()
        {
            m_gameManager.RestartGame();
        }

        public void AddBubblePopCounter()
        {
            m_bubblePopCounter++;
            m_canvasDisplayController.UpdateBubbleCounterText(m_bubblePopCounter);
        }

        public void GameOver()
        {
            if (!m_gameManager.IsGameEnd)
            {
                gameObject.SetActive(true);
                m_gameManager.IsGameEnd = true;
                m_totalBubblePopText.text = m_bubblePopCounter.ToString();

                Invoke(nameof(PauseTime), 0.5f);
            }
        }

        private void PauseTime()
        {
            Time.timeScale = 0f;
        }
    }
}