using System;
using System.Collections.Generic;

namespace ExcelToGraph
{
    public class CharacterNode : Node
    {
        private static readonly Dictionary<Position, Direction> defaultDirections = new Dictionary<Position, Direction>()
        {
            {ExcelToGraph.Position.Left, Direction.FromLeft},
            {ExcelToGraph.Position.Middle, Direction.FromLeft},
            {ExcelToGraph.Position.Right, Direction.FromRight}
        };


        private CharacterProperty property;
        private Direction direction;


        public CharacterNode(Vector2 position, int referenceId, CharacterProperty property, Direction direction)
            : base(position, referenceId)
        {
            this.property = property;
            this.direction = direction;
        }
        public CharacterNode(Vector2 position, int referenceId, CharacterProperty property)
            : this(position, referenceId, property, ConvertToDirection(property.position))
        {

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

        private static Direction ConvertToDirection(Position position)
        {
            return defaultDirections[position];
        }
    }
}
