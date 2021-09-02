using System;

namespace ExcelToGraph
{
    public class NodeLinkData
    {
        private const string DEFAULT_SOURCE_PORT_NAME = "Output";


        protected int referenceId;
        protected string sourceNodeGUID;
        protected string destinationNodeGUID;
        protected string sourcePortName = DEFAULT_SOURCE_PORT_NAME;


        public NodeLinkData(int referenceId, string sourceNodeGUID, string destinationNodeGUID)
        {
            this.referenceId = referenceId;
            this.sourceNodeGUID = sourceNodeGUID;
            this.destinationNodeGUID = destinationNodeGUID;
        }
        public NodeLinkData(int referenceId, string sourceNodeGUID, string destinationNodeGUID, string sourcePortName)
            : this(referenceId, sourceNodeGUID, destinationNodeGUID)
        {
            this.sourcePortName = sourcePortName;
        }


        public override string ToString()
        {
            string text = "";
            text += Node.TAB.Multiply(2) + referenceId.ToString("X8") + ":" + '\n';

            text += Node.TAB.Multiply(3) + "type: " + "{class: " + GetType().Name + ", ns: , asm: Assembly-CSharp}" + '\n';
            text += Node.TAB.Multiply(3) + "data:" + '\n';

            text += Node.TAB.Multiply(4) + "sourceNodeGUID: " + sourceNodeGUID + '\n';
            text += Node.TAB.Multiply(4) + "sourcePortName: " + DEFAULT_SOURCE_PORT_NAME + '\n';
            text += Node.TAB.Multiply(4) + "destinationNodeGUID: " + destinationNodeGUID;

            return text;
        }


        public int ReferenceId => referenceId;
    }
}
