
namespace NarabeDataGenerator
{
    partial class FormMain
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemInitDir = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemExePath = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDesktop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonFile1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFile2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBoxApp = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBoxType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripComboBoxFile = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButtonGen = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.splitContainerFile = new System.Windows.Forms.SplitContainer();
            this.listBoxFile1 = new System.Windows.Forms.ListBox();
            this.contextMenuStripFile1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.削除DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxFile2 = new System.Windows.Forms.ListBox();
            this.contextMenuStripFile2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.削除DToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainerLogText = new System.Windows.Forms.SplitContainer();
            this.listBoxLog = new System.Windows.Forms.ListBox();
            this.textBoxData = new System.Windows.Forms.TextBox();
            this.toolStripComboBoxPass = new System.Windows.Forms.ToolStripComboBox();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFile)).BeginInit();
            this.splitContainerFile.Panel1.SuspendLayout();
            this.splitContainerFile.Panel2.SuspendLayout();
            this.splitContainerFile.SuspendLayout();
            this.contextMenuStripFile1.SuspendLayout();
            this.contextMenuStripFile2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogText)).BeginInit();
            this.splitContainerLogText.Panel1.SuspendLayout();
            this.splitContainerLogText.Panel2.SuspendLayout();
            this.splitContainerLogText.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemFile,
            this.ToolStripMenuItemHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItemFile
            // 
            this.ToolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemInitDir,
            this.toolStripMenuItem1,
            this.ToolStripMenuItemExit});
            this.ToolStripMenuItemFile.Name = "ToolStripMenuItemFile";
            this.ToolStripMenuItemFile.Size = new System.Drawing.Size(67, 20);
            this.ToolStripMenuItemFile.Text = "ファイル(&F)";
            // 
            // ToolStripMenuItemInitDir
            // 
            this.ToolStripMenuItemInitDir.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemExePath,
            this.ToolStripMenuItemDesktop});
            this.ToolStripMenuItemInitDir.Name = "ToolStripMenuItemInitDir";
            this.ToolStripMenuItemInitDir.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItemInitDir.Text = "初期ディレクトリ(&I)";
            // 
            // ToolStripMenuItemExePath
            // 
            this.ToolStripMenuItemExePath.Name = "ToolStripMenuItemExePath";
            this.ToolStripMenuItemExePath.Size = new System.Drawing.Size(181, 22);
            this.ToolStripMenuItemExePath.Text = "ExecutablePath(&E)";
            this.ToolStripMenuItemExePath.Click += new System.EventHandler(this.ToolStripMenuItemExePath_Click);
            // 
            // ToolStripMenuItemDesktop
            // 
            this.ToolStripMenuItemDesktop.Name = "ToolStripMenuItemDesktop";
            this.ToolStripMenuItemDesktop.Size = new System.Drawing.Size(181, 22);
            this.ToolStripMenuItemDesktop.Text = "DesktopDirectory(&D)";
            this.ToolStripMenuItemDesktop.Click += new System.EventHandler(this.ToolStripMenuItemDesktop_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(156, 6);
            // 
            // ToolStripMenuItemExit
            // 
            this.ToolStripMenuItemExit.Name = "ToolStripMenuItemExit";
            this.ToolStripMenuItemExit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.ToolStripMenuItemExit.Size = new System.Drawing.Size(159, 22);
            this.ToolStripMenuItemExit.Text = "終了(&X)";
            this.ToolStripMenuItemExit.Click += new System.EventHandler(this.ToolStripMenuItemExit_Click);
            // 
            // ToolStripMenuItemHelp
            // 
            this.ToolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemAbout});
            this.ToolStripMenuItemHelp.Name = "ToolStripMenuItemHelp";
            this.ToolStripMenuItemHelp.Size = new System.Drawing.Size(65, 20);
            this.ToolStripMenuItemHelp.Text = "ヘルプ(&H)";
            // 
            // ToolStripMenuItemAbout
            // 
            this.ToolStripMenuItemAbout.Name = "ToolStripMenuItemAbout";
            this.ToolStripMenuItemAbout.Size = new System.Drawing.Size(158, 22);
            this.ToolStripMenuItemAbout.Text = "バージョン情報(&A)";
            this.ToolStripMenuItemAbout.Click += new System.EventHandler(this.ToolStripMenuItemAbout_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonFile1,
            this.toolStripButtonFile2,
            this.toolStripSeparator1,
            this.toolStripComboBoxApp,
            this.toolStripComboBoxType,
            this.toolStripComboBoxPass,
            this.toolStripComboBoxFile,
            this.toolStripSeparator2,
            this.toolStripButtonGen,
            this.toolStripButtonSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(784, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButtonFile1
            // 
            this.toolStripButtonFile1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonFile1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFile1.Image")));
            this.toolStripButtonFile1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFile1.Name = "toolStripButtonFile1";
            this.toolStripButtonFile1.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonFile1.Text = "ファイル1";
            this.toolStripButtonFile1.Click += new System.EventHandler(this.toolStripButtonFile1_Click);
            // 
            // toolStripButtonFile2
            // 
            this.toolStripButtonFile2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonFile2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonFile2.Image")));
            this.toolStripButtonFile2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFile2.Name = "toolStripButtonFile2";
            this.toolStripButtonFile2.Size = new System.Drawing.Size(51, 22);
            this.toolStripButtonFile2.Text = "ファイル2";
            this.toolStripButtonFile2.Click += new System.EventHandler(this.toolStripButtonFile2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripComboBoxApp
            // 
            this.toolStripComboBoxApp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxApp.Name = "toolStripComboBoxApp";
            this.toolStripComboBoxApp.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripComboBoxType
            // 
            this.toolStripComboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxType.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripComboBoxType.Name = "toolStripComboBoxType";
            this.toolStripComboBoxType.Size = new System.Drawing.Size(200, 25);
            // 
            // toolStripComboBoxFile
            // 
            this.toolStripComboBoxFile.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripComboBoxFile.Name = "toolStripComboBoxFile";
            this.toolStripComboBoxFile.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButtonGen
            // 
            this.toolStripButtonGen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonGen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonGen.Image")));
            this.toolStripButtonGen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonGen.Name = "toolStripButtonGen";
            this.toolStripButtonGen.Size = new System.Drawing.Size(35, 22);
            this.toolStripButtonGen.Text = "作成";
            this.toolStripButtonGen.Click += new System.EventHandler(this.toolStripButtonGen_Click);
            // 
            // toolStripButtonSave
            // 
            this.toolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonSave.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonSave.Image")));
            this.toolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonSave.Name = "toolStripButtonSave";
            this.toolStripButtonSave.Size = new System.Drawing.Size(35, 22);
            this.toolStripButtonSave.Text = "保存";
            this.toolStripButtonSave.Click += new System.EventHandler(this.toolStripButtonSave_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 419);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 49);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.splitContainerFile);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.splitContainerLogText);
            this.splitContainerMain.Size = new System.Drawing.Size(784, 370);
            this.splitContainerMain.SplitterDistance = 180;
            this.splitContainerMain.TabIndex = 3;
            // 
            // splitContainerFile
            // 
            this.splitContainerFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerFile.Location = new System.Drawing.Point(0, 0);
            this.splitContainerFile.Name = "splitContainerFile";
            this.splitContainerFile.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerFile.Panel1
            // 
            this.splitContainerFile.Panel1.Controls.Add(this.listBoxFile1);
            // 
            // splitContainerFile.Panel2
            // 
            this.splitContainerFile.Panel2.Controls.Add(this.listBoxFile2);
            this.splitContainerFile.Size = new System.Drawing.Size(784, 180);
            this.splitContainerFile.SplitterDistance = 90;
            this.splitContainerFile.TabIndex = 0;
            // 
            // listBoxFile1
            // 
            this.listBoxFile1.ContextMenuStrip = this.contextMenuStripFile1;
            this.listBoxFile1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFile1.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBoxFile1.FormattingEnabled = true;
            this.listBoxFile1.HorizontalScrollbar = true;
            this.listBoxFile1.ItemHeight = 12;
            this.listBoxFile1.Location = new System.Drawing.Point(0, 0);
            this.listBoxFile1.Name = "listBoxFile1";
            this.listBoxFile1.Size = new System.Drawing.Size(784, 90);
            this.listBoxFile1.TabIndex = 0;
            this.listBoxFile1.Click += new System.EventHandler(this.listBoxFile1_Click);
            // 
            // contextMenuStripFile1
            // 
            this.contextMenuStripFile1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.削除DToolStripMenuItem});
            this.contextMenuStripFile1.Name = "contextMenuStripFile1";
            this.contextMenuStripFile1.Size = new System.Drawing.Size(115, 26);
            // 
            // 削除DToolStripMenuItem
            // 
            this.削除DToolStripMenuItem.Name = "削除DToolStripMenuItem";
            this.削除DToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.削除DToolStripMenuItem.Text = "削除(&D)";
            this.削除DToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItemFile1_Click);
            // 
            // listBoxFile2
            // 
            this.listBoxFile2.ContextMenuStrip = this.contextMenuStripFile2;
            this.listBoxFile2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxFile2.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBoxFile2.FormattingEnabled = true;
            this.listBoxFile2.HorizontalScrollbar = true;
            this.listBoxFile2.ItemHeight = 12;
            this.listBoxFile2.Location = new System.Drawing.Point(0, 0);
            this.listBoxFile2.Name = "listBoxFile2";
            this.listBoxFile2.Size = new System.Drawing.Size(784, 86);
            this.listBoxFile2.TabIndex = 1;
            this.listBoxFile2.Click += new System.EventHandler(this.listBoxFile2_Click);
            // 
            // contextMenuStripFile2
            // 
            this.contextMenuStripFile2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.削除DToolStripMenuItem1});
            this.contextMenuStripFile2.Name = "contextMenuStripFile2";
            this.contextMenuStripFile2.Size = new System.Drawing.Size(115, 26);
            // 
            // 削除DToolStripMenuItem1
            // 
            this.削除DToolStripMenuItem1.Name = "削除DToolStripMenuItem1";
            this.削除DToolStripMenuItem1.Size = new System.Drawing.Size(114, 22);
            this.削除DToolStripMenuItem1.Text = "削除(&D)";
            this.削除DToolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItemFile2_Click);
            // 
            // splitContainerLogText
            // 
            this.splitContainerLogText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerLogText.Location = new System.Drawing.Point(0, 0);
            this.splitContainerLogText.Name = "splitContainerLogText";
            this.splitContainerLogText.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerLogText.Panel1
            // 
            this.splitContainerLogText.Panel1.Controls.Add(this.listBoxLog);
            // 
            // splitContainerLogText.Panel2
            // 
            this.splitContainerLogText.Panel2.Controls.Add(this.textBoxData);
            this.splitContainerLogText.Size = new System.Drawing.Size(784, 186);
            this.splitContainerLogText.SplitterDistance = 88;
            this.splitContainerLogText.TabIndex = 0;
            // 
            // listBoxLog
            // 
            this.listBoxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBoxLog.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.listBoxLog.FormattingEnabled = true;
            this.listBoxLog.HorizontalScrollbar = true;
            this.listBoxLog.ItemHeight = 12;
            this.listBoxLog.Location = new System.Drawing.Point(0, 0);
            this.listBoxLog.Name = "listBoxLog";
            this.listBoxLog.Size = new System.Drawing.Size(784, 88);
            this.listBoxLog.TabIndex = 2;
            // 
            // textBoxData
            // 
            this.textBoxData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxData.Font = new System.Drawing.Font("ＭＳ ゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBoxData.Location = new System.Drawing.Point(0, 0);
            this.textBoxData.Multiline = true;
            this.textBoxData.Name = "textBoxData";
            this.textBoxData.ReadOnly = true;
            this.textBoxData.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxData.Size = new System.Drawing.Size(784, 94);
            this.textBoxData.TabIndex = 0;
            this.textBoxData.WordWrap = false;
            // 
            // toolStripComboBoxPass
            // 
            this.toolStripComboBoxPass.Name = "toolStripComboBoxPass";
            this.toolStripComboBoxPass.Size = new System.Drawing.Size(121, 25);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 441);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.splitContainerFile.Panel1.ResumeLayout(false);
            this.splitContainerFile.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerFile)).EndInit();
            this.splitContainerFile.ResumeLayout(false);
            this.contextMenuStripFile1.ResumeLayout(false);
            this.contextMenuStripFile2.ResumeLayout(false);
            this.splitContainerLogText.Panel1.ResumeLayout(false);
            this.splitContainerLogText.Panel2.ResumeLayout(false);
            this.splitContainerLogText.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerLogText)).EndInit();
            this.splitContainerLogText.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAbout;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxType;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxFile;
        private System.Windows.Forms.ToolStripButton toolStripButtonFile1;
        private System.Windows.Forms.ToolStripButton toolStripButtonFile2;
        private System.Windows.Forms.ToolStripButton toolStripButtonGen;
        private System.Windows.Forms.ToolStripButton toolStripButtonSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxApp;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.SplitContainer splitContainerFile;
        private System.Windows.Forms.SplitContainer splitContainerLogText;
        private System.Windows.Forms.ListBox listBoxFile1;
        private System.Windows.Forms.ListBox listBoxFile2;
        private System.Windows.Forms.ListBox listBoxLog;
        private System.Windows.Forms.TextBox textBoxData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemInitDir;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemExePath;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDesktop;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFile1;
        private System.Windows.Forms.ToolStripMenuItem 削除DToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFile2;
        private System.Windows.Forms.ToolStripMenuItem 削除DToolStripMenuItem1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxPass;
    }
}

