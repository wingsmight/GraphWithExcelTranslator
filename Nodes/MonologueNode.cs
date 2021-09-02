using System;
using System.Linq;
using System.Collections.Generic;

namespace ExcelToGraph
{
    public class MonologueNode : Node
    {
        private List<string> texts;
        private string speakerName;


        public MonologueNode(Vector2 position, int referenceId, List<string> texts, string speakerName) : base(position, referenceId)
        {
            this.texts = texts;
            this.speakerName = speakerName;
        }


        public override string ToString()
        {
            string text = base.ToString() + '\n';
            text += TAB.Multiply(4) + "texts:" + '\n';

            foreach (var textElement in texts)
            {
                text += TAB.Multiply(5) + "- \"" + textElement + "\"" + '\n';
            }
            text += TAB.Multiply(4) + "speakerName: " + speakerName;

            return text;
        }
    }
}
