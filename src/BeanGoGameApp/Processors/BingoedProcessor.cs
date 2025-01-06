using AeFinder.Sdk.Processor;
using BeanGoGameApp.Configs;
using BeanGoGameApp.Entities;
using Google.Protobuf.WellKnownTypes;
using Portkey.Contracts.BingoGameContract;

namespace BeanGoGameApp.Processors;

public class BingoedProcessor : BingoGameProcessorBase<Bingoed>
{
    public override string GetContractAddress(string chainId)
    {
        return BeanGoGameConfig.ContractInfo.ContractInfos.First(c => c.ChainId == chainId).BingoGameContractAddress;
    }

    public override async Task ProcessAsync(Bingoed logEvent, LogEventContext context)
    {
        if (logEvent.PlayerAddress == null || logEvent.PlayerAddress.Value == null)
        {
            return;
        }

        //update bingoIndex
        var index = await GetEntityAsync<BingoGameIndex>(logEvent.PlayId.ToHex());
        // we will throw exception if index is null, because we should have played event after bingoed event
        if (index == null)
        {
            return;
        }
        
        index.BingoBlockHeight = context.Block.BlockHeight;
        index.BingoId = context.Transaction.TransactionId;
        index.BingoTime = context.Block.BlockTime.ToTimestamp().Seconds;
        var feeList = GetFeeList(context.Transaction.ExtraProperties);
        index.BingoTransactionFee = feeList;
        index.IsComplete = true;
        index.Dices = logEvent.Dices.Dices.ToList();
        index.Award = logEvent.Award;
        index.BingoBlockHash = context.Block.BlockHash;
        await SaveEntityAsync(index);

        //update bingostatsIndex
        var statsId = IdGenerateHelper.GenerateId(context.ChainId, logEvent.PlayerAddress.ToBase58());
        var bingostatsIndex = await GetEntityAsync<BingoGameStaticsIndex>(statsId);
        if (bingostatsIndex == null)
        {
            bingostatsIndex = new BingoGameStaticsIndex
            {
                Id = statsId,
                PlayerAddress = logEvent.PlayerAddress.ToBase58(),
                Amount = logEvent.Amount,
                Award = logEvent.Award,
                TotalWins = logEvent.Award > 0 ? 1 : 0,
                TotalPlays = 1
            };
        }
        else
        {
            bingostatsIndex.Amount += logEvent.Amount;
            bingostatsIndex.Award += logEvent.Award;
            bingostatsIndex.TotalPlays += 1;
            bingostatsIndex.TotalWins += logEvent.Award > 0 ? 1 : 0;
        }
        
        await SaveEntityAsync(bingostatsIndex);
    }
}