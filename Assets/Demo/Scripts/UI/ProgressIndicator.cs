#nullable enable

using UnityEngine;
using UnityEngine.UIElements;

public class ProgressIndicator : VisualElement
{
    public ProgressIndicator(Vector2? size = null)
    {
        var texture = Resources.Load<Texture2D>("ProgressIndicator");
        if (texture == null)
        {
            Debug.LogError("Failed to load progress indicator image.");
            return;
        }

        style.width = size?.x ?? 64;
        style.height = size?.y ?? 64;
        style.alignSelf = Align.Center;

        var spinnerImage = new Image { image = texture };
        Add(spinnerImage);

        var rotateAnimation = new RotateAnimation(spinnerImage);
        rotateAnimation.Start();
    }

    class RotateAnimation
    {
        readonly VisualElement element;
        readonly float rotationSpeed = 300f;

        float currentRotation = 0f;

        public RotateAnimation(VisualElement element)
        {
            this.element = element;
        }

        public void Start()
        {
            element.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }

        void OnGeometryChanged(GeometryChangedEvent evt)
        {
            currentRotation += rotationSpeed * Time.deltaTime;
            currentRotation %= 360f;
            element.transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);

            element.schedule.Execute(() => OnGeometryChanged(evt)).StartingIn((long)(Time.deltaTime * 1000f));
        }
    }
}
