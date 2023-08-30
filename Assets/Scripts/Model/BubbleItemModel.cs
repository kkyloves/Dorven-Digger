using System.Collections.Generic;
using Controller;
using UnityEngine;

namespace Model
{
    public class BubbleItemModel
    {
        public BubbleColor BubbleColor { get; }
        public bool IsConnectedToTopWall { get; set; }
        
        private readonly BubbleItemController m_bubbleItemController;
        private readonly LayerMask m_layerMaskBubbles;
        
        private readonly List<BubbleItemController> m_neighboursConnectedTop = new();

        public BubbleItemModel(BubbleItemController p_bubbleItemController, BubbleColor p_color, LayerMask p_layerMaskBubbles)
        {
            m_layerMaskBubbles = p_layerMaskBubbles;
            m_bubbleItemController = p_bubbleItemController;
            BubbleColor = p_color;
        }

        public void CheckIfStillConnectedInTopWall()
        {
            if (!IsConnectedToTopWall)
            {
                if (m_neighboursConnectedTop.Count <= 0)
                {
                    Pop();
                }
            }
        }

        public void AddNeighboursConnectedToTopWall(BubbleItemController p_bubbleItemController)
        {
            m_neighboursConnectedTop.Add(p_bubbleItemController);
        }

        public int GetRelatedNeighboursCount()
        {
            var scannedNeighbours = ScanNeighbours();
            var relatedNeighboursCounter = 0;

            foreach (var neighbour in scannedNeighbours)
            {
                if (neighbour.BubbleColor.Equals(m_bubbleItemController.BubbleColor))
                {
                    relatedNeighboursCounter++;
                }
            }

            return relatedNeighboursCounter;
        }

        public void Pop()
        {
            var scannedNeighbours = ScanNeighbours();
            var otherNeighbours = new List<BubbleItemController>();

            //also pop the neighbours that has same colour
            foreach (var neighbour in scannedNeighbours)
            {
                if (neighbour.BubbleColor.Equals(m_bubbleItemController.BubbleColor))
                {
                    neighbour.Pop();
                }
                else
                {
                    otherNeighbours.Add(neighbour);
                }
            }

            //check if the neighbours connected below has still neighbours to connect or pop if they dont have
            foreach (var neighbour in otherNeighbours)
            {
                neighbour.CheckNeighbourSupportFromTop(m_bubbleItemController);
            }
            
            m_bubbleItemController.DestroyBubble();
        }

        public void CheckNeighbourSupportFromTop(BubbleItemController p_bubbleItemController)
        {
            if (!IsConnectedToTopWall)
            {
                m_neighboursConnectedTop.Remove(p_bubbleItemController);
            }
            
            CheckIfStillConnectedInTopWall();
        }
        
        private List<BubbleItemController> ScanNeighbours()
        {
            var objectsDetected = Physics2D.OverlapCircleAll(m_bubbleItemController.transform.position, 0.3f, m_layerMaskBubbles);
            var neighbours = new List<BubbleItemController>();

            if (objectsDetected.Length > 0)
            {
                foreach (var bubbleItem in objectsDetected)
                {
                    var collidedNeighbour = bubbleItem.GetComponent<BubbleItemController>();
                    if (collidedNeighbour != null)
                    {
                        if (!collidedNeighbour.IsPopped)
                        {
                            neighbours.Add(collidedNeighbour);
                        }
                    } 
                }
            }

            return neighbours;
        }
    }
}
