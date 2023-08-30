using System;
using Controller;
using Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Manager
{
    public enum GameMode
    {
        Endless,
        Standard
    }
    
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private InputManager m_inputManager;
        [SerializeField] private SpriteStorageManager m_spriteStorageManager;
        [SerializeField] private BubbleShooterController m_bubbleShooterController;
        [SerializeField] private BubblePuzzleController m_bubblePuzzleController;
        [SerializeField] private PrefabStorageManager m_prefabStorageManager;
        [SerializeField] private WinLoseController m_winLoseController;
        [SerializeField] private CanvasDisplayController m_canvasDisplayController;
        [SerializeField] private TrajectoryShooterController m_trajectoryShooterController;
        [SerializeField] private MainMenuController m_mainMenuController;
        [SerializeField] private TutorialController m_tutorialController;

        public bool IsGameStart { get; private set; }
        public bool IsGameEnd { get; set; }
        public GameMode GameMode { get; private set; } = GameMode.Endless;

        private void Awake()
        {
            Time.timeScale = 1f;
            
            Init();
        }

        private void Init()
        {
            m_winLoseController.Init(m_canvasDisplayController, this);
            m_bubblePuzzleController.Init(m_winLoseController, m_spriteStorageManager, m_prefabStorageManager, this);
            m_bubbleShooterController.Init(this, m_inputManager, m_spriteStorageManager, m_prefabStorageManager, m_bubblePuzzleController, m_winLoseController);
            m_trajectoryShooterController.Init(m_inputManager);
            m_inputManager.Init(this);
            m_mainMenuController.Init(m_inputManager, m_tutorialController, this);
        }

        public void StartEndlessGame()
        {
            GameMode = GameMode.Endless;
            IsGameStart = true;
            
            m_bubblePuzzleController.StartGenerationBubblePuzzle();
            m_bubbleShooterController.InitializeBubbleBullets();
            m_trajectoryShooterController.StartTrajectoryPoints();
        }
        
        public void StartStandardGame()
        {
            GameMode = GameMode.Standard;
            IsGameStart = true;
            
            m_bubblePuzzleController.StartGenerationBubblePuzzle();
            m_bubbleShooterController.InitializeBubbleBullets();
            m_trajectoryShooterController.StartTrajectoryPoints();
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}
