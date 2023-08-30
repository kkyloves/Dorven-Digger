using System.Collections.Generic;
using Controller;
using Manager;
using UnityEngine;

namespace Model
{
    public class BubbleBulletController : MonoBehaviour
    {
        private const float TRAVEL_SPEED = 10f;

        private BubbleColor m_bubbleColor;
        private BubblePuzzleController m_bubblePuzzleController;
        private BubbleItemController m_bubbleItemController;
        private Rigidbody2D m_rigidBody;

        private bool m_hasHit;
        private bool m_move;
        private float m_angles;
        private bool m_revertDirection;

        private WinLoseController m_winLoseController;
        
        private int m_totalRelatedNeighboursCount;
        
        private void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody2D>();
            m_bubbleItemController = GetComponent<BubbleItemController>();
        }

        public void Init(SpriteStorageManager p_spriteStorageManager, BubblePuzzleController p_bubblePuzzleController,
            WinLoseController p_winLoseController)
        {
            m_bubbleColor = p_bubblePuzzleController.GetRandomAvailableColorInPuzzle();
            m_bubbleItemController.InitBubbleItemController(p_bubblePuzzleController, p_winLoseController,
                p_spriteStorageManager.GetGemSpriteByColor(m_bubbleColor), m_bubbleColor, 0);
            m_bubblePuzzleController = p_bubblePuzzleController;
            m_winLoseController = p_winLoseController;
        }

        public void CanNowMove(float p_angles)
        {
            m_angles = p_angles;
            m_move = true;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            {
                switch (col.tag)
                {
                    case "Side Wall":
                        m_revertDirection = true;
                        break;

                    case "Top Wall":
                        m_move = false;
                        AdjustPositionBasedNearestNeighbourInTopWall();
                        ChangeTagDelay();
                        break;

                    case "Bubble Item Puzzle":
                        m_move = false;

                        var bubbleItem = col.GetComponent<BubbleItemController>();
                        
                        CheckRelatedNeighbours(bubbleItem);
                        AdjustPositionBasedOnNeighbour(bubbleItem.transform);
                        ChangeTagDelay();
                        
                        break;
                }
            }
        }

        private void CheckRelatedNeighbours(BubbleItemController p_bubbleItemController)
        {
            if (m_bubbleColor.Equals(p_bubbleItemController.BubbleColor))
            {
                m_totalRelatedNeighboursCount += p_bubbleItemController.GetRelatedNeighboursCount();

                if (m_totalRelatedNeighboursCount >= 2)
                {
                    p_bubbleItemController.Pop();
                    m_winLoseController.AddBubblePopCounter();
                    Destroy(gameObject);
                }
            }
        }
        
        private void AdjustPositionBasedNearestNeighbourInTopWall()
        {
            var nearestNeighbourAtTopWall = m_bubblePuzzleController.GetTopWallMostSidedBubble(m_bubbleItemController);
            var neighbourPosition = nearestNeighbourAtTopWall.transform.position;
            
            transform.position = neighbourPosition.x < transform.position.x ? new Vector2(neighbourPosition.x + 0.4f, neighbourPosition.y + 0.4f) : 
                new Vector2(neighbourPosition.x - 0.4f, neighbourPosition.y + 0.4f);
            
            m_bubbleItemController.MoveDown();
            m_bubblePuzzleController.AddBubbleItemController(m_bubbleItemController);
        }

        private void AdjustPositionBasedOnNeighbour(Transform p_position)
        {
            transform.SetParent(m_bubblePuzzleController.transform);
            var neighbourPosition = p_position.position;

            //Left
            if (transform.position.x <= neighbourPosition.x)
            {
                //left
                if (transform.position.y < neighbourPosition.y + 0.3f &&
                    transform.position.y - 0.3 > neighbourPosition.y)
                {
                    transform.position = new Vector2(neighbourPosition.x - 0.4f, neighbourPosition.y);
                }
                //down left
                else if (transform.position.y < neighbourPosition.y)
                {
                    transform.position = new Vector2(neighbourPosition.x - 0.2f, neighbourPosition.y - 0.4f);
                }
                //down left
                else
                {
                    //m_bubbleNeighbourWithPosition.Add(NeighbourPosition.DownLeft, p_bubbleItemController);
                    //transform.position = new Vector2(neighbourPosition.x + 0.4f, neighbourPosition.y - 0.4f);
                }
            }
            //right
            else
            {
                //right
                if (transform.position.y < neighbourPosition.y + 0.3f &&
                    transform.position.y - 0.3 > neighbourPosition.y)
                {
                    //m_bubbleNeighbourWithPosition.Add(NeighbourPosition.Left, p_bubbleItemController);
                    // transform.position = new Vector2(neighbourPosition.x + 0.4f, neighbourPosition.y);
                }
                //down right
                else if (transform.position.y <= neighbourPosition.y)
                {
                    //m_bubbleNeighbourWithPosition.Add(NeighbourPosition.UpRight, p_bubbleItemController);
                    transform.position = new Vector2(neighbourPosition.x + 0.2f, neighbourPosition.y - 0.4f);
                }
                //down right
                else
                {
                    //m_bubbleNeighbourWithPosition.Add(NeighbourPosition.DownRight, p_bubbleItemController);
                    //transform.position = new Vector2(neighbourPosition.x + 0.4f, neighbourPosition.y + 0.4f);
                }
            }

            m_bubblePuzzleController.AddBubbleItemController(m_bubbleItemController);
        }

        private void ChangeTagDelay()
        {
            gameObject.tag = "Bubble Item Puzzle";
            var LayerIgnoreRaycast = LayerMask.NameToLayer("Bubble");
            gameObject.layer = LayerIgnoreRaycast;

            Destroy(GetComponent<BubbleBulletController>());
        }

        private void Update()
        {
            if (m_hasHit == false)
            {
                var angle = Mathf.Atan2(m_rigidBody.velocity.y, m_rigidBody.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }

            if (m_move)
            {
                if (!m_revertDirection)
                {
                    transform.Translate(
                        Vector3.right * Time.deltaTime * TRAVEL_SPEED * Mathf.Cos(Mathf.Deg2Rad * m_angles),
                        Space.World);
                    transform.Translate(
                        Vector3.up * Time.deltaTime * TRAVEL_SPEED * Mathf.Sin(Mathf.Deg2Rad * m_angles), Space.World);
                }
                else
                {
                    transform.Translate(Vector3.left * TRAVEL_SPEED * Mathf.Cos(Mathf.Deg2Rad * m_angles) *
                                        Time.deltaTime);
                    transform.Translate(
                        Vector3.up * TRAVEL_SPEED * Mathf.Sin(Mathf.Deg2Rad * m_angles) * Time.deltaTime);
                }
            }
        }
    }
}