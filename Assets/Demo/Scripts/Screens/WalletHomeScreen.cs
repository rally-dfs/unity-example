#nullable enable

using System;

using UnityEngine.UIElements;

public class WalletHomeScreen : StatefulVisualElement
{
    enum Tabs
    {
        Tokens,
        NFTs
    }

    readonly string walletAddress;
    readonly Action<string?> setWalletAddress;

    Tabs tab;

    public WalletHomeScreen(string walletAddress, Action<string?> setWalletAddress)
    {
        this.walletAddress = walletAddress;
        this.setWalletAddress = setWalletAddress;

        InitState(() => SwitchTab(Tabs.Tokens));
    }

    protected override void Build()
    {
        var tokensTabButton = new Button(() => SwitchTab(Tabs.Tokens))
        {
            text = "Tokens"
        };
        tokensTabButton.styleSheets.Add(Demo.StyleSheet);
        tokensTabButton.AddToClassList("demoTab");

        var nftsTabButton = new Button(() => SwitchTab(Tabs.NFTs))
        {
            text = "NFTs"
        };
        nftsTabButton.styleSheets.Add(Demo.StyleSheet);
        nftsTabButton.AddToClassList("demoTab");

        var tabBar = new GroupBox();
        tabBar.style.flexDirection = FlexDirection.Row;
        tabBar.style.justifyContent = Justify.Center;

        tabBar.Add(tokensTabButton);
        tabBar.Add(nftsTabButton);
        Add(tabBar);

        var activeTabButton = tab == Tabs.Tokens ? tokensTabButton : nftsTabButton;
        activeTabButton.AddToClassList("demoTabActive");
        activeTabButton.RemoveFromClassList("demoTab");

        var inactiveTabButton = tab == Tabs.Tokens ? nftsTabButton : tokensTabButton;
        inactiveTabButton.RemoveFromClassList("demoTabActive");
        inactiveTabButton.AddToClassList("demoTab");

        VisualElement tokensTabContent = new TokenTab(walletAddress);
        VisualElement nftsTabContent = new NftTab(walletAddress);

        Add(tokensTabContent);
        Add(nftsTabContent);

        var activeTab = tab == Tabs.Tokens ? tokensTabContent : nftsTabContent;
        activeTab.style.display = DisplayStyle.Flex;

        var inactiveTab = tab == Tabs.Tokens ? nftsTabContent : tokensTabContent;
        inactiveTab.style.display = DisplayStyle.None;
    }

    void SwitchTab(Tabs tab) => SetState(() => this.tab = tab);

    async void ClearWallet()
    {
        await AccountManager.GetInstance().PermanentlyDeleteAccount();
        setWalletAddress?.Invoke(null);
    }
}