#nullable enable

using System;

using UnityEngine.UIElements;

public class AppContent : VisualElement
{
    readonly string? walletAddress;
    readonly Action<string?> setWalletAddress;

    public AppContent(string? walletAddress, Action<string?> setWalletAddress)
    {
        this.walletAddress = walletAddress;
        this.setWalletAddress = setWalletAddress;

        Build();
    }

    void Build()
    {
        Add(walletAddress == null ? new WalletCreationScreen(setWalletAddress) : new WalletHomeScreen(walletAddress, setWalletAddress));
    }
}