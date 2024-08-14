using Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class TestItemF : Item
    {
        public TestItemF(long globalId, int gameId, string name, string description, Dictionary<StatType, float> statsChange) : base(globalId, gameId, name, description, statsChange)
        {
        }

        public override void Equip()
        {
            throw new NotImplementedException();
        }

        public override void Unequip()
        {
            throw new NotImplementedException();
        }
    }
}
