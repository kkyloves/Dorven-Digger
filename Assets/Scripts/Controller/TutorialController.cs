using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controller
{
    public class TutorialController : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_tutorialItems;
        [SerializeField] private Button m_nextButton;

        private int m_nextCounter = 0;
        private bool m_fromMainMenu;

        private void Awake()
        {
            m_nextButton.onClick.AddListener(OnClickNext);
            gameObject.SetActive(false);
        }

        public void OpenTutorial(bool p_fromMainMenu = false)
        {
            m_fromMainMenu = p_fromMainMenu;
            gameObject.SetActive(true);
            m_nextCounter = 0;

            foreach (var tutorialItem in m_tutorialItems)
            {
                tutorialItem.SetActive(false);
            }
            
            m_tutorialItems[0].SetActive(true);
        }

        private void OnClickNext()
        {
            if (m_nextCounter >= m_tutorialItems.Length - 1)
            {
                if (m_fromMainMenu)
                {
                    SceneManager.LoadScene(0);
                }
            }
            else
            {
                m_tutorialItems[m_nextCounter].SetActive(false);
                m_nextCounter++;
                m_tutorialItems[m_nextCounter].SetActive(true);
            }
        }
    }
}