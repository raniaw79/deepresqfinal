using UnityEngine;

// Scale the artwork and its buttons together rather than stretching the artwork alone.
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class FitUIToParent : MonoBehaviour
{
    public bool fillParent;
    private void OnEnable() { Refresh(); }
    private void OnRectTransformDimensionsChange() { Refresh(); }
    private void LateUpdate() { Refresh(); }

    public void Refresh()
    {
        var rect = (RectTransform)transform;
        var parent = rect.parent as RectTransform;
        if (parent == null || rect.rect.width <= 0 || rect.rect.height <= 0) return;
        float widthScale = parent.rect.width / rect.rect.width;
        float heightScale = parent.rect.height / rect.rect.height;
        float scale = fillParent ? Mathf.Max(widthScale, heightScale) : Mathf.Min(1f, Mathf.Min(widthScale, heightScale));
        if (scale <= 0) return;
        var desired = new Vector3(scale, scale, 1f);
        if ((rect.localScale - desired).sqrMagnitude > 0.000001f) rect.localScale = desired;
    }
}
