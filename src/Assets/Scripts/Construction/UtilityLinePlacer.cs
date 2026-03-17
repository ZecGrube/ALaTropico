using UnityEngine;
using CaudilloBay.Systems;

namespace CaudilloBay.Construction
{
    public class UtilityLinePlacer : MonoBehaviour
    {
        public static UtilityLinePlacer Instance { get; private set; }

        public UtilityType activeType = UtilityType.Power;
        private UtilityNode selectedStartNode;
        private LineRenderer ghostLine;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            ghostLine = gameObject.AddComponent<LineRenderer>();
            ghostLine.enabled = false;
            ghostLine.widthMultiplier = 0.2f;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                HandleSelection();
            }

            if (selectedStartNode != null)
            {
                UpdateGhostLine();
                if (Input.GetMouseButtonDown(1)) CancelSelection();
            }
        }

        private void HandleSelection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                UtilityNode node = hit.collider.GetComponent<UtilityNode>();
                if (node != null && node.utilityType == activeType)
                {
                    if (selectedStartNode == null)
                    {
                        selectedStartNode = node;
                        ghostLine.enabled = true;
                    }
                    else if (selectedStartNode != node)
                    {
                        CompleteConnection(node);
                    }
                }
            }
        }

        private void UpdateGhostLine()
        {
            ghostLine.SetPosition(0, selectedStartNode.transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ghostLine.SetPosition(1, hit.point);
            }
        }

        private void CompleteConnection(UtilityNode endNode)
        {
            if (activeType == UtilityType.Power)
                PowerGridManager.Instance.CreateConnection(selectedStartNode, endNode);
            else
                WaterNetworkManager.Instance.CreateConnection(selectedStartNode, endNode);

            CancelSelection();
            Debug.Log($"Connected {selectedStartNode.name} to {endNode.name}");
        }

        private void CancelSelection()
        {
            selectedStartNode = null;
            ghostLine.enabled = false;
        }
    }
}
