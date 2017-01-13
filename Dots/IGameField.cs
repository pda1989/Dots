using System.Collections.Generic;

namespace Dots
{
    public interface IGameField
    {
        bool FirstMove { get; set; }
        List<List<Dot>> Field { get; set; }

        void InitialyzeField(int size);
        bool MakeMove(int i, int j);
        void CheckChains();
    }
}
