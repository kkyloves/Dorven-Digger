using System;
using Manager;
using TMPro;
using UnityEngine;

namespace Controller
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_pointerChoices;
        [SerializeField] private TextMeshProUGUI m_versionNumber;

        private int m_pointerChoicesCounter;

        private GameManager m_gameManager;
        private TutorialController m_tutorialController;

        public void Init(InputManager p_inputManager, TutorialController p_tutorialController,
            GameManager p_gameManager)
        {
            m_gameManager = p_gameManager;
            m_tutorialController = p_tutorialController;
            m_versionNumber.text = "v" + Application.version;

            p_inputManager.MainMenuArrowDelegate += mode =>
            {
                switch (mode)
                {
                    case MainMenuMode.Up:
                        OnClickUp();
                        break;
                    case MainMenuMode.Down:
                        OnClickDown();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
                }

                return false;
            };

            p_inputManager.MainMenuChooseDelegate += () =>
            {
                ChooseMainMenu();
                return false;
            };


            foreach (var pointer in m_pointerChoices)
            {
                pointer.SetActive(false);
            }

            m_pointerChoices[m_pointerChoicesCounter].SetActive(true);
        }

        private void ChooseMainMenu()
        {
            switch (m_pointerChoicesCounter)
            {
                case 0:
                    //choose endless mode
                    m_gameManager.StartEndlessGame();
                    gameObject.SetActive(false);
                    break;
                case 1:
                    //choose standard mode
                    m_gameManager.StartStandardGame();
                    gameObject.SetActive(false);
                    break;
                case 2:
                    //choose tutorial mode
                    m_tutorialController.OpenTutorial(true);
                    gameObject.SetActive(false);
                    break;
                default:
                    //choose quit 
                    Application.Quit();
                    break;
            }
        }

        private void OnClickUp()
        {
            m_pointerChoices[m_pointerChoicesCounter].SetActive(false);

            m_pointerChoicesCounter--;

            if (m_pointerChoicesCounter <= 0)
            {
                m_pointerChoicesCounter = 0;
            }

            m_pointerChoices[m_pointerChoicesCounter].SetActive(true);
        }

        private void OnClickDown()
        {
            m_pointerChoices[m_pointerChoicesCounter].SetActive(false);

            m_pointerChoicesCounter++;

            if (m_pointerChoicesCounter >= m_pointerChoices.Length)
            {
                m_pointerChoicesCounter = m_pointerChoices.Length - 1;
            }

            m_pointerChoices[m_pointerChoicesCounter].SetActive(true);
        }

        private void ChooseSelection()
        {
        }
    }
}