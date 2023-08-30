using Manager;
using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class BubbleShooterController : MonoBehaviour
    {
        private const float BUBBLE_BULLET_SPAWN_TIME = 0.5f;
        [SerializeField] private Transform m_launchPosition;

        private BubbleShooterView m_bubbleShooterView;
        private BubbleShooterModel m_bubbleShooterModel;

        private SpriteStorageManager m_spriteStorageManager;
        private BubblePuzzleController m_bubblePuzzleController;
        private GameObject m_bubbleBulletPrefab;

        private BubbleBulletController m_bubbleBulletController;
        private WinLoseController m_winLoseController;
        private GameManager m_gameManager;

        public void Init(GameManager p_gameManager, InputManager p_inputManager, SpriteStorageManager p_spriteStorageManager,
            PrefabStorageManager p_prefabStorageManager, BubblePuzzleController p_bubblePuzzleController, WinLoseController p_winLoseController)
        {
            m_gameManager = p_gameManager;
            m_bubbleShooterView = new BubbleShooterView(transform);
            m_bubbleShooterModel = new BubbleShooterModel(this, m_bubbleShooterView, p_inputManager);

            m_bubbleBulletPrefab = p_prefabStorageManager.BubbleBulletController;

            m_winLoseController = p_winLoseController;
            m_spriteStorageManager = p_spriteStorageManager;
            m_bubblePuzzleController = p_bubblePuzzleController;
        }

        public void InitializeBubbleBullets()
        {
            Invoke(nameof(ProcessInitializeBubbleBullets), BUBBLE_BULLET_SPAWN_TIME);
        }
        
        public void ProcessInitializeBubbleBullets()
        {
            var bullet = Instantiate(m_bubbleBulletPrefab, m_launchPosition.position, m_bubbleShooterView.GetCurrentRotation());
            m_bubbleBulletController = bullet.GetComponent<BubbleBulletController>();
            m_bubbleBulletController.Init(m_spriteStorageManager, m_bubblePuzzleController, m_winLoseController);
            bullet.transform.SetParent(transform);

            m_bubbleShooterModel.CanShoot = true;
        }

        public void ShootBubbles()
        {
            if (m_gameManager.IsGameStart && !m_gameManager.IsGameEnd)
            {
                if (m_bubbleBulletController.GetComponent<BubbleBulletController>() != null)
                {
                    m_bubbleBulletController.transform.parent = null;
                    m_bubbleBulletController.CanNowMove(m_bubbleShooterModel.GetShootingRotation());
                    InitializeBubbleBullets();
                }
            }
        }
    }
}