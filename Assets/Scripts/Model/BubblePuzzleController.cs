using System.Collections.Generic;
using System.Linq;
using Controller;
using Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Model
{
    public class BubblePuzzleController : MonoBehaviour
    {
        private const float CELL_SIZE = 0.5f;
        private const float INITIAL_TIME_TO_START_SPAWNING_BUBBLES = 5f;
        private const int FIXED_SPAWNING_COLUMN_COUNT = 1;

        [SerializeField] private BubbleMapDetails m_bubbleMapDetails;
        [SerializeField] private Transform m_spawnPoint;
        [SerializeField] private Transform m_topWall;

        private GameObject m_bubblePrefab;
        private SpriteStorageManager m_spriteStorageManager;
        private WinLoseController m_winLoseController;
        private GameManager m_gameManger;

        private bool m_toAdjustPositionX;
        private bool m_topWallCanMove;

        public List<BubbleItemController> m_bubblePuzzlesInBoard = new();
        private readonly List<BubbleItemController> m_topWallRowSet = new();

        public void StartGenerationBubblePuzzle()
        {
            GenerateRandomGameModeBubbleMatrix();
            InvokeRepeating(nameof(BubblePuzzleNullCleaner), 0f, 0.1f);
        }

        public void Init(WinLoseController p_winLoseController, SpriteStorageManager p_spriteStorageManager,
            PrefabStorageManager p_prefabStorage, GameManager p_gameManager)
        {
            m_winLoseController = p_winLoseController;
            m_spriteStorageManager = p_spriteStorageManager;
            m_gameManger = p_gameManager;

            m_bubblePrefab = p_prefabStorage.BubbleItemController;
        }

        private void BubblePuzzleNullCleaner()
        {
            for (var i = 0; i < m_bubblePuzzlesInBoard.Count; i++)
            {
                if (m_bubblePuzzlesInBoard[i] == null)
                {
                    m_bubblePuzzlesInBoard.RemoveAt(i);
                }
            }   
        }

        public void AddBubbleItemController(BubbleItemController p_bubbleItemController)
        {
            if (!m_bubblePuzzlesInBoard.Contains(p_bubbleItemController))
            {
                p_bubbleItemController.BubblePuzzleIndex = m_bubblePuzzlesInBoard.Count;
                m_bubblePuzzlesInBoard.Add(p_bubbleItemController);
            }
        }

        public void RemoveBubbleItemController(BubbleItemController p_bubbleItemController)
        {
            m_bubblePuzzlesInBoard.Remove(p_bubbleItemController);

            
            if (m_bubblePuzzlesInBoard.Count <= 0)
            {
                m_winLoseController.GameOver();
            }
        }

        private void GenerateRandomGameModeBubbleMatrix()
        {
            SetBubblePosition(m_bubbleMapDetails.FixedColumnCount, m_bubbleMapDetails.FixedRowCount);

            InvokeRepeating(nameof(InstantiateNewBubbleModel), INITIAL_TIME_TO_START_SPAWNING_BUBBLES,
                m_bubbleMapDetails.BubbleSpawnIntervalType.Equals(BubbleSpawnIntervalType.Forced)
                    ? m_bubbleMapDetails.SpawnIntervalForcedTimer
                    : Random.Range(0f, 10f));
        }

        public BubbleColor GetRandomAvailableColorInPuzzle()
        {
            List<BubbleColor> colors = new();

            foreach (var bubbleItemController in m_bubblePuzzlesInBoard)
            {
                if (!colors.Contains(bubbleItemController.BubbleColor))
                {
                    colors.Add(bubbleItemController.BubbleColor);
                }
            }

            if (colors.Count <= 0)
            {
                return 0;
            }

            var random = Random.Range(0, colors.Count);
            return colors[random];
        }

        private void SetBubblePosition(float p_columnCount, float p_rowCount)
        {
            for (var i = 0; i < p_columnCount; i++)
            {
                m_toAdjustPositionX = !m_toAdjustPositionX;
                m_topWallRowSet.Clear();

                for (var j = 0; j < p_rowCount; j++)
                {
                    var posX = j * CELL_SIZE - 1f;
                    if (m_toAdjustPositionX)
                    {
                        posX -= CELL_SIZE / 2;
                    }

                    var bubbleItemController =
                        InstantiateBubbleModel(posX, m_spawnPoint.position.y, BubbleColor.Random);

                    m_bubblePuzzlesInBoard.Add(bubbleItemController);
                    m_topWallRowSet.Add(bubbleItemController);
                }

                MoveDownRowSets();
            }

            if (m_gameManger.GameMode.Equals(Manager.GameMode.Standard))
            {
                m_topWallCanMove = true;
            }
        }

        private BubbleItemController InstantiateBubbleModel(float p_positionX, float p_positionY, BubbleColor p_color)
        {
            var obj = Instantiate(m_bubblePrefab, transform, true);
            var roundedValue = Mathf.Round(p_positionY * 100f) / 100f;
            obj.transform.position = new Vector2(p_positionX, roundedValue);

            //add and init bubble controller components
            var bubbleItemController = obj.GetComponent<BubbleItemController>();

            Sprite gemSprite;
            if (p_color.Equals(BubbleColor.Random))
            {
                var randomIndex = Random.Range(3, 7);
                p_color = (BubbleColor)randomIndex;
                gemSprite = m_spriteStorageManager.GetGemSpriteByColor(p_color);
            }
            else
            {
                gemSprite = m_spriteStorageManager.GetGemSpriteByColor(p_color);
            }

            bubbleItemController.InitBubbleItemController(this, m_winLoseController, gemSprite, p_color,
                m_bubblePuzzlesInBoard.Count);
            return bubbleItemController;
        }

        private void InstantiateNewBubbleModel()
        {
            if (m_gameManger.GameMode.Equals(Manager.GameMode.Endless))
            {
                SetBubblePosition(FIXED_SPAWNING_COLUMN_COUNT, m_bubbleMapDetails.FixedRowCount);
            }
            else
            {
                MoveDownRowSets();
            }
        }

        private void MoveDownRowSets()
        {
            foreach (var bubbleItemController in m_bubblePuzzlesInBoard)
            {
                if (bubbleItemController != null)
                {
                    bubbleItemController.MoveDown();
                }
            }

            if (m_topWallCanMove)
            {
                m_topWall.transform.position =
                    new Vector2(m_topWall.transform.position.x, m_topWall.transform.position.y - 0.4f);
            }
        }

        public BubbleItemController GetTopWallMostSidedBubble(BubbleItemController p_bubbleItemController)
        {
            foreach (var bubbleItem in m_topWallRowSet)
            {
                if (bubbleItem != null)
                {
                    var bubbleI = bubbleItem.transform.position;

                    if (bubbleI.x > p_bubbleItemController.transform.position.x)
                    {
                        m_topWallRowSet.Insert(0, p_bubbleItemController);

                        return bubbleItem;
                    }

                    for (var j = m_topWallRowSet.Count - 1; j >= 0; j--)
                    {
                        if (m_topWallRowSet[j] != null)
                        {
                            var bubbleJ = m_topWallRowSet[j].transform.position;

                            if (bubbleJ.x < p_bubbleItemController.transform.position.x)
                            {
                                m_topWallRowSet.Add(p_bubbleItemController);
                                return m_topWallRowSet[j];
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}