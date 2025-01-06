using BeanGoGameApp.Options;

namespace BeanGoGameApp.Configs;

public static class TestNetConfig
{
    public static ContractInfoOptions ContractInfo = new ContractInfoOptions
    {
        ContractInfos=new List<ContractInfo>
        {
            new ContractInfo
            {
                ChainId = "tDVW",
                BingoGameContractAddress = "2CrjkQeeWYTnH9zFHmpuMtxv8ZTBDmHi31zzdo9SUNjmpxJ82T"
            }
        }
    };
}