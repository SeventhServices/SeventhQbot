using Seventh.Resource.Asset.SqlLoader.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Seventh.Resource.Asset.SqlLoader.Serializer
{
    [Obsolete("not complete")]
    public class SqlRegexParser<T>
    {
        [Obsolete]
        public IEnumerable<T> Parse(string sqlString)
        {
            var pType = typeof(Card);
            var pPropertyInfoArr = pType.GetProperties();//
            var SQLReadReg = new Regex(@"('(?<card_id>.+?)','(?<character_id>.+?)','(?<potential_group>.+?)','(?<rarity_id>.+?)','(?<card_type_id>.+?)','(?<facial_image_id>.+?)','(?<card_message_id>.+?)','(?<cost>.+?)','(?<card_name>.+?)','(?<description>.+?)','(?<max_level>.+?)','(?<break_max_level>.+?)','(?<hp_grow_type>.+?)','(?<attack_grow_type>.+?)','(?<default_hp>.+?)','(?<default_attack>.+?)','(?<max_hp>.+?)','(?<max_attack>.+?)','(?<break_max_hp>.+?)','(?<break_max_attack>.+?)','(?<member_skill_id>.+?)','(?<leader_skill_id>.+?)','(?<basic_exp>.+?)','(?<payback_7thpt>.+?)','(?<type_potential>.+?)','(?<skill_potential>.+?)','(?<combo_ids>.+?)','(?<live_leader_skill_id>.+?)','(?<live_member_skill_id>.+?)','(?<max_intimate>.+?)','(?<stock_flg>.+?)','(?<sign_flg>.+?)','(?<role>.+?)','(?<limited_flg>.+?)','(?<start_time>.+?)','(?<delete_flg>.+?)')");//
            var pMatch = SQLReadReg.Match(sqlString);
            if (!SQLReadReg.IsMatch(sqlString)) yield break;

            while (pMatch.Success)
            {
                var t = Activator.CreateInstance<T>();
                foreach (var pPropertyInfo in pPropertyInfoArr)
                {
                    if (SQLReadReg.GroupNumberFromName(pPropertyInfo.Name) >= 0)
                    {
                        if (pPropertyInfo.PropertyType == typeof(bool))
                        {
                            bool trgValue = pMatch.Groups[pPropertyInfo.Name].Value.ToLower() == "1" 
                                            || pMatch.Groups[pPropertyInfo.Name].Value.ToLower() == "true";
                            pPropertyInfo.SetValue(t, trgValue);
                        }
                        else
                        {
                            object pObjVal = Convert.ChangeType(pMatch.Groups[pPropertyInfo.Name].Value, pPropertyInfo.PropertyType);//
                            pPropertyInfo.SetValue(t, pObjVal);
                        }
                    }
                }

                yield return t;
                pMatch = pMatch.NextMatch();
            }
        }
    }
}