using System;

namespace ExcelToGraph
{
    public class EntryNode : Node
    {
        private readonly static Vector2 INIT_POSITION = new Vector2(100, 200);


        public EntryNode() : base(INIT_POSITION, 0)
        {

        }
    }
}
