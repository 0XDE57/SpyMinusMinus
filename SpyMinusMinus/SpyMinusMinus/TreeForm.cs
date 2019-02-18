using System;
using System.Collections;
using System.Windows.Forms;

namespace SpyMinusMinus {
    public partial class TreeForm : Form {

        ArrayList windowHandles = new ArrayList();

        public TreeForm() {
            InitializeComponent();
        }

        private void TreeForm_Load(object sender, EventArgs e) {
            PopulateNodes();
        }

        private void PopulateNodes() {
            windowHandles.Clear();

            NativeMethods.EnumWindows(new NativeMethods.EnumWindowProc(EnumWindow), IntPtr.Zero);

            foreach (VirtualWindow window in windowHandles.ToArray()) {
                WindowNode windowNode = new WindowNode(window, true);
                treeViewWindowList.Nodes.Add(windowNode);
            }
        }


        private bool EnumWindow(IntPtr hWnd, IntPtr lParam) {
            windowHandles.Add(new VirtualWindow(hWnd));
            return true; //continue enumeration
        }

        private void treeViewWindowList_BeforeExpand(object sender, TreeViewCancelEventArgs e) {;
            if (e.Node != null) {
                (e.Node as WindowNode).PopulateChildrensChildren();
            }
        }
    }

    class WindowNode : TreeNode {

        private VirtualWindow window;//the window this node is mapped to

        public WindowNode(VirtualWindow window) {
            this.window = window;
            Text = window.ToString();
        }

        public WindowNode(VirtualWindow window, bool populateChildren) : this(window) {
            if (populateChildren)
                PopulateChildNodes();
        }
        
        
        public void PopulateChildNodes() {
            window.PopulateChildren();
            foreach (VirtualWindow child in window.children.ToArray()) {             
                TreeNode childNode = new WindowNode(child);
                Nodes.Add(childNode);
            }
        }

        public void PopulateChildrensChildren() {
            foreach (WindowNode child in Nodes) {
                child.PopulateChildNodes();
            }
        }
    }
}
