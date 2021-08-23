namespace Cellular
{
    /// <summary>
    /// Abstract class for all 2D automata.
    /// When indexing, ("height first") the distance from the top comes before the distance from the left border.
    /// </summary>
    abstract class Automaton2D : CellularAutomaton
    {
        protected int width, height;
    }
}
