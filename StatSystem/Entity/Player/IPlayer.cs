using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface IPlayer
    {
        public int NetId { get; set; }
        public string UserName { get; set; }
    }
}
