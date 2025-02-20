using DuckBank.Core.Dtos;

namespace DuckBank.Core.Interfaces.Bl
{
    public interface IAHorroBl
    {
        Task<int> AgregarAsync(AhorroDtoIn ahorroDtoIn);
    }
}
