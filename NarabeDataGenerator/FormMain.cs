using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NarabeDataGenerator
{
    public partial class FormMain : Form
    {
        //iniファイルアクセス
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL")]
        public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);
        [System.Runtime.InteropServices.DllImport("KERNEL32.DLL")]
        public static extern uint WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);


        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Text = System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductName;
            //iniファイル読込み
            string dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string app = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            string ini = dir + "\\" + app + ".ini";

            StringBuilder fLeft = new StringBuilder(1024);
            GetPrivateProfileString("form", "left", 0.ToString(), fLeft, (uint)fLeft.Capacity, ini);
            StringBuilder fTop = new StringBuilder(1024);
            GetPrivateProfileString("form", "top", 0.ToString(), fTop, (uint)fTop.Capacity, ini);
            StringBuilder fWidth = new StringBuilder(1024);
            GetPrivateProfileString("form", "width", 800.ToString(), fWidth, (uint)fWidth.Capacity, ini);
            StringBuilder fHeight = new StringBuilder(1024);
            GetPrivateProfileString("form", "height", 480.ToString(), fHeight, (uint)fHeight.Capacity, ini);

            StringBuilder iniDirDesktop = new StringBuilder(1024);
            GetPrivateProfileString("form", "iniDirDesktop", false.ToString(), iniDirDesktop, (uint)iniDirDesktop.Capacity, ini);
            ToolStripMenuItemInitDir.Visible = false;
            toolStripMenuItem1.Visible = false;

            StringBuilder sdMain = new StringBuilder(1024);
            GetPrivateProfileString("form", "sdMain", 180.ToString(), sdMain, (uint)sdMain.Capacity, ini);
            StringBuilder sdFile = new StringBuilder(1024);
            GetPrivateProfileString("form", "sdFile", 90.ToString(), sdFile, (uint)sdFile.Capacity, ini);
            StringBuilder sdLogText = new StringBuilder(1024);
            GetPrivateProfileString("form", "sdLogText", 90.ToString(), sdLogText, (uint)sdLogText.Capacity, ini);

            StringBuilder cbApp = new StringBuilder(1024);
            GetPrivateProfileString("form", "cbApp", 0.ToString(), cbApp, (uint)cbApp.Capacity, ini);
            StringBuilder cbType = new StringBuilder(1024);
            GetPrivateProfileString("form", "cbType", 0.ToString(), cbType, (uint)cbType.Capacity, ini);
            StringBuilder cbPass = new StringBuilder(1024);
            GetPrivateProfileString("form", "cbPass", 0.ToString(), cbPass, (uint)cbPass.Capacity, ini);
            StringBuilder cbFile = new StringBuilder(1024);
            GetPrivateProfileString("form", "cbFile", 0.ToString(), cbFile, (uint)cbFile.Capacity, ini);

            Left = int.Parse(fLeft.ToString());
            Top = int.Parse(fTop.ToString());
            Width = int.Parse(fWidth.ToString());
            Height = int.Parse(fHeight.ToString());

            if (!bool.Parse(iniDirDesktop.ToString()))
            {
                ToolStripMenuItemExePath.Checked = true;
                ToolStripMenuItemDesktop.Checked = false;
            }
            else
            {
                ToolStripMenuItemExePath.Checked = false;
                ToolStripMenuItemDesktop.Checked = true;
            }

            splitContainerMain.SplitterDistance = int.Parse(sdMain.ToString());
            splitContainerFile.SplitterDistance = int.Parse(sdFile.ToString());
            splitContainerLogText.SplitterDistance = int.Parse(sdLogText.ToString());

            toolStripComboBoxApp.Items.Add("駅並べ2");
            toolStripComboBoxApp.Items.Add("並べぇ");
            toolStripComboBoxApp.SelectedIndex = int.Parse(cbApp.ToString());

            toolStripComboBoxType.Items.Add("更新スケジュール");
            toolStripComboBoxType.Items.Add("フォルダリスト");
            toolStripComboBoxType.Items.Add("フォルダ情報リスト");
            toolStripComboBoxType.Items.Add("問いリスト");
            toolStripComboBoxType.Items.Add("問いリストBase64");
            toolStripComboBoxType.Items.Add("問いリストAES32");
            toolStripComboBoxType.SelectedIndex = int.Parse(cbType.ToString());

            toolStripComboBoxPass.Items.Add("");
            toolStripComboBoxPass.Items.Add("");
            toolStripComboBoxPass.SelectedIndex = int.Parse(cbPass.ToString());

            toolStripComboBoxFile.Items.Add("updateSchedule.json");
            toolStripComboBoxFile.Items.Add("updateScheduleTest.json");
            toolStripComboBoxFile.Items.Add("folder.json");
            toolStripComboBoxFile.Items.Add("folderInfo.json");
            toolStripComboBoxFile.Items.Add("question.json");
            toolStripComboBoxFile.SelectedIndex = int.Parse(cbFile.ToString());



        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //iniファイル書込み
            string dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string app = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            string ini = dir + "\\" + app + ".ini";

            WritePrivateProfileString("form", "left", Left.ToString(), ini);
            WritePrivateProfileString("form", "top", Top.ToString(), ini);
            WritePrivateProfileString("form", "width", Width.ToString(), ini);
            WritePrivateProfileString("form", "height", Height.ToString(), ini);

            WritePrivateProfileString("form", "iniDirDesktop", ToolStripMenuItemDesktop.Checked.ToString(), ini);

            WritePrivateProfileString("form", "sdMain", splitContainerMain.SplitterDistance.ToString(), ini);
            WritePrivateProfileString("form", "sdFile", splitContainerFile.SplitterDistance.ToString(), ini);
            WritePrivateProfileString("form", "sdLogText", splitContainerLogText.SplitterDistance.ToString(), ini);

            WritePrivateProfileString("form", "cbApp", toolStripComboBoxApp.SelectedIndex.ToString(), ini);
            WritePrivateProfileString("form", "cbType", toolStripComboBoxType.SelectedIndex.ToString(), ini);
            WritePrivateProfileString("form", "cbPass", toolStripComboBoxPass.SelectedIndex.ToString(), ini);
            WritePrivateProfileString("form", "cbFile", toolStripComboBoxFile.SelectedIndex.ToString(), ini);




        }

        private void ToolStripMenuItemExePath_Click(object sender, EventArgs e)
        {
            ToolStripMenuItemExePath.Checked = !ToolStripMenuItemExePath.Checked;
            ToolStripMenuItemDesktop.Checked = !ToolStripMenuItemExePath.Checked;
        }

        private void ToolStripMenuItemDesktop_Click(object sender, EventArgs e)
        {
            ToolStripMenuItemDesktop.Checked = !ToolStripMenuItemDesktop.Checked;
            ToolStripMenuItemExePath.Checked = !ToolStripMenuItemDesktop.Checked;
        }

        private void ToolStripMenuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ToolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            var productName = fileVersionInfo.ProductName;
            var ver = fileVersionInfo.ProductVersion;
            var copyright = fileVersionInfo.LegalCopyright;
            MessageBox.Show(productName + "\nver." + ver + "\n" + copyright, "バージョン情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void toolStripButtonFile1_Click(object sender, EventArgs e)
        {
            //iniファイル
            string dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string app = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            string ini = dir + "\\" + app + ".ini";
            StringBuilder idOpen = new StringBuilder(1024);
            GetPrivateProfileString("form", "idOpen", System.IO.Path.GetDirectoryName(Application.ExecutablePath), idOpen, (uint)idOpen.Capacity, ini);

            //ファイル1
            OpenFileDialog ofd = new OpenFileDialog();
            //if (ToolStripMenuItemExePath.Checked)
            //    ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            //else
            //    ofd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ofd.InitialDirectory = idOpen.ToString();

            ofd.Multiselect = true;
            ofd.Filter = "csvファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.Title = "ファイルを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //listBoxFile1.Items.Add(ofd.FileName);
                foreach (var item in ofd.FileNames)
                    listBoxFile1.Items.Add(item);

                WritePrivateProfileString("form", "idOpen", System.IO.Path.GetDirectoryName(ofd.FileName), ini);
            }
        }

        private void toolStripButtonFile2_Click(object sender, EventArgs e)
        {
            //iniファイル
            string dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string app = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            string ini = dir + "\\" + app + ".ini";
            StringBuilder idOpen = new StringBuilder(1024);
            GetPrivateProfileString("form", "idOpen", System.IO.Path.GetDirectoryName(Application.ExecutablePath), idOpen, (uint)idOpen.Capacity, ini);

            //ファイル2
            OpenFileDialog ofd = new OpenFileDialog();
            //if (ToolStripMenuItemExePath.Checked)
            //    ofd.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            //else
            //    ofd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            ofd.InitialDirectory = idOpen.ToString();

            ofd.Multiselect = true;
            ofd.Filter = "csvファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.Title = "ファイルを選択してください";
            ofd.RestoreDirectory = true;
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //listBoxFile2.Items.Add(ofd.FileName);
                foreach (var item in ofd.FileNames)
                    listBoxFile2.Items.Add(item);

                WritePrivateProfileString("form", "idOpen", System.IO.Path.GetDirectoryName(ofd.FileName), ini);
            }
        }

        private void toolStripButtonGen_Click(object sender, EventArgs e)
        {
            //jsonデータ作成
            List<string> file1 = new List<string>();
            foreach (var item in listBoxFile1.Items)
                file1.Add(item.ToString());
            List<string> file2 = new List<string>();
            foreach (var item in listBoxFile2.Items)
                file2.Add(item.ToString());

            Tools.MakeJson mj = new Tools.MakeJson();

            listBoxLog.Items.Clear();
            mj.Message += new Tools.MakeJson.MessgeEventHandler(MakeJson_Message);

            string aesPass = toolStripComboBoxPass.Text;

            switch (toolStripComboBoxApp.SelectedIndex)
            {
                case 0: //駅並べ2
                    switch (toolStripComboBoxType.SelectedIndex)
                    {
                        case 0: //更新スケジュール
                            textBoxData.Text = mj.UpdateSchedule(toolStripComboBoxFile.Text, file1);
                            break;
                        case 1: //フォルダリスト
                            textBoxData.Text = mj.Eki2Folder(toolStripComboBoxFile.Text, file1);
                            break;
                        case 2: //フォルダ情報リスト
                            textBoxData.Text = mj.Eki2FolderInfo(toolStripComboBoxFile.Text, file1);
                            break;
                        case 3: //問いリスト
                            textBoxData.Text = mj.Eki2Question(toolStripComboBoxFile.Text, file1, file2, false, false, aesPass);
                            break;
                        case 4: //問いリストBase64
                            textBoxData.Text = mj.Eki2Question(toolStripComboBoxFile.Text, file1, file2, true, false, aesPass);
                            break;
                        case 5: //問いリストAES32
                            textBoxData.Text = mj.Eki2Question(toolStripComboBoxFile.Text, file1, file2, true, true, aesPass);
                            break;
                    }
                    break;
                case 1: //並べぇ
                    switch (toolStripComboBoxType.SelectedIndex)
                    {
                        case 0: //更新スケジュール
                            textBoxData.Text = mj.UpdateSchedule(toolStripComboBoxFile.Text, file1);
                            break;
                        case 1: //フォルダリスト
                            textBoxData.Text = mj.NarabeFolder(toolStripComboBoxFile.Text, file1);
                            break;
                        case 2: //フォルダ情報リスト
                            textBoxData.Text = mj.NarabeFolderInfo(toolStripComboBoxFile.Text, file1);
                            break;
                        case 3: //問いリスト
                            textBoxData.Text = mj.NarabeQuestion(toolStripComboBoxFile.Text, file1, file2, false, false, aesPass);
                            break;
                        case 4: //問いリストBase64
                            textBoxData.Text = mj.NarabeQuestion(toolStripComboBoxFile.Text, file1, file2, true, false, aesPass);
                            break;
                        case 5: //問いリストAES32
                            textBoxData.Text = mj.NarabeQuestion(toolStripComboBoxFile.Text, file1, file2, true, true, aesPass);
                            break;
                    }
                    break;
            }
        }

        private void MakeJson_Message(object sender, Tools.MessageEventArgs e)
        {
            //データセット作成ログ出力
            listBoxLog.Items.Add(e.Message);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {

            //iniファイル
            string dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            string app = System.IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            string ini = dir + "\\" + app + ".ini";
            StringBuilder idSave = new StringBuilder(1024);
            GetPrivateProfileString("form", "idSave", System.IO.Path.GetDirectoryName(Application.ExecutablePath), idSave, (uint)idSave.Capacity, ini);

            //保存
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = toolStripComboBoxFile.Text;
            //if (ToolStripMenuItemExePath.Checked)
            //    sfd.InitialDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            //else
            //    sfd.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            sfd.InitialDirectory = idSave.ToString();

            sfd.Filter = "jsonファイル(*.json)|*.json|すべてのファイル(*.*)|*.*";
            sfd.FilterIndex = 2;
            sfd.Title = "保存先のファイルを選択してください";
            sfd.RestoreDirectory = true;
            sfd.OverwritePrompt = true;
            sfd.CheckPathExists = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.Stream stream;
                stream = sfd.OpenFile();
                if (stream != null)
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(stream);
                    sw.Write(textBoxData.Text);
                    sw.Close();
                    stream.Close();
                }

                WritePrivateProfileString("form", "idSave", System.IO.Path.GetDirectoryName(sfd.FileName), ini);
            }
        }

        private void ToolStripMenuItemFile1_Click(object sender, EventArgs e)
        {
            //リストから削除
            if (listBoxFile1.SelectedIndex >= 0)
                listBoxFile1.Items.RemoveAt(listBoxFile1.SelectedIndex);
        }

        private void ToolStripMenuItemFile2_Click(object sender, EventArgs e)
        {
            //リストから削除
            if (listBoxFile2.SelectedIndex >= 0)
                listBoxFile2.Items.RemoveAt(listBoxFile2.SelectedIndex);
        }

        private void listBoxFile1_Click(object sender, EventArgs e)
        {
            //路線選択or路線リスト選択
            if (0 <= listBoxFile1.SelectedIndex)
            {
                //選択されたファイルのデータを表示
                string foo = string.Empty;
                StreamReader sr = new StreamReader(listBoxFile1.Items[listBoxFile1.SelectedIndex].ToString());
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    foo += line + "\r\n";
                }
                textBoxData.Text = foo;
            }
        }

        private void listBoxFile2_Click(object sender, EventArgs e)
        {
            //路線選択or路線リスト選択
            if (0 <= listBoxFile2.SelectedIndex)
            {
                //選択されたファイルのデータを表示
                string foo = string.Empty;
                StreamReader sr = new StreamReader(listBoxFile2.Items[listBoxFile2.SelectedIndex].ToString());
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    foo += line + "\r\n";
                }
                textBoxData.Text = foo;
            }
        }



    }
}
