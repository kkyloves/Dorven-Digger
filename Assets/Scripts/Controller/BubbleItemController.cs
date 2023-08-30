using Model;
using UnityEngine;
using View;

namespace Controller
{
    public class BubbleItemController : MonoBehaviour
    {
        [SerializeField] private LayerMask m_layerMastToCollide;
        
        private BubblePuzzleController m_bubblePuzzleController;
        private BubbleItemModel m_bubbleItemModel;
        private BubbleItemView m_bubbleItemView;
        private WinLoseController m_winLoseController;
        
        public int BubblePuzzleIndex { get; set; }
        
        public BubbleColor BubbleColor => m_bubbleItemModel.BubbleColor;
        public bool IsPopped { get; private set; }

        public void InitBubbleItemController(BubblePuzzleController p_bubblePuzzleController,
            WinLoseController pWinLoseController, Sprite p_sprite, BubbleColor p_color, int p_bubblePuzzleIndex)
        {
            m_bubblePuzzleController = p_bubblePuzzleController;
            m_winLoseController = pWinLoseController;

            var spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            var animator = gameObject.GetComponent<Animator>();
            m_bubbleItemView = new BubbleItemView(transform, spriteRenderer, animator);
            m_bubbleItemModel = new BubbleItemModel(this, p_color, m_layerMastToCollide);

            m_bubbleItemView.Init(p_sprite);

            BubblePuzzleIndex = p_bubblePuzzleIndex;
        }

        public int GetRelatedNeighboursCount()
        {
            return m_bubbleItemModel.GetRelatedNeighboursCount();
        }

        public void MoveDown()
        {
            m_bubbleItemView.MoveDown();
        }

        public void Pop()
        {
            IsPopped = true;
            m_bubblePuzzleController.RemoveBubbleItemController(this);
            m_winLoseController.AddBubblePopCounter();

            m_bubbleItemModel.Pop();
            m_bubbleItemView.Pop();
        }

        public void DestroyBubble()
        {
            Destroy(gameObject);
        }

        public void CheckNeighbourSupportFromTop(BubbleItemController p_bubbleItemController)
        {
            m_bubbleItemModel.CheckNeighbourSupportFromTop(p_bubbleItemController);
        }

        public void ConnectToTopWall(bool p_isConnectedToTopWall)
        {
            m_bubbleItemModel.IsConnectedToTopWall = p_isConnectedToTopWall;
        }

        public void AddNeighboursConnectedToTopWall(BubbleItemController p_bubbleItemController)
        {
            m_bubbleItemModel.AddNeighboursConnectedToTopWall(p_bubbleItemController);
        }

        public void CheckIfStillConnectedInTopWall()
        {
            m_bubbleItemModel.CheckIfStillConnectedInTopWall();
        }

        public void CollidedWithBottomWall()
        {
            m_winLoseController.GameOver();
        }
    }
}