using BeanGoGameApp.Entities;
using BeanGoGameApp.GraphQL;
using AutoMapper;

namespace BeanGoGameApp;

public class BeanGoGameAppAutoMapperProfile : Profile
{
    public BeanGoGameAppAutoMapperProfile()
    {
        CreateMap<BingoGameIndex, BingoInfo>();
        CreateMap<BingoGameStaticsIndex, Bingostats>();
    }
}