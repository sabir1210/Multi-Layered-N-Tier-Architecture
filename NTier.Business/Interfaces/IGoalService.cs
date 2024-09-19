using NTier.DataObject.DTOs.Goal;

namespace NTier.Business.Interfaces
{
    //public interface IGoalService
    //{
    //    Task<IEnumerable<GoalDTO>> GetAllGoalsAsync();
    //    Task<GoalDTO> GetGoalByIdAsync(Guid id);
    //    Task<GoalDTO> CreateGoalAsync(GoalDTO goalDto);
    //    Task<GoalDTO> UpdateGoalAsync(Guid id, GoalDTO goalDto);
    //    Task<bool> DeleteGoalAsync(Guid id);
    //}
    public interface IGoalService
    {
        Task<IEnumerable<GoalDTO>> GetAllGoalsAsync();
        Task<GoalDTO> GetGoalByIdAsync(Guid id);
        Task<GoalDTO> CreateGoalAsync(CreateGoalDTO createGoalDto);
        Task<GoalDTO> UpdateGoalAsync(Guid id, UpdateGoalDTO updateGoalDto);
        Task<bool> DeleteGoalAsync(Guid id);
    }
}
