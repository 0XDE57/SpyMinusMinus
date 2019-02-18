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
            this.treeViewWindowList = new System.Windows.Forms.TreeView();
            this.menuStripTreeMenu = new System.Windows.Forms.MenuStrip();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
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
            this.treeViewWindowList.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeViewWindowList_BeforeExpand);
            // 
            // menuStripTreeMenu
            // 
            this.menuStripTreeMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem,
            this.testToolStripMenuItem1});
            this.menuStripTreeMenu.Location = new System.Drawing.Point(0, 0);
            this.menuStripTreeMenu.Name = "menuStripTreeMenu";
            this.menuStripTreeMenu.Size = new System.Drawing.Size(394, 24);
            this.menuStripTreeMenu.TabIndex = 3;
            this.menuStripTreeMenu.Text = "menuStripTreeMenu";
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tesToolStripMenuItem,
            this.tesToolStripMenuItem1});
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(38, 20);
            this.testToolStripMenuItem.Text = "test";
            // 
            // tesToolStripMenuItem
            // 
            this.tesToolStripMenuItem.Name = "tesToolStripMenuItem";
            this.tesToolStripMenuItem.Size = new System.Drawing.Size(89, 22);
            this.tesToolStripMenuItem.Text = "tes";
            // 
            // tesToolStripMenuItem1
            // 
            this.tesToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testToolStripMenuItem2});
            this.tesToolStripMenuItem1.Name = "tesToolStripMenuItem1";
            this.tesToolStripMenuItem1.Size = new System.Drawing.Size(89, 22);
            this.tesToolStripMenuItem1.Text = "tes";
            // 
            // testToolStripMenuItem2
            // 
            this.testToolStripMenuItem2.Name = "testToolStripMenuItem2";
            this.testToolStripMenuItem2.Size = new System.Drawing.Size(93, 22);
            this.testToolStripMenuItem2.Text = "test";
            // 
            // testToolStripMenuItem1
            // 
            this.testToolStripMenuItem1.Name = "testToolStripMenuItem1";
            this.testToolStripMenuItem1.Size = new System.Drawing.Size(38, 20);
            this.testToolStripMenuItem1.Text = "test";
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
        private System.Windows.Forms.ToolStripMenuItem tesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tesToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem1;
    }
}