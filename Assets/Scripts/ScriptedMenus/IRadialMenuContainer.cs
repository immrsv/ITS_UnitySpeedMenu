using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RadialMenu.ScriptedMenus {
    public interface IRadialMenuContainer {
        bool HasChildren { get; }
        List<RadialMenu_MenuItem> Children { get; }
    }
}
