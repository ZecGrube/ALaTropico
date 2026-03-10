using UnityEngine;
using UnityEngine.UI;
using CaudilloBay.Core;

namespace CaudilloBay.UI
{
    public class BuildingProgressBar : MonoBehaviour
    {
        public GameObject targetObject;
        public Image progressFill;
        public Canvas canvas;

        private IBuildable buildable;
        private Camera mainCamera;

        private void Start()
        {
            if (targetObject != null)
            {
                buildable = targetObject.GetComponent<IBuildable>();
            }
            mainCamera = Camera.main;
            canvas.worldCamera = mainCamera;
        }

        private void Update()
        {
            if (buildable == null || buildable.IsConstructed)
            {
                gameObject.SetActive(false);
                return;
            }

            // Face the camera
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                             mainCamera.transform.rotation * Vector3.up);

            // Update fill
            progressFill.fillAmount = buildable.CurrentProgress / buildable.ConstructionTime;
        }
    }
}
