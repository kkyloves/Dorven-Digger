using UnityEngine;

namespace Controller
{
    public class BubbleItemTriggerController : MonoBehaviour
    {
        private BubbleItemController m_bubbleItemController;

        private void Awake()
        {
            m_bubbleItemController = GetComponent<BubbleItemController>();
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Bottom Wall"))
            {
                //game over
                m_bubbleItemController.CollidedWithBottomWall();
            }

            if (col.CompareTag("Top Wall"))
            {
                m_bubbleItemController.ConnectToTopWall(true);
            }

            if (col.CompareTag("Bubble Item Puzzle"))
            {
                if (col.GetComponent<BubbleItemController>().transform.position.y > transform.position.y)
                {
                    var bubble = col.GetComponent<BubbleItemController>();
                    m_bubbleItemController.AddNeighboursConnectedToTopWall(bubble);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            if (col.CompareTag("Top Wall"))
            {
                m_bubbleItemController.ConnectToTopWall(false);
            }
        }
    }
}
