using MVCMiniProject.Models;

namespace MVCMiniProject.Services.Interfaces
{
    public interface IPositionService
    {
        Task<IEnumerable<Position>> GetAllAsync();
        Task<Position> GetByIdAsync(int id);
    }
}
