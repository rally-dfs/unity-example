#nullable enable

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

public class Demo : MonoBehaviour
{
    public static readonly INetwork RlyNetwork = NetworkProvider.RlyMumbai;
    public static StyleSheet? StyleSheet { get; private set; }

    [SerializeField] StyleSheet styleSheet = null!;
    [SerializeField] UIDocument uiDocument = null!;
    [SerializeField] bool clearOnStart = false;

    void Awake()
    {
        Assert.IsNotNull(styleSheet);
        Assert.IsNotNull(uiDocument);

        StyleSheet = styleSheet;
    }

    async void Start()
    {
        if (clearOnStart)
        {
            await UnitySdkPlugin.DeleteMnemonic();
            PlayerPrefs.DeleteKey("nft_txn_hash");
        }

        var root = uiDocument.rootVisualElement;


        var appContainer = new AppContainer("EOA Demo");
        root.Add(appContainer);
    }
}