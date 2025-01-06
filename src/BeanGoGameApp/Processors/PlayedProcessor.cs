using AeFinder.Sdk.Processor;
using BeanGoGameApp.Configs;
using BeanGoGameApp.Entities;
using Google.Protobuf.WellKnownTypes;
using Portkey.Contracts.BingoGameContract;

namespace BeanGoGameApp.Processors;

public class PlayedProcessor: BingoGameProcessorBase<Played>
{
    public override string GetContractAddress(string chainId)
    {
        return BeanGoGameConfig.ContractInfo.ContractInfos.First(c => c.ChainId == chainId).BingoGameContractAddress;
    }

    public override async Task ProcessAsync(Played logEvent, LogEventContext context)
    {
        if (logEvent.PlayerAddress == null || logEvent.PlayerAddress.Value == null)
        {
            return;
        }

        var index = await GetEntityAsync<BingoGameIndex>(logEvent.PlayId.ToHex());
        // we will throw exception if index is not null, because we should not have handled this event before
        if (index != null)
        {
            return;
        }
        var feeList = GetFeeList(context.Transaction.ExtraProperties);
        var bingoIndex = new BingoGameIndex
        {
            Id = logEvent.PlayId.ToHex(),
            PlayBlockHeight = logEvent.PlayBlockHeight,
            Amount = logEvent.Amount,
            IsComplete = false,
            PlayId = context.Transaction.TransactionId,
            BingoType = (int)logEvent.Type,
            Dices = new List<int>(),
            PlayerAddress = logEvent.PlayerAddress.ToBase58(),
            PlayTime = context.Block.BlockTime.ToTimestamp().Seconds,
            PlayTransactionFee = feeList,
            PlayBlockHash = context.Block.BlockHash
        };

        await SaveEntityAsync(bingoIndex);
    }
}