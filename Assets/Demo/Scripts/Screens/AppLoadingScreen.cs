#nullable enable

using UnityEngine.UIElements;

public class AppLoadingScreen : VisualElement
{
    public AppLoadingScreen()
    {
        Build();
    }

    void Build()
    {
        var loadingLabel = new Label("Attempting to load existing wallet...");
        Add(loadingLabel);

        var progressIndicator = new ProgressIndicator();
        Add(progressIndicator);
    }
}