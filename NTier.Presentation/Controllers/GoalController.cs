using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using NTier.Business.Interfaces;
using NTier.DataObject.DTOs.Goal;

namespace NTier.Presentation.Controllers
{
    [EnableCors]
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GoalController : BaseController
    {
        private readonly IGoalService _goalService;

        public GoalController(IGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGoals()
        {
            var goals = await _goalService.GetAllGoalsAsync();
            return Ok(goals);
        }

        [HttpGet("today")]
        public async Task<IActionResult> GetGoalsByStartDateToday()
        {
            var goals = await _goalService.GetGoalsByStartDateTodayAsync();
            return Ok(goals);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetGoalById(Guid id)
        {
            var goal = await _goalService.GetGoalByIdAsync(id);
            if (goal == null)
                return NotFound();

            return Ok(goal);
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal([FromBody] CreateGoalDTO goalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdGoal = await _goalService.CreateGoalAsync(goalDto);
            return CreatedAtAction(nameof(GetGoalById), new { id = createdGoal.Id }, createdGoal);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateGoal(Guid id, [FromBody] UpdateGoalDTO goalDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedGoal = await _goalService.UpdateGoalAsync(id, goalDto);
            if (updatedGoal == null)
                return NotFound();

            return Ok(updatedGoal);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteGoal(Guid id)
        {
            var success = await _goalService.DeleteGoalAsync(id);
            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}
