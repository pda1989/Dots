using System.Collections.Generic;

namespace Dots
{
    public interface IGameField
    {
        bool FirstMove { get; }
        List<List<Dot>> Field { get; }
        string Result { get; }

        void Initialyze(int size);
        List<List<Dot>> Clone();
        bool MakeMove(int i, int j);
        void CheckChains();
    }
}
