using System;

namespace ExcelToGraph
{
    public class CharacterPositionNode : Node
    {
        private Position characterPosition;


        public CharacterPositionNode(Vector2 position, int referenceId, Position characterPosition) : base(position, referenceId)
        {
            this.characterPosition = characterPosition;
        }


        public override string ToString()
        {
            string text = base.ToString();
            text += TAB.Multiply(4) + "characterPosition: " + (int)characterPosition;

            return text;
        }
    }
}
