using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Holo
{
    /// <summary>
    /// There are five base elements, and many composed elements.
    /// Five base element: Metal Wood Water Fire Earth
    /// Metal: Solid, Cold
    /// Wood: Vitality, Growth
    /// Water: Tensile, Tough, Splitable
    /// Fire: Mistery, Aggresive, Active, Hot
    /// Earth: sustained, Unactive
    /// 
    /// Other Ex:
    /// Metal1 + Fire10 == Steal1 + power
    /// Metal1 + Fire100 == Gold0.1 + power
    /// Earth1 + Fire10 == Stone1 + power
    /// Water1 + Fire10 == Vapor50 + Oxygen25 + Hydrogen50 + power
    /// Vapor2 + Fire0.1 == Oxygen1 + Hydrogen2 + power
    /// 
    /// </summary>
    public interface Element
    {
        string GetName();
        void SetName(string name);
        Element GetParent();
        bool TryLink(Element element);
        bool TryDeLink(Element element);
        /// <summary>
        /// Decompose a element item will make the item split into elements which are in lower level, 
        /// and release power in a calculated amount.
        /// </summary>
        /// <returns></returns>
        Element[] Decompose();
    }
}
