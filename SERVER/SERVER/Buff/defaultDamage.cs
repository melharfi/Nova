using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVER.Buff
{
    public static class defaultDamage
    {
        public static void Apply(object[] parameters)
        {
            Actor spellCaster = parameters[0] as Actor;
            List<Effects.ZoneEffect.ZoneEffectTemplate> affectedPlayers = parameters[1] as List<Effects.ZoneEffect.ZoneEffectTemplate>;
            int spellID = (int)parameters[2];
            Actor.effects effect = parameters[3] as Actor.effects;
            bool cd = Convert.ToBoolean(parameters[4]);
            Point spellPos = parameters[5] as Point;

            Actor.SpellsInformations infos_sorts = spellCaster.sorts.Find(f => f.SpellId == spellID);
            mysql.spells spell_Template = (DataBase.DataTables.spells as List<mysql.spells>).Find(f => f.spellID == spellID && f.level == infos_sorts.Level);

            Battle _battle = Battle.Battles.Find(f => f.IdBattle == spellCaster.idBattle);

            #region inscription dans les envoutements
            //// ajout du sort dans la liste des envoutements systeme "non debufable"
            if (spellCaster.BuffsList.Exists(f => f.SortID == spellID && f.system))
            {
                // sort trouvé
                Actor.Buff piEnv = spellCaster.BuffsList.Find(f => f.SortID == spellID && f.system);
                foreach (Effects.ZoneEffect.ZoneEffectTemplate affectedPlayerTemplate in affectedPlayers)
                    if (affectedPlayerTemplate.AffectedActor != null)
                        piEnv.playerRoxed.Add(affectedPlayerTemplate.AffectedActor.Pseudo);
                    else
                        piEnv.playerRoxed.Add("null");
            }
            else
            {
                // ajout du sort dans les envoutements
                Actor.Buff piEnv1 = new Actor.Buff();
                piEnv1.SortID = spellID;
                piEnv1.title = spell_Template.spellName;
                piEnv1.Debuffable = false;
                piEnv1.VisibleToPlayers = false;
                if (affectedPlayers.Count > 0)
                    foreach (Effects.ZoneEffect.ZoneEffectTemplate affectedPlayerTemplate in affectedPlayers)
                        if (affectedPlayerTemplate.AffectedActor != null)
                            piEnv1.playerRoxed.Add(affectedPlayerTemplate.AffectedActor.Pseudo);
                        else
                            piEnv1.playerRoxed.Add("null");
                else
                    piEnv1.playerRoxed.Add("null");
                piEnv1.relanceInterval = spell_Template.relanceInterval;
                piEnv1.BuffState = Enums.Buff.State.Fin;
                piEnv1.relanceParTour = spell_Template.relanceParTour;
                piEnv1.system = true;
                piEnv1.Cd = cd;
                piEnv1.Player = spellCaster.Pseudo;
                spellCaster.BuffsList.Add(piEnv1);
            }
            #endregion
        }
    }
}
