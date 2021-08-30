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
            text += TAB.Multiply(2) + referenceId.ToString("D8") + ":" + '\n';

            text += TAB.Multiply(3) + "type: " + "{class: " + GetType().Name + ", ns: , asm: Assembly-CSharp}" + '\n';
            text += TAB.Multiply(3) + "data:" + '\n';

            text += TAB.Multiply(4) + "guid: " + "1b4b375a-e982-4645-8274-70940803fe55" + '\n';
            text += TAB.Multiply(4) + "position: " + "{x: " + position.x + ", y: " + position.y + "}" + '\n';

            return text;
        }
    }
}