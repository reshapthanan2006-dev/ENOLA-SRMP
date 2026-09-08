using SRMP.DTOs;
using SRMP.Interfaces.Services;
using SRMP.Models;
using Microsoft.AspNetCore.Mvc;


namespace SRMP.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchingController : ControllerBase
    {
        private readonly IMatchingService _matchingService;

        public MatchingController(IMatchingService matchingService)
        {
            _matchingService = matchingService;
        }

        [HttpPost("calculate")]
        public ActionResult<MatchResult> CalculateMatch(
        MatchRequestDto request)
        {
            var result = _matchingService.CalculateMatch(
                request.Profile,
                request.Vacancy);

            return Ok(result);
        }

        [HttpPost("rank")]
        public ActionResult<List<MatchResult>> RankCandidates(
        RankRequestDto request)
        {
            var results = _matchingService.RankCandidates(
                request.Candidates,
                request.Vacancy);

            return Ok(results);
        }
    }
}