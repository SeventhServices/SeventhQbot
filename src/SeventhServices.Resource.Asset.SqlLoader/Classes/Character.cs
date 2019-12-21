using SeventhServices.Resource.Asset.SqlLoader.Abstractions;

namespace SeventhServices.Resource.Asset.SqlLoader.Classes
{
    public class Character : Entity
    {
        public int CharacterId { get; set; }
        public string CharacterName { get; set; }
        public string FirstName { get; set; }
        public string CharaNormalImageId { get; set; }
        public string Nickname { get; set; }
        public string SortName { get; set; }
        public string Age { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string Bust { get; set; }
        public string Waist { get; set; }
        public string Hips { get; set; }
        public string Cup { get; set; }
        public string BirthMonth { get; set; }
        public string Birthday { get; set; }
        public string Constellation { get; set; }
        public string BloodType { get; set; }
        public string SpecialAbility { get; set; }
        public string Favorite { get; set; }
        public string Affiliation { get; set; }
        public string Cv { get; set; }
        public string EpisodeSortId { get; set; }
        public string SegmentId { get; set; }
        public string EnglishName { get; set; }
        public string DeleteFlg { get; set; }


    }
}