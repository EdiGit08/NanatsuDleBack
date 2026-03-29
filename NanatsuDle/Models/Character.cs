namespace NanatsuDle.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string Race { get; set; } = string.Empty;
        public string Arc { get; set; } = string.Empty;
        public int ArcOrder { get; set; }
        public string HairColor { get; set; } = string.Empty;
        public string Affiliation { get; set; } = string.Empty;
        public int Height { get; set; }
        public string TypeOfSkill { get; set; } = string.Empty;
        public string Magic { get; set; } = string.Empty;
        public string FirstAppearance { get; set; } = string.Empty;
    }
}