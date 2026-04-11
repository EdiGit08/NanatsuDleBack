using System.ComponentModel.DataAnnotations.Schema;

namespace NanatsuDle.Models
{
    public class Character
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Image2Url { get; set; } = string.Empty;
        public int Height { get; set; }
        public string Magic { get; set; } = string.Empty;
        public string FirstAppearance { get; set; } = string.Empty;

        public int GenderId { get; set; }
        public int RaceId { get; set; }
        public int ArcId { get; set; }
        public int HairColorId { get; set; }
        public int AffiliationId { get; set; }
        public int TypeOfSkillId { get; set; }


        [ForeignKey("GenderId")]
        public virtual required Gender Gender { get; set; }
        
        [ForeignKey("RaceId")]
        public virtual required Race Race { get; set; }
        
        [ForeignKey("ArcId")]
        public virtual required Arc Arc { get; set; }

        [ForeignKey("HairColorId")]
        public virtual required HairColor HairColor { get; set; }

        [ForeignKey("AffiliationId")]
        public virtual required Affiliation Affiliation { get; set; }
        
        [ForeignKey("TypeOfSkillId")]
        public virtual required TypeOfSkill TypeOfSkill { get; set; }
    }
}