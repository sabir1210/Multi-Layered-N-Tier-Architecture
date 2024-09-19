using AutoMapper;
using NTier.DataObject.DTOs.Goal;
using NTier.DataObject.Entities;

namespace NTier.Business.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapping between the Goal entity and GoalDTO
            CreateMap<Goal, GoalDTO>().ReverseMap();

            // Mapping for Goal creation DTO
            CreateMap<Goal, CreateGoalDTO>().ReverseMap();

            // Mapping for Goal update DTO
            CreateMap<Goal, UpdateGoalDTO>().ReverseMap();
        }
    }
}
