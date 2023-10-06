#nullable enable

using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UIElements;

public class AppContainer : StatefulVisualElement
{
    readonly string title;

    bool appFinishedLoading;
    string? walletAddress;

    public AppContainer(string title)
    {
        this.title = title;

        InitState(() =>
        {
            _ = AttemptToLoadExistingWallet();
            Demo.RlyNetwork.SetApiKey(Constants.RlyApiKey);
        });
    }

    protected override void Build()
    {
        var header = new VisualElement();
        header.style.backgroundColor = new Color(0.3137255f, 0.6431373f, 0.9529412f);
        header.style.height = 75;
        header.style.flexDirection = FlexDirection.Row;
        header.style.justifyContent = Justify.Center;
        header.style.alignItems = Align.Center;

        var titleLabel = new Label(title)
        {
            style =
            {
                unityTextAlign = TextAnchor.MiddleCenter,
                color = Color.white,
                fontSize = 24
            }
        };
        header.Add(titleLabel);
        Add(header);

        var contentArea = new VisualElement();
        contentArea.style.flexGrow = 1;
        Add(contentArea);

        contentArea.Add(appFinishedLoading ? new AppContent(walletAddress, (walletAddress) => SetState(() => this.walletAddress = walletAddress)) : new AppLoadingScreen());
    }

    async Task AttemptToLoadExistingWallet()
    {
        var existingWallet = await AccountManager.GetInstance().GetPublicAddress();

        SetState(() =>
        {
            appFinishedLoading = true;
            walletAddress = existingWallet;
        });
    }
}