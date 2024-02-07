#nullable enable

public partial class NetworkConfigProvider
{
    public static readonly NetworkConfig Mumbai = new(
        new Contracts(
            tokenFaucet: "0xe7C3BD692C77Ec0C0bde523455B9D142c49720fF",
            rlyERC20: "0x1C7312Cb60b40cF586e796FEdD60Cf243286c9E9"
        ),
        new GSNConfig(
            paymasterAddress: "0x8b3a505413Ca3B0A17F077e507aF8E3b3ad4Ce4d",
            forwarderAddress: "0xB2b5841DBeF766d4b521221732F9B618fCf34A87",
            relayHubAddress: "0x3232f21A6E08312654270c78A773f00dd61d60f5",
            relayWorkerAddress: "0xb9950b71ec94cbb274aeb1be98e697678077a17f",
            relayUrl: "https://api.rallyprotocol.com",
            rpcUrl: "https://polygon-mumbai.g.alchemy.com/v2/-dYNjZXvre3GC9kYtwDzzX4N8tcgomU4",
            chainId: "80001",
            maxAcceptanceBudget: "285252",
            domainSeparatorName: "GSN Relayed Transaction",
            gtxDataZero: 4,
            gtxDataNonZero: 16,
            requestValidSeconds: 172800,
            maxPaymasterDataLength: 300,
            maxApprovalDataLength: 300,
            maxRelayNonceGap: 3
        )
    );
}