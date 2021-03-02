using System.Windows.Forms;

namespace System
{
    internal class PaintEventHandler
    {
        private Action<object, PaintEventArgs> toolStripButton4_Click;

        public PaintEventHandler(Action<object, PaintEventArgs> toolStripButton4_Click)
        {
            this.toolStripButton4_Click = toolStripButton4_Click;
        }
    }
}