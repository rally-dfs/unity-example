#nullable enable

using System;

using UnityEngine;
using UnityEngine.UIElements;

public class WalletCreationScreen : VisualElement
{
    readonly Action<string?> setWalletAddress;

    public WalletCreationScreen(Action<string?> setWalletAddress)
    {
        this.setWalletAddress = setWalletAddress;

        Build();
    }

    void Build()
    {
        style.paddingLeft = 10;
        style.paddingRight = 10;

        var welcomeLabel = new Label("Welcome!")
        {
            style =
            {
                fontSize = 20,
                unityTextAlign = TextAnchor.MiddleCenter
            }
        };
        Add(welcomeLabel);

        var descriptionLabel = new Label("To get started let's create an EOA")
        {
            style =
            {
                fontSize = 16,
                unityTextAlign = TextAnchor.MiddleCenter,
                whiteSpace = WhiteSpace.Normal
            }
        };
        Add(descriptionLabel);

        var generateWalletButton = new Button(CreateWalletAsync)
        {
            text = "Generate a wallet",
            style =
            {
                marginTop = 16,
                unityTextAlign = TextAnchor.MiddleCenter,
            }
        };
        generateWalletButton.styleSheets.Add(Demo.StyleSheet);
        generateWalletButton.AddToClassList("demoButton");
        Add(generateWalletButton);

        var infoLabel = new Label("This will generate an EOA, encrypt it, store it locally and back up to cloud")
        {
            style =
            {
                fontSize = 12,
                unityTextAlign = TextAnchor.MiddleCenter,
                unityFontStyleAndWeight = FontStyle.Italic,
                whiteSpace = WhiteSpace.Normal
            }
        };
        Add(infoLabel);
    }

    async void CreateWalletAsync()
    {
        var newWallet = await AccountManager.GetInstance().CreateAccount();

        setWalletAddress?.Invoke(newWallet.Address);
    }
}