using UnityEngine;

namespace Manager
{
    public enum RotationMode
    {
        Left,
        Right
    }

    public enum MainMenuMode
    {
        Up,
        Down
    }
    
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private KeyCode m_rotateToLeft;
        [SerializeField] private KeyCode m_rotateToRight;
        [SerializeField] private KeyCode m_shoot;
        [SerializeField] private KeyCode m_mainMenuUp;
        [SerializeField] private KeyCode m_mainMenuDown;
        [SerializeField] private KeyCode m_mainMenuChoose;

        public delegate bool ArrowDetectionDelegate(RotationMode p_rotationMode);
        public delegate bool MainMenuArrowDetectionDelegate(MainMenuMode p_rotationMode);
        public delegate bool MainMenuChooseDetectionDelegate();
        public delegate bool ShootDetectionDelegate();

        private GameManager m_gameManager;
        
        /* Setters and getters */
        public ArrowDetectionDelegate ArrowDelegate { set; get; }
        public ShootDetectionDelegate ShootDelegate { set; get; }
        public MainMenuArrowDetectionDelegate MainMenuArrowDelegate { set; get; }
        public MainMenuChooseDetectionDelegate MainMenuChooseDelegate { set; get; }

        public void Init(GameManager p_gameManager)
        {
            m_gameManager = p_gameManager;
        }
        
        private void Update()
        {
            if (!m_gameManager.IsGameEnd)
            {
                if (Input.GetKeyDown(m_rotateToRight) || Input.GetKey(m_rotateToRight))
                {
                    ArrowDelegate(RotationMode.Right);
                }

                if (Input.GetKeyDown(m_rotateToLeft) || Input.GetKey(m_rotateToLeft))
                {
                    ArrowDelegate(RotationMode.Left);
                }

                if (Input.GetKeyDown(m_mainMenuUp))
                {
                    MainMenuArrowDelegate(MainMenuMode.Up);
                }

                if (Input.GetKeyDown(m_mainMenuDown))
                {
                    MainMenuArrowDelegate(MainMenuMode.Down);
                }

                if (Input.GetKeyDown(m_mainMenuChoose))
                {
                    MainMenuChooseDelegate();
                }

                if (Input.GetKeyDown(m_shoot))
                {
                    ShootDelegate();
                }
            }
        }
    }
}
