using System;

namespace ExcelToGraph
{
    public abstract class Node
    {
        public const string TAB = "  ";


        protected Vector2 position;
        protected int referenceId;
        protected string guid;


        public Node(Vector2 position, int referenceId)
        {
            this.position = position;
            this.referenceId = referenceId;
            this.guid = Guid.NewGuid().ToString();
        }


        public override string ToString()
        {
            string text = "";
            text += TAB.Multiply(2) + referenceId.ToString("X8") + ":" + '\n';

            text += TAB.Multiply(3) + "type: " + "{class: " + GetType().Name + ", ns: , asm: Assembly-CSharp}" + '\n';
            text += TAB.Multiply(3) + "data:" + '\n';

            text += TAB.Multiply(4) + "guid: " + guid + '\n';
            text += TAB.Multiply(4) + "position: " + "{x: " + position.x + ", y: " + position.y + "}";

            return text;
        }


        public string GUID => guid;
        public int ReferenceId { get => referenceId; set => referenceId = value; }
        public Vector2 Position { get => position; set => position = value; }
    }
}
