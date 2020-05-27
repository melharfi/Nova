using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace SERVER.Net.Messages.Response
{
    internal class SyncFeaturesResponseMessage : IResponseMessage
    {
        public object[] CommandStrings { get; set; }
        public NetConnection Nc { get; set; }
        public bool _serialized { get; set; }
        public string _buffer { get; set; }
        private object[] _o;

        public SyncFeaturesResponseMessage(object[] o)
        {
            /*_o = actor.Pseudo + "#" + actor.classeName + "#" + actor.Spirit + "#" + actor.SpiritLvl + "#" + actor.Pvp + "#" + actor.village + "#" + actor.MaskColors + "#" + actor.Orientation + "#" + actor.Level + "#" + actor.map + "#" + actor.officialRang + "#" + actor.currentHealth + "#" + actor.totalHealth + "#" + actor.xp + "#" + totalXp + "#" + actor.doton + "#" + actor.katon + "#" + actor.futon + "#" + actor.raiton + "#" + actor.suiton + "#" + MainClass.chakralvl1 + "#" + MainClass.chakralvl2 + "#" + MainClass.chakralvl3 + "#" + MainClass.chakralvl4 + "#" + MainClass.chakralvl5 + "#" + actor.usingDoton + "#" + actor.usingKaton + "#" + actor.usingFuton + "#" + actor.usingRaiton + "#" + actor.usingSuiton + "#" + actor.equipedDoton + "#" + actor.equipedKaton + "#" + actor.equipedFuton + "#" + actor.equipedRaiton + "#" + actor.equipedSuiton + "#" + actor.original_Pc + 
                    "#" + actor.original_Pm + "#" + actor.pe + "#" + actor.cd + "#" + actor.summon + "#" + actor.Initiative + "#" + 
                    actor.job1 + "#" + actor.job2 + "#" + actor.specialty1 + "#" + actor.specialty2 + "#" + actor.maxWeight + "#" + 
                    actor.CurrentWeight + "#" + actor.Ryo + "#" + actor.resiDotonPercent + "#" + actor.resiKatonPercent + "#" + 
                    actor.resiFutonPercent + "#" + actor.resiRaitonPercent + "#" + actor.resiSuitonPercent + "#" + actor.dodgePC + "#" + 
                    actor.dodgePM + "#" + actor.dodgePE + "#" + actor.dodgeCD + "#" + actor.removePC + "#" + actor.removePM + "#" + 
                    actor.removePE + "#" + actor.removeCD + "#" + actor.escape + "#" + actor.blocage + "#" + spell + "#" + 
                    actor.resiDotonFix + "#" + actor.resiKatonFix + "#" + actor.resiFutonFix + "#" + actor.resiRaitonFix + "#" + 
                    actor.resiSuitonFix + "#" + actor.resiFix + "#" + actor.domDotonFix + "#" + actor.domKatonFix + "#" + actor.domFutonFix
                    + "#" + actor.domRaitonFix + "#" + actor.domSuitonFix + "#" + actor.domFix + "#" + actor.power + "#" + actor.equipedPower
                    + "•" + quests + "•" + actor.spellPointLeft*/
            _o = o;
        }

        public void Initialize(object[] commandStrings, NetConnection nc)
        {
            _serialized = false;
            Nc = nc;
            CommandStrings = commandStrings;
        }

        public void Send()
        {
            // _buffer = 
            if (!_serialized)
                throw new NotImplementedException("buffer not serialized yet, you should call Serialize() function first");
            CommonCode.SendMessage(_buffer, Nc, true);
            Console.WriteLine("(SEND)" + _buffer.Replace(CommandDelimitterChar.Delimitter, '.'));
        }

        public void Serialize()
        {
            _buffer = GetType().Name + CommandDelimitterChar.Delimitter;
            foreach (object o in _o)
                _buffer += o.ToString() + CommandDelimitterChar.Delimitter;
            _buffer = _buffer.Substring(0, _buffer.Length + 1);
            _serialized = true;
        }
    }
}
