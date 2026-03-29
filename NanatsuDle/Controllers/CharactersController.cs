using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NanatsuDle.Data;
using NanatsuDle.Models;

namespace NanatsuDle.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CharactersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("random")]
        public async Task<IActionResult> GetRandom()
        {
            var count = await _context.Characters.CountAsync();
            if (count == 0)
                return NotFound("No hay personajes en la base de datos.");

            var randomIndex = new Random().Next(0, count);

            var character = await _context.Characters
                .Skip(randomIndex)
                .Select(c => new
                {
                    c.Id,
                    c.ImageUrl,
                    c.Gender,
                    c.Race,
                    c.Arc,
                    c.ArcOrder,
                    c.HairColor,
                    c.Affiliation,
                    c.Height,
                    c.TypeOfSkill
                })
                .FirstOrDefaultAsync();

            return Ok(character);
        }

        [HttpGet("hint/{id}")]
        public async Task<IActionResult> GetHint(int id, [FromQuery] int attempts)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
                return NotFound("Personaje no encontrado.");

            var hint = new HintResult();

            if (attempts >= 5)
                hint.Magic = character.Magic;

            if (attempts >= 7)
                hint.FirstAppearance = character.FirstAppearance;

            return Ok(hint);
        }

        [HttpPost("guess")]
        public async Task<IActionResult> Guess([FromBody] GuessRequest request)
        {
            var target = await _context.Characters.FindAsync(request.TargetId);
            if (target == null)
                return NotFound("Personaje objetivo no encontrado.");

            var guess = await _context.Characters.FindAsync(request.GuessId);
            if (guess == null)
                return NotFound("Personaje del intento no encontrado.");

            var result = new GuessResult
            {
                GuessId = guess.Id,
                GuessName = guess.Name,
                GuessImageUrl = guess.ImageUrl,
                IsCorrect = guess.Id == target.Id,

                Gender = CompareExact(guess.Gender, target.Gender),
                Race = CompareExact(guess.Race, target.Race),
                HairColor = CompareExact(guess.HairColor, target.HairColor),

                Affiliation = CompareMulti(guess.Affiliation, target.Affiliation),
                TypeOfSkill = CompareMulti(guess.TypeOfSkill, target.TypeOfSkill),

                Height = CompareNumeric(guess.Height, target.Height),
                Arc = CompareNumeric(guess.ArcOrder, target.ArcOrder,
                                               guess.Arc, target.Arc)
            };

            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Ok(new List<object>());

            var results = await _context.Characters
                .Where(c => c.Name.Contains(name))
                .Select(c => new { c.Id, c.Name, c.ImageUrl })
                .ToListAsync();

            return Ok(results);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _context.Characters.ToListAsync());
        }

        // ─── Métodos de comparación ──────────────────────────────────────

        private static FieldResult CompareExact(string guessVal, string targetVal)
        {
            bool correct = string.Equals(guessVal.Trim(), targetVal.Trim(),
                                         StringComparison.OrdinalIgnoreCase);
            return new FieldResult
            {
                Value = guessVal,
                Status = correct ? "correct" : "incorrect"
            };
        }

        private static FieldResult CompareMulti(string guessVal, string targetVal)
        {
            var guessSet = guessVal.Split(',')
                                    .Select(x => x.Trim().ToLower())
                                    .ToHashSet();
            var targetSet = targetVal.Split(',')
                                     .Select(x => x.Trim().ToLower())
                                     .ToHashSet();

            bool allMatch = guessSet.SetEquals(targetSet);
            bool anyMatch = guessSet.Intersect(targetSet).Any();

            return new FieldResult
            {
                Value = guessVal,
                Status = allMatch ? "correct" : anyMatch ? "partial" : "incorrect"
            };
        }

        private static FieldResult CompareNumeric(int guessVal, int targetVal,
                                                   string? guessLabel = null,
                                                   string? targetLabel = null)
        {
            string status;
            if (guessVal == targetVal) status = "correct";
            else if (guessVal < targetVal) status = "higher";
            else status = "lower";

            return new FieldResult
            {
                Value = guessLabel ?? guessVal.ToString(),
                Status = status
            };
        }
    }

    // ─── DTOs ────────────────────────────────────────────────────────────

    public class GuessRequest
    {
        public int TargetId { get; set; }
        public int GuessId { get; set; }
    }

    public class FieldResult
    {
        public string Value { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public class GuessResult
    {
        public int GuessId { get; set; }
        public string GuessName { get; set; } = string.Empty;
        public string GuessImageUrl { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }

        public FieldResult Gender { get; set; } = new();
        public FieldResult Race { get; set; } = new();
        public FieldResult HairColor { get; set; } = new();
        public FieldResult Affiliation { get; set; } = new();
        public FieldResult TypeOfSkill { get; set; } = new();
        public FieldResult Height { get; set; } = new();
        public FieldResult Arc { get; set; } = new();
    }

    public class HintResult
    {
        public string? Magic { get; set; } = null;
        public string? FirstAppearance { get; set; } = null;
    }
}