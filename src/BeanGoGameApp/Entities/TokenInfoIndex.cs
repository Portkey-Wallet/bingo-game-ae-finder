using AeFinder.Sdk.Entities;
using Nest;

namespace BeanGoGameApp.Entities;

public class TokenInfoIndex: AeFinderEntity, IAeFinderEntity
{
    [Keyword] public override string Id { get; set; }
    [Keyword] public string Symbol { get; set; }
    [Keyword] public string TokenContractAddress { get; set; }
    public int Decimals { get; set; }
    public long TotalSupply { get; set; }
    [Keyword] public string TokenName { get; set; }
    [Keyword] public string Issuer { get; set; }
    public bool IsBurnable { get; set; }
    public int IssueChainId { get; set; }
}