using AeFinder.Sdk.Processor;
using AElf.CSharp.Core;
using BeanGoGameApp.Entities;
using Newtonsoft.Json;
using Volo.Abp.ObjectMapping;

namespace BeanGoGameApp.Processors;

public abstract class BingoGameProcessorBase<TEvent> : LogEventProcessorBase<TEvent>
    where TEvent : IEvent<TEvent>, new()
{
    protected IObjectMapper ObjectMapper => LazyServiceProvider.LazyGetRequiredService<IObjectMapper>();

    protected Dictionary<string, long> GetTransactionFee(Dictionary<string, string> extraProperties)
    {
        var feeMap = new Dictionary<string, long>();
        if (extraProperties.TryGetValue("TransactionFee", out var transactionFee))
        {
            feeMap = JsonConvert.DeserializeObject<Dictionary<string, long>>(transactionFee) ??
                     new Dictionary<string, long>();
        }

        if (extraProperties.TryGetValue("ResourceFee", out var resourceFee))
        {
            var resourceFeeMap = JsonConvert.DeserializeObject<Dictionary<string, long>>(resourceFee) ??
                                 new Dictionary<string, long>();
            foreach (var (symbol, fee) in resourceFeeMap)
            {
                if (feeMap.ContainsKey(symbol))
                {
                    feeMap[symbol] += fee;
                }
                else
                {
                    feeMap[symbol] = fee;
                }
            }
        }

        return feeMap;
    }

    protected List<TransactionFee> GetFeeList(Dictionary<string, string> extraProperties)
    {
        var feeMap = GetTransactionFee(extraProperties);
        List<TransactionFee> feeList;
        if (!feeMap.IsNullOrEmpty())
        {
            feeList = feeMap.Select(pair => new TransactionFee
            {
                Symbol = pair.Key,
                Amount = pair.Value
            }).ToList();
        }
        else
        {
            feeList = new List<TransactionFee>
            {
                new()
                {
                    Symbol = null,
                    Amount = 0
                }
            };
        }

        return feeList;
    }
}