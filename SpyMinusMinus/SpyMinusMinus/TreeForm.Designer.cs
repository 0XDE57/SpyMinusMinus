namespace SpyMinusMinus {
    partial class TreeForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.treeViewWindowList = new System.Windows.Forms.TreeView();
            this.menuStripTreeMenu = new System.Windows.Forms.MenuStrip();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.disableAutoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemUpdate500 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUpdate1000 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUpdate2000 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUpdate5000 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemUpdate10000 = new System.Windows.Forms.ToolStripMenuItem();
            this.showHistoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.popoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageListTreeIcons = new System.Windows.Forms.ImageList(this.components);
            this.menuStripTreeMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewWindowList
            // 
            this.treeViewWindowList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewWindowList.Location = new System.Drawing.Point(0, 27);
            this.treeViewWindowList.Name = "treeViewWindowList";
            this.treeViewWindowList.Size = new System.Drawing.Size(394, 473);
            this.treeViewWindowList.TabIndex = 1;
            this.treeViewWindowList.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.TreeViewWindowList_BeforeExpand);
            this.treeViewWindowList.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewWindowList_NodeMouseClick);
            this.treeViewWindowList.DoubleClick += new System.EventHandler(this.TreeViewWindowList_DoubleClick);
            // 
            // menuStripTreeMenu
            // 
            this.menuStripTreeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem,
            this.refreshToolStripMenuItem,
            this.popoutToolStripMenuItem});
            this.menuStripTreeMenu.Location = new System.Drawing.Point(0, 0);
            this.menuStripTreeMenu.Name = "menuStripTreeMenu";
            this.menuStripTreeMenu.Size = new System.Drawing.Size(394, 24);
            this.menuStripTreeMenu.TabIndex = 3;
            this.menuStripTreeMenu.Text = "menuStripTreeMenu";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateRateToolStripMenuItem,
            this.showHistoryToolStripMenuItem});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.testToolStripMenuItem.Text = "Settings";
            // 
            // updateRateToolStripMenuItem
            // 
            this.updateRateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableAutoToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItemUpdate500,
            this.toolStripMenuItemUpdate1000,
            this.toolStripMenuItemUpdate2000,
            this.toolStripMenuItemUpdate5000,
            this.toolStripMenuItemUpdate10000});
            this.updateRateToolStripMenuItem.Name = "updateRateToolStripMenuItem";
            this.updateRateToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.updateRateToolStripMenuItem.Text = "Update Rate (ms)";
            // 
            // disableAutoToolStripMenuItem
            // 
            this.disableAutoToolStripMenuItem.Name = "disableAutoToolStripMenuItem";
            this.disableAutoToolStripMenuItem.Size = new System.Drawing.Size(139, 22);
            this.disableAutoToolStripMenuItem.Text = "Disable auto";
            this.disableAutoToolStripMenuItem.Click += new System.EventHandler(this.DisableAutoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(136, 6);
            // 
            // toolStripMenuItemUpdate500
            // 
            this.toolStripMenuItemUpdate500.Name = "toolStripMenuItemUpdate500";
            this.toolStripMenuItemUpdate500.Size = new System.Drawing.Size(139, 22);
            this.toolStripMenuItemUpdate500.Text = "500";
            this.toolStripMenuItemUpdate500.Click += new System.EventHandler(this.ToolStripMenuItemUpdate500_Click);
            // 
            // toolStripMenuItemUpdate1000
            // 
            this.toolStripMenuItemUpdate1000.Name = "toolStripMenuItemUpdate1000";
            this.toolStripMenuItemUpdate1000.Size = new System.Drawing.Size(139, 22);
            this.toolStripMenuItemUpdate1000.Text = "1000";
            this.toolStripMenuItemUpdate1000.Click += new System.EventHandler(this.ToolStripMenuItemUpdate1000_Click);
            // 
            // toolStripMenuItemUpdate2000
            // 
            this.toolStripMenuItemUpdate2000.Name = "toolStripMenuItemUpdate2000";
            this.toolStripMenuItemUpdate2000.Size = new System.Drawing.Size(139, 22);
            this.toolStripMenuItemUpdate2000.Text = "2000";
            this.toolStripMenuItemUpdate2000.Click += new System.EventHandler(this.toolStripMenuItemUpdate2000_Click);
            // 
            // toolStripMenuItemUpdate5000
            // 
            this.toolStripMenuItemUpdate5000.Name = "toolStripMenuItemUpdate5000";
            this.toolStripMenuItemUpdate5000.Size = new System.Drawing.Size(139, 22);
            this.toolStripMenuItemUpdate5000.Text = "5000";
            this.toolStripMenuItemUpdate5000.Click += new System.EventHandler(this.ToolStripMenuItemUpdate5000_Click);
            // 
            // toolStripMenuItemUpdate10000
            // 
            this.toolStripMenuItemUpdate10000.Name = "toolStripMenuItemUpdate10000";
            this.toolStripMenuItemUpdate10000.Size = new System.Drawing.Size(139, 22);
            this.toolStripMenuItemUpdate10000.Text = "10000";
            this.toolStripMenuItemUpdate10000.Click += new System.EventHandler(this.ToolStripMenuItemUpdate10000_Click);
            // 
            // showHistoryToolStripMenuItem
            // 
            this.showHistoryToolStripMenuItem.Name = "showHistoryToolStripMenuItem";
            this.showHistoryToolStripMenuItem.Size = new System.Drawing.Size(165, 22);
            this.showHistoryToolStripMenuItem.Text = "Show History";
            this.showHistoryToolStripMenuItem.Click += new System.EventHandler(this.ShowHistoryToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.AutoToolTip = true;
            this.refreshToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.refreshToolStripMenuItem.Image = global::SpyMinusMinus.Properties.Resources.arrow_circle_double;
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.ToolTipText = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.RefreshToolStripMenuItem_Click);
            // 
            // popoutToolStripMenuItem
            // 
            this.popoutToolStripMenuItem.AutoToolTip = true;
            this.popoutToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.popoutToolStripMenuItem.Image = global::SpyMinusMinus.Properties.Resources.applications_blue;
            this.popoutToolStripMenuItem.Name = "popoutToolStripMenuItem";
            this.popoutToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.popoutToolStripMenuItem.Text = "Popout";
            this.popoutToolStripMenuItem.ToolTipText = "Pop out";
            this.popoutToolStripMenuItem.Click += new System.EventHandler(this.PopoutToolStripMenuItem_Click);
            // 
            // imageListTreeIcons
            // 
            this.imageListTreeIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageListTreeIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.imageListTreeIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // TreeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 501);
            this.Controls.Add(this.treeViewWindowList);
            this.Controls.Add(this.menuStripTreeMenu);
            this.Name = "TreeForm";
            this.Text = "Windows";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TreeForm_FormClosed);
            this.Load += new System.EventHandler(this.TreeForm_Load);
            this.menuStripTreeMenu.ResumeLayout(false);
            this.menuStripTreeMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewWindowList;
        private System.Windows.Forms.MenuStrip menuStripTreeMenu;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateRateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem disableAutoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUpdate500;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUpdate1000;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUpdate2000;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUpdate5000;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemUpdate10000;
        private System.Windows.Forms.ToolStripMenuItem showHistoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem popoutToolStripMenuItem;
        private System.Windows.Forms.ImageList imageListTreeIcons;
    }
}