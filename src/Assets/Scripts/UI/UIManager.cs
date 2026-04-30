using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CaudilloBay.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public CanvasGroup overlayFade;
        private Stack<GameObject> windowStack = new Stack<GameObject>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public void OpenWindow(GameObject window)
        {
            if (windowStack.Count > 0)
                windowStack.Peek().SetActive(false);

            windowStack.Push(window);
            window.SetActive(true);
            StartCoroutine(FadeIn(window.GetComponent<CanvasGroup>()));
        }

        public void CloseCurrentWindow()
        {
            if (windowStack.Count == 0) return;

            GameObject current = windowStack.Pop();
            StartCoroutine(FadeOut(current.GetComponent<CanvasGroup>(), () => {
                current.SetActive(false);
                if (windowStack.Count > 0)
                    windowStack.Peek().SetActive(true);
            }));
        }

        private IEnumerator FadeIn(CanvasGroup cg)
        {
            if (cg == null) yield break;
            cg.alpha = 0;
            while (cg.alpha < 1)
            {
                cg.alpha += Time.unscaledDeltaTime * 5f;
                yield return null;
            }
        }

        private IEnumerator FadeOut(CanvasGroup cg, System.Action onComplete)
        {
            if (cg == null) { onComplete?.Invoke(); yield break; }
            while (cg.alpha > 0)
            {
                cg.alpha -= Time.unscaledDeltaTime * 5f;
                yield return null;
            }
            onComplete?.Invoke();
        }
    }
}
