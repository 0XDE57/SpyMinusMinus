using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpyMinusMinus {
    class WindowNode : TreeNode {

        private Timer markedTimer;
        private VirtualWindow window;//the window this node is mapped to

        public WindowNode(VirtualWindow window) {
            this.window = window;
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
            Text = window.ToString();
        }

        public Icon GetIcon() {
            return window.GetAppIcon();
        }

        /*
        public void UpdateIcon(ImageList.ImageCollection imageList) {
            Icon icon = window.GetAppIcon();
            if (icon == null) {
                ImageKey = "blank";
                return;
            }
            string key = window.handle.ToString();
            if (!imageList.ContainsKey(key)) {
                imageList.Add(key, icon.ToBitmap());
                
            }
            UpdateIconKey();
        }*/

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
            markedTimer.Tick += UnMark;
            markedTimer.Interval = TreeForm.markedTime;
            markedTimer.Start();
            BackColor = TreeForm.newColor;
        }

        internal void MarkDead() {
            markedTimer = new Timer();
            markedTimer.Tick += (o, e) => Remove();
            markedTimer.Interval = TreeForm.markedTime;
            markedTimer.Start();
            BackColor = TreeForm.deadColor;
        }

        internal void UnMark(object sender, EventArgs e) {
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
