using System;
using System.Collections.Generic;
using System.Text;

namespace FarlandsCoreMod.Utiles
{

    public interface IManager
    {
        public int Index { get; }
        public void Init();
    }
}
