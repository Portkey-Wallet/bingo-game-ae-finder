namespace BeanGoGameApp.Options;

public class ContractInfoOptions
{
    public List<ContractInfo> ContractInfos { get; set; }
}

public class ContractInfo
{
    public string ChainId { get; set; }
    public string BingoGameContractAddress { get; set;}
}