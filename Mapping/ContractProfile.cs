using AutoMapper;
using MARN_API.DTOs.Contracts;
using MARN_API.Models;

namespace MARN_API.Mapping
{
    public class ContractProfile : Profile
    {
        public ContractProfile()
        {
            CreateMap<Contract, ContractResponseDto>();
        }
    }
}
