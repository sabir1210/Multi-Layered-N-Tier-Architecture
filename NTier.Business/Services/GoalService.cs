using AutoMapper;
using NTier.Business.Interfaces;
using NTier.DataAccess.UnitOfWork;
using NTier.DataObject.DTOs.Goal;
using NTier.DataObject.Entities;

namespace NTier.Business.Services
{
    public class GoalService : BaseService, IGoalService
    {
        public GoalService(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }
        public async Task<IEnumerable<GoalDTO>> GetAllGoalsAsync()
        {

            var goalRepository = _unitOfWork.GetRepository<Goal>();
            var goals = await goalRepository.GetAllAsync();
            var goalDtos = _mapper.Map<IEnumerable<GoalDTO>>(goals);
            return goalDtos;
        }

        public async Task<GoalDTO> GetGoalByIdAsync(Guid id)
        {
            var goalRepository = _unitOfWork.GetRepository<Goal>();
            var goal = await goalRepository.GetByIdAsync(id);
            if (goal == null)
            {
                return null; // Or handle not found scenario
            }
            var goalDto = _mapper.Map<GoalDTO>(goal);
            return goalDto;
        }

        public async Task<GoalDTO> CreateGoalAsync(CreateGoalDTO createGoalDto)
        {
            var goalRepository = _unitOfWork.GetRepository<Goal>();
            var goal = _mapper.Map<Goal>(createGoalDto);
            await goalRepository.AddAsync(goal);
            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync(); // Commit the transaction
            var goalDto = _mapper.Map<GoalDTO>(goal);
            return goalDto;
        }

        public async Task<GoalDTO> UpdateGoalAsync(Guid id, UpdateGoalDTO updateGoalDto)
        {
            var goalRepository = _unitOfWork.GetRepository<Goal>();
            var goal = await goalRepository.GetByIdAsync(id);
            if (goal == null)
            {
                return null; // Or handle not found scenario
            }

            _mapper.Map(updateGoalDto, goal);
            goalRepository.UpdateAsync(goal);
            await _unitOfWork.SaveChangesAsync();
            var goalDto = _mapper.Map<GoalDTO>(goal);
            return goalDto;
        }

        public async Task<bool> DeleteGoalAsync(Guid id)
        {
            var goalRepository = _unitOfWork.GetRepository<Goal>();
            var goal = await goalRepository.GetByIdAsync(id);
            if (goal == null)
            {
                return false; // Or handle not found scenario
            }

            goalRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public Task<GoalDTO> UpdateGoalAsync(int id, UpdateGoalDTO updateGoalDto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteGoalAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
