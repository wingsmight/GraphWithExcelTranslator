using System;

namespace ExcelToGraph
{
    public class LocationNode : Node
    {
        private string locationName;


        public LocationNode(Vector2 position, int referenceId, string locationName) : base(position, referenceId)
        {
            this.locationName = locationName;
        }


        public override string ToString()
        {
            string text = base.ToString() + '\n';
            text += TAB.Multiply(4) + "name: " + locationName;

            return text;
        }
    }
}
