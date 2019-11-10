using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace SpyMinusMinus {
    public partial class TreeForm : Form {
        
        private Panel parentPanel;

        private WindowManager windowManager;

        private Timer autoRefreshTimer;
        private bool autoRefresh = true;
        private bool showHistory = true;
        public static int markedTime = 5000;
        public static Color newColor = Color.LightGreen;
        public static Color deadColor = Color.MediumVioletRed;
     
        public TreeForm(Panel parent) {
            InitializeComponent();
            parentPanel = parent;
            Embed();
            treeViewWindowList.ImageList = imageListTreeIcons;

            showHistoryToolStripMenuItem.Checked = showHistory;
        }

        private void TreeForm_Load(object sender, EventArgs e) {
            windowManager = new WindowManager();
            windowManager.EnumerateWindows();

            
            imageListTreeIcons.Images.Add("blank", new Bitmap(1, 1));                     

            PopulateNodes();


            autoRefreshTimer = new Timer();
            autoRefreshTimer.Tick += RefreshNodes;
            if (autoRefresh) {
                toolStripMenuItemUpdate2000.PerformClick();
            }

            UpdateTitle();
        }

        private void TreeViewWindowList_DoubleClick(object sender, EventArgs e) {
            WindowNode selectedNode = (WindowNode)treeViewWindowList.SelectedNode;
            VirtualWindow selectedWindow = selectedNode.GetWindow();

            Console.WriteLine("double click " + selectedWindow.ToString());
            //selectedWindow.
            /*
            switch (behavior) {
                case properties: openProperties
                case message: openMessage
                case show: highlight
                case pullforward: bringToFront and Focus
            }
            */
        }

        private void TreeViewWindowList_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e) {
            if (e.Button == MouseButtons.Right) {
                WindowNode selectedNode = (WindowNode)e.Node;
                if (selectedNode == null) {
                    return;
                }

                ContextMenu cm = new ContextMenu();
                cm.MenuItems.Add("Properties", (send, ev) => OpenProperties(send, ev, selectedNode.GetWindow()));
                cm.MenuItems.Add("Messages", (send, ev) => OpenMessages(send, ev, selectedNode.GetWindow()));
                //cm.MenuItems.Add("Highlight");
                treeViewWindowList.ContextMenu = cm;
            }
        }

        private void OpenProperties(object sender, EventArgs e, VirtualWindow window) {
            Console.WriteLine("properties: " + window.ToString());
            window.OpenPropertiesForm();
        }

        private void OpenMessages(object sender, EventArgs e, VirtualWindow window) {
            Console.WriteLine("messages: " + window.ToString());
            window.OpenMessageLog();
        }


        #region window management    
        private void AddNewWindowNode(VirtualWindow window, bool mark) {
            WindowNode windowNode = new WindowNode(window, true);

            Icon icon = window.GetAppIcon();
            if (icon != null) {
                //Console.WriteLine("icon added for: " + window.ToString());
                imageListTreeIcons.Images.Add(window.handle.ToString(), icon);
            }

            if (mark) {
                windowNode.MarkNew();
            }

            treeViewWindowList.Nodes.Add(windowNode);
        }

        private void PopulateNodes() {
            var time = DateTime.Now;    
            treeViewWindowList.SuspendLayout();

            treeViewWindowList.Nodes.Clear();
            foreach (VirtualWindow window in windowManager.GetWindowHandles()) {
                AddNewWindowNode(window, false);
            }

            treeViewWindowList.ResumeLayout();
            Console.WriteLine("PopulateNodes: " + DateTime.Now.Subtract(time).Milliseconds);
        }

        private void RefreshNodes(object sender, EventArgs e) {
            //TODO: switch from polling approach to global hook on WM_CREATE / WM_DESTROY?
            var time = DateTime.Now;

            windowManager.EnumerateWindows();

            treeViewWindowList.SuspendLayout();

            
            //add new nodes
            windowManager.windowHandles.Where(w => !windowManager.previousHandles.Contains(w)).ToList().ForEach(window => {
                AddNewWindowNode(window, showHistory);
            });


            //remove old nodes
            windowManager.previousHandles.Where(w => !windowManager.windowHandles.Contains(w)).ToList().ForEach(window => {
                foreach (WindowNode searchNode in treeViewWindowList.Nodes) {
                    if (searchNode.GetWindow().Equals(window)) {
                        if (showHistory) {
                            searchNode.MarkDead();//todo: remove icon image when timer removes node.
                        } else {
                            imageListTreeIcons.Images.RemoveByKey(searchNode.GetWindow().handle.ToString());
                            searchNode.Remove();
                        }
                        break;
                    }
                }
            });


            foreach (WindowNode node in treeViewWindowList.Nodes) {
                if (node.IsSelected || node.IsExpanded) {
                    //node.PopulateChildrensChildren();
                    node.UpdateText();
                }
                
                //node.RefreshText();
                //Console.WriteLine(node.Text);
            }

            treeViewWindowList.ResumeLayout();

            UpdateTitle();
            Console.WriteLine("RefreshNodes: " + DateTime.Now.Subtract(time).Milliseconds);
        }

        private void UpdateTitle() {
            var time = DateTime.Now;
            Text = "Windows (" + windowManager.windowHandles.Count + ") - " + time.ToLongTimeString();
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

            //keep within parent
            if (Location.X < 0) Location = new Point(0, Location.Y);
            if (Location.Y < 0) Location = new Point(Location.X, 0);

            popoutToolStripMenuItem.Text = "Pop out";
        }
        #endregion


        #region tool strip Options
        private void TreeViewWindowList_BeforeExpand(object sender, TreeViewCancelEventArgs e) {;
            if (e.Node != null) {
                (e.Node as WindowNode).PopulateChildrensChildren();
            }
        }

        private void ShowHistoryToolStripMenuItem_Click(object sender, EventArgs e) {
            showHistory = !showHistory;
            showHistoryToolStripMenuItem.Checked = showHistory;
        }
     
       
        private void PopoutToolStripMenuItem_Click(object sender, EventArgs e) {
            if (TopLevel) {
                Embed();
            } else {
                PopOut();
            }
        }

        private void RefreshToolStripMenuItem_Click(object sender, EventArgs e) {
            //TODO: if can get event based hook on create/destroy window, make this a 
            //hard refresh (clear and rebuild tree)
            RefreshNodes(sender, e);
        }

        #region autoRefresh
        private void UpdateStrip() {
            foreach (ToolStripItem item in updateRateToolStripMenuItem.DropDownItems) {
                if (item is ToolStripMenuItem)
                    (item as ToolStripMenuItem).Checked = false;
            }
        }

        private void DisableAutoToolStripMenuItem_Click(object sender, EventArgs e) {
            autoRefreshTimer.Stop();

            UpdateStrip();
            disableAutoToolStripMenuItem.Checked = true;
        }   

        private void ToolStripMenuItemUpdate500_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 500;
            autoRefreshTimer.Start();

            UpdateStrip();
            toolStripMenuItemUpdate500.Checked = true;
        }

        private void ToolStripMenuItemUpdate1000_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 1000;
            autoRefreshTimer.Start();

            UpdateStrip();
            toolStripMenuItemUpdate1000.Checked = true;
        }

        private void toolStripMenuItemUpdate2000_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 2000;
            autoRefreshTimer.Start();

            UpdateStrip();
            toolStripMenuItemUpdate2000.Checked = true;
        }

        private void ToolStripMenuItemUpdate5000_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 5000;
            autoRefreshTimer.Start();


            UpdateStrip();
            toolStripMenuItemUpdate5000.Checked = true;
        }

        private void ToolStripMenuItemUpdate10000_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 10000;
            autoRefreshTimer.Start();

            UpdateStrip();
            toolStripMenuItemUpdate10000.Checked = true;
        }
        #endregion
        #endregion

        private void TreeForm_FormClosed(object sender, FormClosedEventArgs e) {
            autoRefreshTimer.Dispose();
            Dispose();
        }

    }

}
