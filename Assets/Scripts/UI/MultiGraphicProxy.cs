using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A non-rendering UI Graphic proxy. 
/// Plug this into a Button's "Target Graphic" field, and list the actual graphics you want tinted.
/// It detects color changes on its own CanvasRenderer (from CrossFadeColor) and propagates them to the target graphics.
/// </summary>
[ExecuteAlways]
public class MultiGraphicProxy : Graphic {
    [Tooltip("Graphics that will inherit the color changes of this proxy.")]
    [SerializeField] private Graphic[] targetGraphics;

    private Color lastColor;

    public override Color color {
        get => base.color;
        set {
            base.color = value;
            PropagateColor(value);
        }
    }

    protected override void Start() {
        base.Start();
        if (canvasRenderer != null) {
            lastColor = canvasRenderer.GetColor();
            PropagateColor(lastColor);
        }
    }

    private void Update() {
        if (canvasRenderer == null) return;

        Color currentColor = canvasRenderer.GetColor();
        if (currentColor != lastColor) {
            lastColor = currentColor;
            PropagateColor(currentColor);
        }
    }

    private void PropagateColor(Color newColor) {
        if (targetGraphics == null) return;
        foreach (var target in targetGraphics) {
            if (target != null) {
                target.color = newColor;
            }
        }
    }

    // Overriding this ensures the proxy itself doesn't generate any vertices or draw pixels
    protected override void OnPopulateMesh(VertexHelper vh) {
        vh.Clear();
    }
}
