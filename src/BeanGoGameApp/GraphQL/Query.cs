using System.Linq.Expressions;
using AeFinder.Sdk;
using BeanGoGameApp.Entities;
using GraphQL;
using Nest;
using Volo.Abp.ObjectMapping;

namespace BeanGoGameApp.GraphQL;

public class Query
{
    public static async Task<BingoResultDto> BingoGameInfo(
        [FromServices] IReadOnlyRepository<BingoGameIndex> repository,
        [FromServices] IReadOnlyRepository<BingoGameStaticsIndex> statsRepository,
        [FromServices] IObjectMapper objectMapper, GetBingoDto dto)
    {
        var queryable = await repository.GetQueryableAsync();

        if (!dto.PlayId.IsNullOrWhiteSpace())
        {
            queryable = queryable.Where(a => a.PlayId == dto.PlayId);
        }

        queryable = queryable.Where(t => t.IsComplete == true);

        if (!dto.CAAddresses.IsNullOrEmpty())
        {
            queryable = queryable.Where(
                dto.CAAddresses.Select(address =>
                        (Expression<Func<BingoGameIndex, bool>>)(t => t.PlayerAddress == address))
                    .Aggregate((prev, next) => prev.Or(next)));
        }

        var result = queryable.OrderByDescending(t => t.BingoBlockHeight).Skip(dto.SkipCount).Take(dto.MaxResultCount).ToList();
        var dataList = objectMapper.Map<List<BingoGameIndex>, List<BingoInfo>>(result);

        var statsQueryable = await statsRepository.GetQueryableAsync();
        if (!dto.CAAddresses.IsNullOrEmpty())
        {
            statsQueryable = statsQueryable.Where(
                dto.CAAddresses.Select(address =>
                        (Expression<Func<BingoGameStaticsIndex, bool>>)(t => t.PlayerAddress == address))
                    .Aggregate((prev, next) => prev.Or(next)));
        }
        
        var statsDataList = objectMapper.Map<List<BingoGameStaticsIndex>, List<Bingostats>>(statsQueryable.ToList());
        
        return new BingoResultDto
        {
            TotalRecordCount = queryable.Count(),
            Data = dataList,
            Stats = statsDataList,
        };
    }
}