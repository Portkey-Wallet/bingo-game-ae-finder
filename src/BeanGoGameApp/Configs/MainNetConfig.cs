using BeanGoGameApp.Options;

namespace BeanGoGameApp.Configs;

public static class MainNetConfig
{
    public static ContractInfoOptions ContractInfo = new ContractInfoOptions
    {
        ContractInfos = new List<ContractInfo>
        {
            new ContractInfo
            {
                ChainId = "tDVV",
                BingoGameContractAddress = "fU9csLqXtnSbcyRJs3fPYLFTz2S9EZowUqkYe4zrJgp1avXK2"
            }
        }
    };
}