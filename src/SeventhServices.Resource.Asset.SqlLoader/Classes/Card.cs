using SeventhServices.Resource.Asset.SqlLoader.Abstractions;

namespace SeventhServices.Resource.Asset.SqlLoader.Classes
{
    public class Card : Entity
    {
        public int CardId { get; set; }
		public int CharacterId { get; set; }
		public string PotentialGroup { get; set; }
		public string RarityId { get; set; }
		public string CardTypeId { get; set; }
		public string FacialImageId { get; set; }
		public string CardMessageId { get; set; }
		public int Cost { get; set; }
		public string CardName { get; set; }
		public string Description { get; set; }
		public int MaxLevel { get; set; }
		public int BreakMaxLevel { get; set; }
		public string HpGrowType { get; set; }
		public string AttackGrowType { get; set; }
		public string DefaultHp { get; set; }
		public string DefaultAttack { get; set; }
		public int MaxHp { get; set; }
		public int MaxAttack { get; set; }
		public int BreakMaxHp { get; set; }
		public int BreakMaxAttack { get; set; }
		public string MemberSkillId { get; set; }
		public string LeaderSkillId { get; set; }
		public string BasicExp { get; set; }
		public int Payback7thpt { get; set; }
		public string TypePotential { get; set; }
		public string SkillPotential { get; set; }
		public string ComboIds { get; set; }
		public string LiveLeaderSkillId { get; set; }
		public string LiveMemberSkillId { get; set; }
		public string MaxIntimate { get; set; }
		public string StockFlg { get; set; }
		public string SignFlg { get; set; }
		public string Role { get; set; }
		public string LimitedFlg { get; set; }
		public string StartTime { get; set; }
		public string DeleteFlg { get; set; }

        //public override string ToString()
        //{
        //    return $"Name : {CardName} \n" +
        //           $"Description : {Description} \n" +
        //           $"7thPt : {Payback7thpt} Cost {Cost} \n" +
        //           $"Level(Max) : {MaxLevel} £ºLevel(BMax) : {BreakMaxLevel} \n" +
        //           $"HP(lv.1) : {DefaultHp} ATK(lv.1) £º{DefaultAttack} \n" +
        //           $"HP(Max) : {MaxHp} ATK(Max) £º{MaxAttack} \n" +
        //           $"HP(BMax) : {BreakMaxHp} ATK(BMax) £º{BreakMaxAttack} ";
        //}
    }
}