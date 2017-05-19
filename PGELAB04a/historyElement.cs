using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PGELAB04a
{
    public class historyElement
    {
        public int numRound,
                    userPoints = 0,
                    cpuPoints = 0,
                    round = 0;

        public historyElement(int userPoints, int cpuPoints, int numRound)
        {
            this.userPoints = userPoints;
            this.cpuPoints = cpuPoints;
            this.numRound = numRound;
        }
    }
}
