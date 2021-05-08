using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class EnterExitWidget : MonoBehaviour
{
    [Serializable]
    public struct TweenSettings
    {
        public float Duration;
        public Ease Ease;
    }

    [SerializeField]
    private TweenSettings enter = new TweenSettings()
    {
        Duration = 0.5f,
        Ease = Ease.OutCubic
    };

    [SerializeField]
    private TweenSettings exit = new TweenSettings()
    {
        Duration = 0.5f,
        Ease = Ease.InCubic
    };

    private bool isInitialized;
    private bool isHidden;

    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    private Vector2 outsideScreenPosition;
    private Vector2 normalScreenPosition;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (isInitialized)
            return;

        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = (RectTransform) rectTransform.parent;
        normalScreenPosition = rectTransform.anchoredPosition;
        outsideScreenPosition = GetOutsideScreenPosition();
        isInitialized = true;
    }

    public void Show(Action onCompleteCallback = null)
    {
        Initialize();
        if (isHidden)
        {
            outsideScreenPosition = GetOutsideScreenPosition();
            rectTransform.anchoredPosition = outsideScreenPosition;
        }

        rectTransform.gameObject.SetActive(true);
        rectTransform.DOKill(false);

        isHidden = false;

        rectTransform
            .DOAnchorPos(normalScreenPosition, enter.Duration)
            .SetEase(enter.Ease)
            .OnComplete(() => onCompleteCallback?.Invoke());
    }

    public void Hide(Action onCompleteCallback = null)
    {
        Initialize();
        rectTransform.DOKill(false);

        outsideScreenPosition = GetOutsideScreenPosition();

        rectTransform
            .DOAnchorPos(outsideScreenPosition, exit.Duration)
            .SetEase(exit.Ease)
            .OnComplete(() =>
            {
                rectTransform.anchoredPosition = outsideScreenPosition;
                isHidden = true;
                gameObject.SetActive(false);
                onCompleteCallback?.Invoke();
            });
    }

    private Vector2 GetOutsideScreenPosition()
    {
        Vector2 result = normalScreenPosition;
        result.y = -parentRectTransform.rect.height
                   * Mathf.Max(rectTransform.anchorMin.y, rectTransform.anchorMax.y)
                   - (1 - rectTransform.pivot.y) * rectTransform.sizeDelta.y;

        return result;
    }
}