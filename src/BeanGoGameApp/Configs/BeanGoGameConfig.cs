using BeanGoGameApp.Options;

namespace BeanGoGameApp.Configs;

public static class BeanGoGameConfig
{
    static BeanGoGameConfig()
    {
        SetConfig(NetWork.MainNet); // modify network
    }
    public static ContractInfoOptions ContractInfo { get; set; }

    private static void SetConfig(NetWork netWork)
    {
        ContractInfo = netWork == NetWork.TestNet ? TestNetConfig.ContractInfo : MainNetConfig.ContractInfo;
    }
}

public enum NetWork
{
    MainNet,
    TestNet
}