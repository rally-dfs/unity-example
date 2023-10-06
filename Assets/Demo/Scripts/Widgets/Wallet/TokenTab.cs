#nullable enable

using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UIElements;

public class TokenTab : StatefulVisualElement
{
    readonly string walletAddress;

    double? balance;
    bool loading;
    bool claimingRly;

    public TokenTab(string walletAddress)
    {
        this.walletAddress = walletAddress;

        InitState(() => _ = GetBalance());
    }

    protected override void Build()
    {
        if (loading)
        {
            var loadingIndicator = new ProgressIndicator();
            Add(loadingIndicator);
            return;
        }

        Add(balance == 0 ? ClaimRlyWidget() : AlreadyClaimedUserWidget());
    }

    async Task GetBalance()
    {
        SetState(() => loading = true);

        var balance = (double)await Demo.RlyNetwork.GetBalance(humanReadable: true);

        SetState(() =>
        {
            this.balance = balance;
            loading = false;
        });
    }

    VisualElement ClaimRlyWidget()
    {
        var claimContainer = new VisualElement();
        claimContainer.style.flexDirection = FlexDirection.Column;
        claimContainer.style.alignItems = Align.Center;
        claimContainer.style.justifyContent = Justify.Center;
        claimContainer.style.marginTop = 12;

        if (claimingRly)
        {
            var claimingLabel = new Label("Claiming RLY...")
            {
                style = { fontSize = 18 }
            };
            claimContainer.Add(claimingLabel);

            var loadingIndicator = new ProgressIndicator();
            claimContainer.Add(loadingIndicator);

            return claimContainer;
        }

        var welcomeLabel = new Label("Welcome New User to Rally Protocol!")
        {
            style =
            {
                fontSize = 20,
                unityTextAlign = TextAnchor.MiddleCenter,
                whiteSpace = WhiteSpace.Normal
            }
        };
        claimContainer.Add(welcomeLabel);

        var claimButton = new Button(ClaimRly)
        {
            text = "Claim ERC20",
            style =
            {
                marginTop = 12
            }
        };
        claimButton.styleSheets.Add(Demo.StyleSheet);
        claimButton.AddToClassList("demoButton");
        claimButton.SetEnabled(!loading);
        claimContainer.Add(claimButton);

        var infoLabel = new Label("This will claim 10 units of an ERC20 contract without the user needing native tokens in their wallet")
        {
            style =
            {
                unityTextAlign = TextAnchor.MiddleCenter,
                unityFontStyleAndWeight = FontStyle.Italic,
                fontSize = 12,
                marginTop = 12,
                whiteSpace = WhiteSpace.Normal
            }
        };
        claimContainer.Add(infoLabel);

        return claimContainer;
    }

    async void ClaimRly()
    {
        SetState(() => claimingRly = true);

        await Demo.RlyNetwork.ClaimRly();
        await GetBalance();

        SetState(() => claimingRly = false);
    }

    VisualElement AlreadyClaimedUserWidget()
    {
        var claimedContainer = new VisualElement();
        claimedContainer.style.flexDirection = FlexDirection.Column;
        claimedContainer.style.alignItems = Align.Center;
        claimedContainer.style.justifyContent = Justify.Center;
        claimedContainer.style.flexGrow = 1;

        var congratsLabel = new Label("Congrats you've claimed your gasless erc20 tokens")
        {
            style =
        {
            fontSize = 18,
            unityTextAlign = TextAnchor.MiddleCenter,
            unityFontStyleAndWeight = FontStyle.Bold,
            marginBottom = 12,
            whiteSpace = WhiteSpace.Normal
        }
        };
        claimedContainer.Add(congratsLabel);

        var balanceLabel = new Label("$RLY balance")
        {
            style =
        {
            fontSize = 18,
            unityTextAlign = TextAnchor.MiddleCenter,
            marginTop = 4,
        }
        };
        claimedContainer.Add(balanceLabel);

        var balanceDisplay = new Label(balance.HasValue ? balance.Value.ToString("N2") : "Loading...")
        {
            style =
        {
            fontSize = 24,
            unityTextAlign = TextAnchor.MiddleCenter,
            marginTop = 4,
        }
        };
        claimedContainer.Add(balanceDisplay);

        var viewOnChainButton = new Button(() => ShowBalanceInExplorer())
        {
            text = "view on chain",
            style =
        {
            marginTop = 12,
            fontSize = 14,
        }
        };
        viewOnChainButton.styleSheets.Add(Demo.StyleSheet);
        viewOnChainButton.AddToClassList("demoButton");
        claimedContainer.Add(viewOnChainButton);

        return claimedContainer;
    }

    void ShowBalanceInExplorer()
    {
        var url = $"{Constants.ExplorerUrl}/address/{walletAddress}#tokentxns";
        Application.OpenURL(url);
    }
}
