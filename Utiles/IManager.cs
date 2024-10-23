using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace FarlandsCoreMod.Utiles
{

    public interface IManager
    {
        public int Index { get; }
        public void Init();
    }

    public interface IManagerASM : IManager
    {
        public void SetASM(IEnumerable<Assembly> asm);
    }

}
