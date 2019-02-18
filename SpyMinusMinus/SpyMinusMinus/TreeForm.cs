﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace SpyMinusMinus {
    public partial class TreeForm : Form {
        /*
         *  TODO:
         *      [ ] desktop window -> GetDesktopWindow
         *      [...] tree options
         *          [x] auto refresh
         *          [x] refresh rate
         *          [...] history (red removed, green added)
         *          [...] view handles as int, hex, both
         *              [ ] view hex upper or lower case
         *      [ ] sort: ascending/descending
         *      [ ] move options to properties
         *      
         */
        private Panel parentPanel;

        private List<VirtualWindow> windowHandles;
        private List<VirtualWindow> previousHandles;
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

            showHistoryToolStripMenuItem.Checked = showHistory;
        }

        private void TreeForm_Load(object sender, EventArgs e) {
            windowHandles = new List<VirtualWindow>();
            previousHandles = new List<VirtualWindow>();

            EnumerateWindows();          
            PopulateNodes();

            //windowHandles.Sort();
            //windowHandles.ForEach(window => Console.WriteLine(window.ToString()));

            autoRefreshTimer = new Timer();
            autoRefreshTimer.Tick += RefreshNodes;
            if (autoRefresh) {
                toolStripMenuItemUpdate2000.PerformClick();
            }
        }

        #region window management
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

        private void RefreshNodes(object sender, EventArgs e) {         
            var time = DateTime.Now;
            Text = "Windows - " + time.ToLongTimeString();
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

        private void EnumerateWindows() {
            windowHandles.Clear();
            NativeMethods.EnumWindows(new NativeMethods.EnumWindowProc(EnumWindow), IntPtr.Zero);
        }

        private bool EnumWindow(IntPtr hWnd, IntPtr lParam) {
            windowHandles.Add(new VirtualWindow(hWnd));
            return true; //continue enumeration
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
        #endregion

        #region tool strip Options
        private void treeViewWindowList_BeforeExpand(object sender, TreeViewCancelEventArgs e) {;
            if (e.Node != null) {
                (e.Node as WindowNode).PopulateChildrensChildren();
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
            RefreshNodes(sender, e);
        }

        private void popoutToolStripMenuItem_Click(object sender, EventArgs e) {
            if (TopLevel) {
                Embed();
            } else {
                PopOut();
            }
        }

        private void showHistoryToolStripMenuItem_Click(object sender, EventArgs e) {
            showHistory = !showHistory;
            showHistoryToolStripMenuItem.Checked = showHistory;
        }

        #region autoRefresh
        private void disableAutoToolStripMenuItem_Click(object sender, EventArgs e) {
            autoRefreshTimer.Stop();

            foreach (ToolStripItem item in updateRateToolStripMenuItem.DropDownItems) {
                if (item is ToolStripMenuItem)
                    (item as ToolStripMenuItem).Checked = false;
            }
            disableAutoToolStripMenuItem.Checked = true;
        }

        private void toolStripMenuItemUpdate500_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 500;
            autoRefreshTimer.Start();
                  
            foreach (ToolStripItem item in updateRateToolStripMenuItem.DropDownItems) {
                if (item is ToolStripMenuItem)
                    (item as ToolStripMenuItem).Checked = false;
            }
            toolStripMenuItemUpdate500.Checked = true;
        }

        private void toolStripMenuItemUpdate1000_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 1000;
            autoRefreshTimer.Start();

            foreach (ToolStripItem item in updateRateToolStripMenuItem.DropDownItems) {
                if (item is ToolStripMenuItem)
                    (item as ToolStripMenuItem).Checked = false;
            }
            toolStripMenuItemUpdate1000.Checked = true;
        }

        private void toolStripMenuItemUpdate2000_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 2000;
            autoRefreshTimer.Start();

            foreach (ToolStripItem item in updateRateToolStripMenuItem.DropDownItems) {
                if (item is ToolStripMenuItem)
                    (item as ToolStripMenuItem).Checked = false;
            }
            toolStripMenuItemUpdate2000.Checked = true;
        }

        private void toolStripMenuItemUpdate5000_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 5000;
            autoRefreshTimer.Start();


            foreach (ToolStripItem item in updateRateToolStripMenuItem.DropDownItems) {
                if (item is ToolStripMenuItem)
                    (item as ToolStripMenuItem).Checked = false;
            }
            toolStripMenuItemUpdate5000.Checked = true;
        }

        private void toolStripMenuItemUpdate10000_Click(object sender, EventArgs e) {
            autoRefreshTimer.Interval = 10000;
            autoRefreshTimer.Start();

            foreach (ToolStripItem item in updateRateToolStripMenuItem.DropDownItems) {
                if (item is ToolStripMenuItem)
                    (item as ToolStripMenuItem).Checked = false;
            }
            toolStripMenuItemUpdate10000.Checked = true;
        }
        #endregion
        #endregion

        private void TreeForm_FormClosed(object sender, FormClosedEventArgs e) {
            autoRefreshTimer.Dispose();
            Dispose();
        }

        private void lowerCaseToolStripMenuItem_Click(object sender, EventArgs e) {

        }
    }

}
