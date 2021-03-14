using GridLevelEditor.Objects;
using System.Collections.Generic;

namespace GridLevelEditor.Models
{
    public class Level
    {
        public string Name { get; set; }
        public List<MgElem> Elems { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public string[][] Data { get; set; }

        public Level()
        {
            Name = "";
            Elems = new List<MgElem>();
            Height = 0;
            Width = 0;
            Data = new string[0][];
        }
    }
}
