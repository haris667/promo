using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface IAbility
    {
        int ID { get; }
        string Name { get; }    
        string Description { get; }
    }
}
