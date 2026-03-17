using UnityEngine;
using CaudilloBay.World;

namespace CaudilloBay.Construction
{
    public class DistrictTool : MonoBehaviour
    {
        public static DistrictTool Instance { get; private set; }

        private Vector2Int startPos;
        private bool isSelecting = false;

        public Color districtColor = Color.blue;
        public string districtName = "New District";

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                StartSelection();
            }

            if (isSelecting && Input.GetMouseButtonUp(0))
            {
                EndSelection();
            }
        }

        private void StartSelection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                startPos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));
                isSelecting = true;
            }
        }

        private void EndSelection()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector2Int endPos = new Vector2Int(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));
                RectInt area = new RectInt(
                    Mathf.Min(startPos.x, endPos.x),
                    Mathf.Min(startPos.y, endPos.y),
                    Mathf.Abs(startPos.x - endPos.x) + 1,
                    Mathf.Abs(startPos.y - endPos.y) + 1
                );

                DistrictManager.Instance.CreateDistrict(districtName, districtColor, area);
            }
            isSelecting = false;
        }
    }
}
