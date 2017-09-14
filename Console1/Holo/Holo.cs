using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holo
{
    /// <summary>
    /// HOLO Only Light from Orient (Opener Origin Oridinary Odd)
    /// </summary>
    public interface Holo
    {
    }

    /// <summary>
    /// Holo has five base elements, and many composed elements.
    /// This interface define all behaviors that a element will perform.
    /// </summary>
    /// Five base element: Metal Wood Water Fire Earth
    /// Metal: Solid, Cold, Anti-Oxidation
    /// Wood: Vitality, Growth, Easy-Oxidation
    /// Water: Tensile, Tough, Splitable, Non-Oxidation
    /// Fire: Mistery, Aggresive, Active, Hot, Non-Oxidation
    /// Earth: sustained, Unactive, Easy-Oxidation
    /// 
    /// Other Ex:
    /// Metal1 + Fire10 == Steal1 + power
    /// Metal1 + Fire100 == Gold0.1 + power
    /// Earth1 + Fire10 == Stone1 + power
    /// Water1 + Fire10 == Vapor50 + Oxygen25 + Hydrogen50 + power
    /// Vapor2 + Fire0.1 == Oxygen1 + Hydrogen2 + power
    public interface Element : Holo
    {
        bool TryLink(Element element);
        bool TryDeLink(Element element);
        /// <summary>
        /// Decompose a element item will consume power, make the item split into elements which are in lower level, and release power in a calculated amount.
        /// </summary>
        Element[] Decompose(Power power);
        void TakeIn(Power power);
        Element[] TakeOut();
    }

    /// <summary>
    /// The energy part of Holo
    /// </summary>
    /// Every base element has its own type of Power, and a few of composed elements have their unique Power.
    /// Ex. Metal:Electric Wood:Growth Water:Pure Fire:Thermal & Light Earth:Physical
    /// Oxygen:Oxidation 
    public interface Power : Holo
    {
        float GetAmount();
    }

    /// <summary>
    /// The most mistery part of Holo
    /// </summary>
    public interface Mental : Holo
    {

    }

    public interface BaseElement : Element
    { }

    public interface ComposeElement : Element
    {
        Element[] GetComposeParent();
    }
}
