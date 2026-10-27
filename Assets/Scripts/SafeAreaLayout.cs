using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class SafeAreaLayout : MonoBehaviour
{
    private Rect lastSafeArea;
    private Vector2Int lastScreen;

    private void OnEnable() { Refresh(); }
    private void Update()
    {
        if (lastSafeArea != Screen.safeArea || lastScreen != new Vector2Int(Screen.width, Screen.height))
            Refresh();
    }

    private void Refresh()
    {
        if (!Application.isPlaying)
        {
            ApplyNormalizedArea(new Rect(0f, 0f, 1f, 1f));
            return;
        }
        if (Screen.width <= 0 || Screen.height <= 0) return;
        lastScreen = new Vector2Int(Screen.width, Screen.height);
        lastSafeArea = Screen.safeArea;
        ApplyNormalizedArea(new Rect(lastSafeArea.x / Screen.width, lastSafeArea.y / Screen.height,
            lastSafeArea.width / Screen.width, lastSafeArea.height / Screen.height));
    }

    public void ApplyNormalizedArea(Rect area)
    {
        var rect = (RectTransform)transform;
        rect.anchorMin = new Vector2(Mathf.Clamp01(area.xMin), Mathf.Clamp01(area.yMin));
        rect.anchorMax = new Vector2(Mathf.Clamp01(area.xMax), Mathf.Clamp01(area.yMax));
        rect.offsetMin = rect.offsetMax = Vector2.zero;
    }
}
