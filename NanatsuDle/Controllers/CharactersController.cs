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
                    Gender = c.Gender.Name,
                    Race = c.Race.Name,
                    Arc = c.Arc.Name,
                    HairColor = c.HairColor.Name,
                    Affiliation = c.Affiliation.Name,
                    c.Height,
                    TypeOfSkill = c.TypeOfSkill.Name
                })
                .FirstOrDefaultAsync();

            return Ok(character);
        }
        // GET api/characters/daily
        [HttpGet("daily")]
        public async Task<IActionResult> GetDaily()
        {
            var count = await _context.Characters.CountAsync();
            if (count == 0)
                return NotFound("No hay personajes en la base de datos.");

            // Hora Colombia = UTC-5
            var colombiaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SA Pacific Standard Time");

            // Usamos el día del año como semilla para que sea consistente
            var seed = colombiaTime.Year * 1000 + colombiaTime.DayOfYear;
            var index = seed % count;

            var character = await _context.Characters
                .OrderBy(c => c.Id)
                .Skip(index)
                .Include(c => c.Gender)
                .Include(c => c.Race)
                .Include(c => c.Arc)
                .Include(c => c.HairColor)
                .Include(c => c.Affiliation)
                .Include(c => c.TypeOfSkill)
                .Select(c => new
                {
                    c.Id,
                    c.ImageUrl,
                    Gender = c.Gender.Name,
                    Race = c.Race.Name,
                    Arc = c.Arc.Name,
                    HairColor = c.HairColor.Name,
                    Affiliation = c.Affiliation.Name,
                    c.Height,
                    TypeOfSkill = c.TypeOfSkill.Name
                })
                .FirstOrDefaultAsync();

            return Ok(character);
        }

        [HttpGet("daily-date")]
        public IActionResult GetDailyDate()
        {
            var colombiaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SA Pacific Standard Time");

            return Ok(new
            {
                date = colombiaTime.ToString("yyyy-MM-dd")
            });
        }

        // GET api/characters/daily-hint?attempts=5
        [HttpGet("daily-hint")]
        public async Task<IActionResult> GetDailyHint([FromQuery] int attempts)
        {
            var count = await _context.Characters.CountAsync();
            if (count == 0)
                return NotFound();

            var colombiaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SA Pacific Standard Time");

            var seed = colombiaTime.Year * 1000 + colombiaTime.DayOfYear;
            var index = seed % count;

            var character = await _context.Characters
                .OrderBy(c => c.Id)
                .Skip(index)
                .Include(c => c.Gender)
                .Include(c => c.Race)
                .Include(c => c.Arc)
                .Include(c => c.HairColor)
                .Include(c => c.Affiliation)
                .Include(c => c.TypeOfSkill)
                .FirstOrDefaultAsync();

            if (character == null)
                return NotFound();

            var hint = new HintResult();
            if (attempts >= 5) hint.Magic = character.Magic;
            if (attempts >= 7) hint.FirstAppearance = character.FirstAppearance;

            return Ok(hint);
        }

        [HttpGet("daily2-hint")]
        public async Task<IActionResult> GetDaily2Hint([FromQuery] int attempts)
        {
            var count = await _context.Characters.CountAsync();
            if (count == 0)
                return NotFound();

            var colombiaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SA Pacific Standard Time");

            var seed = colombiaTime.Year * 1000 + colombiaTime.DayOfYear + 500;
            var index = seed % count;

            var character = await _context.Characters
                .OrderBy(c => c.Id)
                .Skip(index)
                .FirstOrDefaultAsync();

            if (character == null)
                return NotFound();

            var hint = new HintResult();
            if (attempts >= 5) hint.Magic = character.Magic;
            if (attempts >= 7) hint.FirstAppearance = character.FirstAppearance;

            return Ok(hint);
        }

        [HttpPost("guess")]
        public async Task<IActionResult> Guess([FromBody] GuessRequest request)
        {
            var target = await _context.Characters
                .Include(c => c.Gender)
                .Include(c => c.Race)
                .Include(c => c.Arc)
                .Include(c => c.HairColor)
                .Include(c => c.Affiliation)
                .Include(c => c.TypeOfSkill)
                .FirstOrDefaultAsync(c => c.Id == request.TargetId);

            var guess = await _context.Characters
                .Include(c => c.Gender)
                .Include(c => c.Race)
                .Include(c => c.Arc)
                .Include(c => c.HairColor)
                .Include(c => c.Affiliation)
                .Include(c => c.TypeOfSkill)
                .FirstOrDefaultAsync(c => c.Id == request.GuessId);

            var result = new GuessResult
            {
                GuessId = guess.Id,
                GuessName = guess.Name,
                GuessImageUrl = guess.ImageUrl,
                IsCorrect = guess.Id == target.Id,

                Gender = CompareExact(guess.Gender.Name, target.Gender.Name),
                Race = CompareExact(guess.Race.Name, target.Race.Name),
                HairColor = CompareExact(guess.HairColor.Name, target.HairColor.Name),

                Affiliation = CompareMulti(guess.Affiliation.Name, target.Affiliation.Name),
                TypeOfSkill = CompareMulti(guess.TypeOfSkill.Name, target.TypeOfSkill.Name),

                Height = CompareNumeric(guess.Height, target.Height),

                Arc = CompareNumeric(guess.Arc.Id, target.Arc.Id,
                                     guess.Arc.Name, target.Arc.Name)
            };

            return Ok(result);
        }

        // GET api/characters/daily2
        [HttpGet("daily2")]
        public async Task<IActionResult> GetDaily2()
        {
            var count = await _context.Characters.CountAsync();
            if (count == 0)
                return NotFound("No hay personajes en la base de datos.");

            var colombiaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SA Pacific Standard Time");

            var seed = colombiaTime.Year * 1000 + colombiaTime.DayOfYear + 500;
            var index = seed % count;

            var character = await _context.Characters
                .OrderBy(c => c.Id)
                .Skip(index)
                .Select(c => new
                {
                    c.Id,
                    c.Image2Url,
                })
                .FirstOrDefaultAsync();

            return Ok(character);
        }

        [HttpPost("guess2")]
        public async Task<IActionResult> Guess2([FromBody] Guess2Request request)
        {
            var target = await _context.Characters.FindAsync(request.TargetId);
            if (target == null)
                return NotFound("Personaje no encontrado.");

            var guess = await _context.Characters
                .Where(c => string.Equals(c.Name.Trim(), request.Answer.Trim()))
                .FirstOrDefaultAsync();

            var isCorrect = string.Equals(
                target.Name.Trim(),
                request.Answer.Trim(),
                StringComparison.OrdinalIgnoreCase
            );

            return Ok(new
            {
                isCorrect,
                correctName = isCorrect ? target.Name : null,
                imageUrl = guess?.ImageUrl ?? null
            });
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
    public class Guess2Request
    {
        public int TargetId { get; set; }
        public string Answer { get; set; } = string.Empty;
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