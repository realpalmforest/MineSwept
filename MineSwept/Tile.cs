using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MineSwept;

public struct Tile
{
    public bool Covered { get; set; } = true;
    public bool Flagged { get; set; } = false;
    public bool Mine { get; set; } = false;

    public Tile()
    {

    }
}