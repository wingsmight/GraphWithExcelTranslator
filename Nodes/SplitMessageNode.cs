using System;
using System.Linq;
using System.Collections.Generic;

namespace ExcelToGraph
{
    public class SplitMessageNode : Node
    {
        private List<string> texts;


        public SplitMessageNode(Vector2 position, int referenceId, List<string> texts) : base(position, referenceId)
        {
            this.texts = texts;
        }


        public override string ToString()
        {
            string text = base.ToString() + '\n';

            text += TAB.Multiply(4) + "text:";
            foreach (var textElement in texts)
            {
                text += '\n' + TAB.Multiply(4) + "- \"" + textElement + "\"";
            }

            return text;
        }
    }
}
