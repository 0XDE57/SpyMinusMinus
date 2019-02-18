using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace SpyMinusMinus {
    public partial class TreeForm : Form {

        private Panel parentPanel;

        private List<VirtualWindow> windowHandles;
        private List<VirtualWindow> previousHandles;
        private Timer autoRefreshTimer;

        //TODO: move values to config
        private bool autoRefresh = true;
        private bool showHistory = true;
        private int refreshInterval = 2000;
        public static int markedTime = 5000;
        public static Color newColor = Color.LightGreen;
        public static Color deadColor = Color.MediumVioletRed;

        
        public TreeForm(Panel parent) {
            InitializeComponent();
            this.parentPanel = parent;
            Embed();
            /*
             *  TODO:
             *      [ ] desktop window -> GetDesktopWindow
             *      [...] tree options
             *          [...] auto refresh, refresh rate, history (red removed, green added)
             *          [ ] view handles as int, hex, both
             *          [ ] view hex upper or lower case
             *      [ ] sort: ascending/descending
             *              
            */
        }

        private void TreeForm_Load(object sender, EventArgs e) {
            windowHandles = new List<VirtualWindow>();
            previousHandles = new List<VirtualWindow>();

            EnumerateWindows();          
            PopulateNodes();

            //windowHandles.Sort();
            //windowHandles.ForEach(window => Console.WriteLine(window.ToString()));
            if (autoRefresh) {
                autoRefreshTimer = new Timer();
                autoRefreshTimer.Interval = refreshInterval;
                autoRefreshTimer.Tick += RefreshNodes;
                autoRefreshTimer.Start();
            }
        }

 
        private void EnumerateWindows() {
            windowHandles.Clear();
            NativeMethods.EnumWindows(new NativeMethods.EnumWindowProc(EnumWindow), IntPtr.Zero);
        }

        private void PopulateNodes() {
            var time = DateTime.Now;
            treeViewWindowList.SuspendLayout();

            treeViewWindowList.Nodes.Clear();
            foreach (VirtualWindow window in windowHandles.ToArray()) {
                WindowNode windowNode = new WindowNode(window, true);
                treeViewWindowList.Nodes.Add(windowNode);
            }

            treeViewWindowList.ResumeLayout();
            Console.WriteLine(DateTime.Now.Subtract(time).Milliseconds);
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

        private void testToolStripMenuItemRefresh_Click(object sender, EventArgs e) {
            RefreshNodes(sender, e);
        }

        private void RefreshNodes(object sender, EventArgs e) {
            var time = DateTime.Now;
            treeViewWindowList.SuspendLayout();

            previousHandles.Clear();
            previousHandles.AddRange(windowHandles.ToArray());

            EnumerateWindows();

            //new nodes
            windowHandles.Where(w => !previousHandles.Contains(w)).ToList().ForEach(w => {
                WindowNode newNode = new WindowNode(w, true);
                if (showHistory)
                    newNode.MarkNew();

                treeViewWindowList.Nodes.Add(newNode);
            });

            
            //old nodes
            previousHandles.Where(w => !windowHandles.Contains(w)).ToList().ForEach(w => {
                foreach (WindowNode searchNode in treeViewWindowList.Nodes) {
                    if (searchNode.GetWindow().Equals(w)) {
                        if (showHistory) {
                            searchNode.MarkDead();
                        } else {
                            searchNode.Remove();
                        }
                        break;
                    }
                }
            });

            
            foreach (WindowNode node in treeViewWindowList.Nodes) {
                if (node.IsSelected || node.IsExpanded) {
                    //node.PopulateChildrensChildren();
                    node.RefreshText();
                }
                //node.RefreshText();
                //Console.WriteLine(node.Text);
            }

            treeViewWindowList.ResumeLayout();

            Console.WriteLine(DateTime.Now.Subtract(time).Milliseconds);
        }

        private void popoutToolStripMenuItem_Click(object sender, EventArgs e) {
            if (TopLevel) {
                Embed();
            } else {
                PopOut();
            }
        }

        private void PopOut() {
            //pop out into external window
            Parent.Controls.Remove(this);
            TopLevel = true;
            popoutToolStripMenuItem.Text = "Dock";
        }

        private void Embed() {
            //embed form inside main panel
            TopLevel = false;
            parentPanel.Controls.Add(this);
            popoutToolStripMenuItem.Text = "Popout";
        }
    }

    class WindowNode : TreeNode {

        private Timer markedTimer;
        private VirtualWindow window;//the window this node is mapped to

        public WindowNode(VirtualWindow window) {
            this.window = window;
            Text = window.ToString();         
        }

        public WindowNode(VirtualWindow window, bool populateChildren) : this(window) {
            if (populateChildren)
                PopulateChildNodes();
        }

        public VirtualWindow GetWindow() {
            return window;
        }

        internal void RefreshText() {
            Text = window.ToString();
            //Console.WriteLine(Text);
        }

        public void PopulateChildNodes() {
            window.PopulateChildren();
            //Nodes.Clear();
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
