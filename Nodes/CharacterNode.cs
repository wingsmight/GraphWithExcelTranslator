using System;

namespace ExcelToGraph
{
    public class CharacterNode : Node
    {
        private CharacterProperty property;
        private Direction direction;


        public CharacterNode(Vector2 position, int referenceId, CharacterProperty property, Direction direction) : base(position, referenceId)
        {
            this.property = property;
            this.direction = direction;
        }


        public override string ToString()
        {
            string text = base.ToString() + '\n';
            text += TAB.Multiply(4) + "character:" + '\n';

            text += TAB.Multiply(5) + "name: " + property.name + '\n';
            text += TAB.Multiply(5) + "position: " + (int)property.position + '\n';
            text += TAB.Multiply(5) + "emotion: " + (int)property.emotion + '\n';

            text += TAB.Multiply(4) + "direction: " + (int)direction;

            return text;
        }
    }
}
