using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NanatsuDle.Data;

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

        // GET api/characters/daily
        [HttpGet("daily")]
        public async Task<IActionResult> GetDaily()
        {
            var count = await _context.Characters.CountAsync();
            if (count == 0)
                return NotFound("No hay personajes en la base de datos.");

            var colombiaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SA Pacific Standard Time");

            var seed = colombiaTime.Year * 1000 + colombiaTime.DayOfYear;

            var random = new Random(seed);
            var randomIndex = random.Next(0, count);

            var character = await _context.Characters
                .OrderBy(c => c.Id)
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
            var random = new Random(seed);
            var randomIndex = random.Next(0, count);

            var character = await _context.Characters
                .OrderBy(c => c.Id)
                .Skip(randomIndex)
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
            if (attempts >= 5) hint.FirstAppearance = character.FirstAppearance;
            if (attempts >= 7) hint.Magic = character.Magic;
            

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
            var random = new Random(seed);
            var randomIndex = random.Next(0, count);

            var character = await _context.Characters
                .OrderBy(c => c.Id)
                .Skip(randomIndex)
                .FirstOrDefaultAsync();

            if (character == null)
                return NotFound();

            var hint = new HintResult();
            if (attempts >= 5) hint.FirstAppearance = character.FirstAppearance;
            if (attempts >= 7) hint.Magic = character.Magic;

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
            var random = new Random(seed);
            var randomIndex = random.Next(0, count);

            var character = await _context.Characters
                .OrderBy(c => c.Id)
                .Skip(randomIndex)
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

        // GET api/characters/daily3
        [HttpGet("daily3")]
        public async Task<IActionResult> GetDaily3()
        {
            var count = await _context.Characters.CountAsync();
            if (count == 0)
                return NotFound("No hay personajes en la base de datos.");

            var colombiaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SA Pacific Standard Time");

            var seed = colombiaTime.Year * 1000 + colombiaTime.DayOfYear + 1000;
            var random = new Random(seed);
            var randomIndex = random.Next(0, count);

            var character = await _context.Characters
                .OrderBy(c => c.Id)
                .Skip(randomIndex)
                .Include(c => c.Gender)
                .Include(c => c.Race)
                .Include(c => c.HairColor)
                .Include(c => c.Affiliation)
                .Select(c => new
                {
                    c.Id,
                    c.ImageUrl,
                    c.Height,
                    Race = c.Race.Name,
                    HairColor = c.HairColor.Name,
                    Gender = c.Gender.Name,
                    Affiliation = c.Affiliation.Name,
                })
                .FirstOrDefaultAsync();

            return Ok(character);
        }

        [HttpPost("guess3")]
        public async Task<IActionResult> Guess3([FromBody] Guess3Request request)
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
                correctName = target.Name,
                correctImageUrl = target.ImageUrl,
                imageUrl = guess?.ImageUrl ?? null
            });
        }

        [HttpGet("row-game3")]
        public async Task<IActionResult> GetRowGame3([FromQuery] int attempts)
        {
            var count = await _context.Characters.CountAsync();
            if (count == 0)
                return NotFound();

            var colombiaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SA Pacific Standard Time");

            var seed = colombiaTime.Year * 1000 + colombiaTime.DayOfYear + 1000;
            var random = new Random(seed);
            var randomIndex = random.Next(0, count);

            var character = await _context.Characters
                .OrderBy(c => c.Id)
                .Skip(randomIndex)
                .Include(c => c.Gender)
                .Include(c => c.Race)
                .Include(c => c.HairColor)
                .Include(c => c.Affiliation)
                .FirstOrDefaultAsync();

            if (character == null)
                return NotFound();

            var hint = new RowResult();
            if (attempts >= 0) hint.HairColor = character.HairColor.Name;
            if (attempts >= 1) hint.Height = character.Height;
            if (attempts >= 2) hint.Race = character.Race.Name;
            if (attempts >= 3) hint.Gender = character.Gender.Name;
            if (attempts >= 4) hint.Affiliation = character.Affiliation.Name;

            return Ok(hint);
        }

        [HttpGet("daily-category")]
        public async Task<(string nombre, string tipo)> GetCategoriaDiariaAsync()
        {
            var colombiaTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(
                DateTime.UtcNow, "SA Pacific Standard Time");

            var seed = colombiaTime.Year * 1000 + colombiaTime.DayOfYear + 1500;

            string[] categorias = { "Affiliation", "Gender", "HairColor", "Race"};
            var tipo = categorias[seed % categorias.Length];

            string nombre = tipo switch
            {
                "Affiliation" => (await _context.Affiliations
                    .OrderBy(a => a.Id)
                    .Skip(seed % await _context.Affiliations.CountAsync())
                    .Select(a => a.Name)
                    .FirstOrDefaultAsync())!,

                "Gender" => (await _context.Genders
                    .OrderBy(g => g.Id)
                    .Skip(seed % await _context.Genders.CountAsync())
                    .Select(g => g.Name)
                    .FirstOrDefaultAsync())!,

                "HairColor" => (await _context.HairColors
                    .OrderBy(h => h.Id)
                    .Skip(seed % await _context.HairColors.CountAsync())
                    .Select(h => h.Name)
                    .FirstOrDefaultAsync())!,

                "Race" => (await _context.Races
                    .OrderBy(r => r.Id)
                    .Skip(seed % await _context.Races.CountAsync())
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync())!,
            };

            return (nombre, tipo);
        }

        [HttpGet("daily4")]
        public async Task<IActionResult> GetDaily4()
        {
            return Ok(new { ready = true });
        }

        [HttpPost("guess-category")]
        public async Task<IActionResult> GuessCategory([FromBody] Guess4Request request)
        {
            var (nombre, tipo) = await GetCategoriaDiariaAsync();

            var isCorrect = string.Equals(nombre.Trim(), request.AnswerValue.Trim(),
                                          StringComparison.OrdinalIgnoreCase)
                         && string.Equals(tipo.Trim(), request.AnswerTipo.Trim(),
                                          StringComparison.OrdinalIgnoreCase);

            return Ok(new
            {
                isCorrect,
                correctCategory = nombre,
                correctTipo = tipo,
            });
        }

        [HttpPost("guess4")]
        public async Task<IActionResult> Guess4([FromBody] Guess4CharacterRequest request)
        {
            var (nombre, tipo) = await GetCategoriaDiariaAsync();

            var guess = await _context.Characters
                .Include(c => c.Gender)
                .Include(c => c.Race)
                .Include(c => c.HairColor)
                .Include(c => c.Affiliation)
                .FirstOrDefaultAsync(c => c.Id == request.GuessId);

            if (guess == null)
                return NotFound("Personaje no encontrado.");

            FieldResult comparacion = tipo switch
            {
                "Gender" => CompareExact(guess.Gender.Name, nombre),
                "Race" => CompareExact(guess.Race.Name, nombre),
                "HairColor" => CompareExact(guess.HairColor.Name, nombre),
                "Affiliation" => CompareMulti(guess.Affiliation.Name, nombre),
                _ => new FieldResult { Value = "", Status = "incorrect" }
            };

            return Ok(new
            {
                guessId = guess.Id,
                guessName = guess.Name,
                guessImageUrl = guess.ImageUrl,
                resultado = comparacion,
                tipo
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
                .OrderBy(c => c.Name)
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

    public class Guess3Request
    {
        public int TargetId { get; set; }
        public string Answer { get; set; } = string.Empty;
    }

    public class Guess4Request
    {
        public string AnswerTipo { get; set; } = string.Empty;
        public string AnswerValue { get; set; } = string.Empty;
    }
    public class Guess4CharacterRequest
    {
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

    public class GuessResultCategory
    {
        public FieldResult Gender { get; set; } = new();
        public FieldResult Race { get; set; } = new();
        public FieldResult HairColor { get; set; } = new();
        public FieldResult Affiliation { get; set; } = new();
        public FieldResult TypeOfSkill { get; set; } = new();
        public FieldResult Arc { get; set; } = new();
    }

    public class HintResult
    {
        public string? Magic { get; set; } = null;
        public string? FirstAppearance { get; set; } = null;
    }

    public class RowResult
    {
        public int? Height { get; set; } = null;
        public string? Race { get; set; } = null;
        public string? HairColor { get; set; } = null;
        public string? Gender { get; set; } = null;
        public string? Affiliation { get; set; } = null;
    }
}