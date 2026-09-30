using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SimpleMessageHeightFitter : UIBehaviour, ILayoutElement
{
    [SerializeField] float offset;
    [SerializeField] TextMeshProUGUI text;
    
    private float mPreferredHeight;
    private float mMinHeight;

    // We don't care about width, so we do nothing here
    public void CalculateLayoutInputHorizontal() { }

    // Sum up the heights of the assigned text components
    public void CalculateLayoutInputVertical()
    {
        mPreferredHeight = offset + text.preferredHeight;

        // 2. Safely calculate minHeight based on AutoSize settings
        if (text.enableAutoSizing && text.fontSizeMax > 0)
        {
            // Apply your ratio if AutoSize is on and Max is safe
            mMinHeight = offset + text.preferredHeight * (text.fontSizeMin / text.fontSizeMax);
        }
        else
        {
            // If AutoSize is off, minHeight should likely just match preferredHeight
            mMinHeight = mPreferredHeight; 
        }
    }

    // Return -1 for widths so this script ignores them and lets Unity handle them normally
    public float minWidth => -1;
    public float preferredWidth => -1;
    public float flexibleWidth => -1;

    // Apply our calculated height
    public float minHeight => mMinHeight;
    public float preferredHeight => mPreferredHeight;
    public float flexibleHeight => -1;
    
    // Priority 1 ensures this overrides native layout components if they exist
    public int layoutPriority => 1;

    // --- Boilerplate to ensure the UI updates instantly ---
    protected override void OnRectTransformDimensionsChange() => SetDirty();
    
    private void SetDirty()
    {
        if (IsActive()) LayoutRebuilder.MarkLayoutForRebuild(transform as RectTransform);
    }

#if UNITY_EDITOR
    protected override void OnValidate() => SetDirty();
    protected override void Reset(){
        base.Reset();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
#endif

}