using UnityEngine;
using UnityEngine.UI;

namespace RedAxeGames
{
    public class InputProgressBar : MonoBehaviour
    {
        [SerializeField] private Transform progressBarParent = null;
        [SerializeField] private Image progressImage = null;

        private void OnEnable()
        {
            RaycastPlayerInputTarget.OnInputHolding += HandleOnInputHolding;
            RaycastPlayerInputTarget.OnHoldInputCanceled += HandleOnInputCanceled;
        }

        private void OnDisable()
        {
            RaycastPlayerInputTarget.OnInputHolding -= HandleOnInputHolding;
            RaycastPlayerInputTarget.OnHoldInputCanceled -= HandleOnInputCanceled;
        }

        private void HandleOnInputHolding(float startTime, float endTime)
        {
            progressBarParent.gameObject.SetActive(true);

            //                                          Elapsed Time       /    Total Duration
            progressImage.fillAmount = Mathf.Clamp01((float)(Time.time - startTime) / (float)(endTime - startTime));

            if (progressImage.fillAmount >= 1.00f)
            {
                HandleOnInputCanceled();
            }
        }

        private void HandleOnInputCanceled()
        {
            progressImage.fillAmount = 0.00f;
            progressBarParent.gameObject.SetActive(false);
        }
    }
}