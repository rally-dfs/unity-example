#nullable enable

using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class NftTab : StatefulVisualElement
{
    readonly string walletAddress;

    bool hasLoaded;
    bool hasMinted;
    bool minting;
    string? nftUri;
    string? txnHash;

    public NftTab(string walletAddress)
    {
        this.walletAddress = walletAddress;

        InitState(LoadExistingStateFromStorage);
    }

    override protected void Build()
    {
        if (!hasLoaded)
        {
            var progressIndicator = new ProgressIndicator();
            Add(progressIndicator);
            return;
        }

        Add(hasMinted ? NftWidget() : MintNftWidget());
    }

    void LoadExistingStateFromStorage()
    {
        var storedData = PlayerPrefs.GetString("nft_txn_hash")?.Split(",");
        if (storedData == null || storedData.Length < 2)
        {
            SetState(() => hasLoaded = true);
            return;
        }

        var existingHash = storedData[0];
        var existingUri = storedData[1];

        SetState(() =>
        {
            txnHash = existingHash;
            nftUri = existingUri;
            hasMinted = true;
            hasLoaded = true;
        });
    }

    VisualElement MintNftWidget()
    {
        var mintContainer = new VisualElement();
        mintContainer.style.flexDirection = FlexDirection.Column;
        mintContainer.style.alignItems = Align.Center;
        mintContainer.style.justifyContent = Justify.Center;
        mintContainer.style.marginTop = 12;

        if (minting)
        {
            var claimingLabel = new Label("Minting your NFT...")
            {
                style = { fontSize = 18, whiteSpace = WhiteSpace.Normal }
            };
            mintContainer.Add(claimingLabel);

            var loadingIndicator = new ProgressIndicator();
            mintContainer.Add(loadingIndicator);

            return mintContainer;
        }

        var infoLabel = new Label("You don't have an NFT yet.")
        {
            style =
        {
            fontSize = 22,
            unityTextAlign = TextAnchor.MiddleCenter,
            marginBottom = 12,
            whiteSpace = WhiteSpace.Normal
        }
        };
        mintContainer.Add(infoLabel);

        var mintButton = new Button(MintNFT)
        {
            text = "Mint NFT",
            style =
            {
                marginTop = 12
            }
        };
        mintButton.styleSheets.Add(Demo.StyleSheet);
        mintButton.AddToClassList("demoButton");
        mintContainer.Add(mintButton);

        var detailsLabel = new Label("This will mint an NFT without user needing any native tokens to pay for gas")
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
        mintContainer.Add(detailsLabel);

        return mintContainer;
    }

    async void MintNFT()
    {
        SetState(() => minting = true);

        var provider = await Demo.RlyNetwork.GetClient();

        var nft = new NFT(Constants.NftContractAddress, walletAddress, provider);

        var nextNFTId = await nft.GetCurrentNFTIdAsync();

        var gsnTx = await nft.GetMinftNFTTx();

        var txHash = await Demo.RlyNetwork.Relay(gsnTx);

        var tokenURI = await nft.GetTokenURIAsync(nextNFTId);

        var parts = tokenURI.Split(',');

        var base64Data = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));

        var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(base64Data);
        var imageUri = json!["image"];

        PlayerPrefs.SetString("nft_txn_hash", $"{txHash},{imageUri}");

        SetState(() =>
        {
            hasMinted = true;
            minting = false;
            nftUri = imageUri;
            txnHash = txHash;
        });
    }

    VisualElement NftWidget()
    {
        var nftContainer = new VisualElement();
        nftContainer.style.flexDirection = FlexDirection.Column;
        nftContainer.style.alignItems = Align.Center;
        nftContainer.style.justifyContent = Justify.Center;
        nftContainer.style.flexGrow = 1;

        var congratsLabel = new Label("Gasless NFT Minted!")
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
        nftContainer.Add(congratsLabel);

        if (nftUri != null)
        {
            var nftImage = new Image();
            nftImage.style.height = 200;
            nftImage.style.width = new Length(100, LengthUnit.Percent);
            nftImage.style.marginBottom = 8;
            nftContainer.Add(nftImage);

            var www = UnityWebRequestTexture.GetTexture(nftUri);
            www.SendWebRequest().completed += operation =>
            {
                if (www.result == UnityWebRequest.Result.Success && nftImage != null)
                    nftImage.image = DownloadHandlerTexture.GetContent(www);
            };
        }

        var viewOnChainButton = new Button(() => ShowTransactionOnExplorer())
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
        nftContainer.Add(viewOnChainButton);

        return nftContainer;
    }

    void ShowTransactionOnExplorer()
    {
        var url = $"{Constants.ExplorerUrl}/tx/{txnHash}";
        Application.OpenURL(url);
    }
}
