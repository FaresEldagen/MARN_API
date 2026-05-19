using MARN_API.DTOs.Contracts;

namespace MARN_API.Services.Interfaces
{
    public interface IContractPdfGenerator
    {
        GeneratedContractPdfResult Generate(ContractPdfRequest request);
    }
}
