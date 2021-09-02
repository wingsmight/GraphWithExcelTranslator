using System;
using System.Linq;
using System.Collections.Generic;

namespace ExcelToGraph
{
    public class DialogueNode : Node
    {
        private string dialogueText;
        private string speakerName;
        private List<int> responseIds;


        public DialogueNode(Vector2 position, int referenceId, string dialogueText, string speakerName, List<int> responseIds) : base(position, referenceId)
        {
            this.dialogueText = dialogueText;
            this.speakerName = speakerName;
            this.responseIds = responseIds;
        }


        public override string ToString()
        {
            string text = base.ToString() + '\n';

            text += TAB.Multiply(4) + "dialogueText: " + dialogueText + '\n';
            text += TAB.Multiply(4) + "speakerName: " + speakerName + '\n';
            text += TAB.Multiply(4) + "responses:";
            foreach (var responseId in responseIds)
            {
                text += '\n' + TAB.Multiply(4) + "- id: " + responseId;
            }

            return text;
        }
    }
}
