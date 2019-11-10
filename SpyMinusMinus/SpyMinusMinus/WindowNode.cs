using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpyMinusMinus {
    class WindowNode : TreeNode {

        private Timer markedTimer;
        private VirtualWindow window;//the window this node is mapped to
        private bool isAlive;

        public WindowNode(VirtualWindow window) {
            this.window = window;
            isAlive = true;
            UpdateText();
            UpdateIconKey();
        }


        public WindowNode(VirtualWindow window, bool populateChildren) : this(window) {
            if (populateChildren)
                PopulateChildNodes();
        }

        public VirtualWindow GetWindow() {
            return window;
        }

        internal void UpdateText() {
            if (!isAlive) return;

            Text = window.ToString();
        }

        public Icon GetIcon() {
            return window.GetAppIcon();
        }

        private void UpdateIconKey() {
            ImageKey = window.handle.ToString();
            SelectedImageKey = ImageKey;
        }

        public void PopulateChildNodes() {
            window.PopulateChildren();
            foreach (VirtualWindow child in window.children) {
                TreeNode childNode = new WindowNode(child);
                Nodes.Add(childNode);
            }
        }

        public void PopulateChildrensChildren() {
            foreach (WindowNode child in Nodes) {
                child.PopulateChildNodes();
            }
        }

        internal void MarkNew() {
            markedTimer = new Timer();
            markedTimer.Tick += (s, e) => UnMark();
            markedTimer.Interval = TreeForm.markedTime;
            markedTimer.Start();
            BackColor = TreeForm.newColor;
        }

        internal void MarkDead() {
            isAlive = false;

            markedTimer = new Timer();
            markedTimer.Tick += (s, e) => Remove();
            markedTimer.Interval = TreeForm.markedTime;
            markedTimer.Start();
            
            BackColor = TreeForm.deadColor;
        }

        internal void UnMark() {
            BackColor = Color.White;
        }

        public override bool Equals(object obj) {
            if (obj == null) return false;

            if (obj is WindowNode) {
                return (obj as WindowNode).window.Equals(window);
            } else if (obj is VirtualWindow) {
                return (obj as VirtualWindow).Equals(window);
            }

            return false;
        }

        public override int GetHashCode() {
            return window.GetHashCode();
        }

    }
}
