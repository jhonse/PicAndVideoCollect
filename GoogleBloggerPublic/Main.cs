using GoogleBloggerPublic.Lib;
using System;
using System.Windows.Forms;

namespace GoogleBloggerPublic
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            btnInit.Enabled = true;
            btnCollectStop.Enabled = false;
            JOs.init(lbLogList,lbSendLogList,btnVerity,btnCompant,btnCollectStart,btnCollectStop,btnPublicStart,btnPublicStop,btnInit);
        }

        private void Main_Leave(object sender, EventArgs e)
        {
            JOs.uregisterjHotKey(Handle);
        }

        private void Main_Activated(object sender, EventArgs e)
        {
            JOs.registerjHotKey(Handle);
        }

        protected override void WndProc(ref Message m)
        {

            const int WM_HOTKEY = 0x0312;
            //按快捷键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 100:    //按下的是Ctrl+S
                            this.Visible = true;
                            break;
                        case 101:    //按下的是Ctrl+B
                            this.Visible = false;
                            break;
                        case 102:    //按下的是Ctrl+D
                            this.Close();
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void btnInit_Click(object sender, EventArgs e)
        {
            JOs.first(tbsenderServerIp.Text, tbtoMailAddress.Text, tbfromMailAddress.Text, tbmailUsername.Text, tbmailPassword.Text, tbmailPort.Text,
                tbPageList.Text,tbPageNum.Text, tbPublicTimes.Text, tbPicPath.Text, tbApiKey.Text, tbSharedSecret.Text, tbGroupNum.Text, 
                tbPublicNextTime.Text,tbPublicNum.Text,cbType.Text,tbtran_appid.Text,tbTran_key.Text,tbTran_from.Text,tbTran_to.Text,
                cbTran_open.Checked,tbPicPositionStartX.Text,tbPicPositionStartY.Text,tbPicPositionEndX.Text,tbPicPositionEndY.Text,
                tbPicWaterText.Text,cbPicWaterPosition.Text,cbPicCutOpen.Checked,cbPicWaterOpen.Checked,cbPicBackup.Checked,cbTimerOpen.Checked,
                dtpTimerStart.Value,dtpTimerStop.Value, cbTimerLog.Checked, tbPublicPassword.Text, cbMailSendType.Text, tbMailSendUrl.Text,cbPublicType.Text,
                tbXmlRpcUrl.Text,tbXmlRpcUsername.Text,tbXmlRpcPassword.Text,cbPublicPicType.Text,tbXmlPRCCat.Text,tbPublicSendTime.Text,tbPageEndNum.Text);
        }

        private void btnChonse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择目录";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                tbPicPath.Text = foldPath;
            }
        }

        private void btnVerity_Click(object sender, EventArgs e)
        {
            JOs.verity();
        }

        private void btnCompant_Click(object sender, EventArgs e)
        {
            JOs.compant(tbOAuthCode.Text);
        }

        private void btnJiemianShow_Click(object sender, EventArgs e)
        {
            if (JPassword.GoogleBloggerPublic.getShow(tbJiemainPassword.Text))
            {
                if(tcgb.TabPages.Count == 1)
                {
                    btnJiemianShow.Enabled = false;
                    btnJiemianHide.Enabled = true;
                    tcgb.TabPages.Add(tpLog);
                    tcgb.TabPages.Add(tpMail);
                    tcgb.TabPages.Add(tpXmlRpc);
                    tcgb.TabPages.Add(tpCollect);
                    tcgb.TabPages.Add(tpPic);
                    tcgb.TabPages.Add(tpTrans);
                    tcgb.TabPages.Add(tpTimer);
                    tcgb.TabPages.Add(tpPublic);
                    tcgb.SelectedIndex = 0;
                }
            }
            else
            {
                MessageBox.Show("密码错误!");
            }
        }

        private void btnJiemianHide_Click(object sender, EventArgs e)
        {
            if (JPassword.GoogleBloggerPublic.getShow(tbJiemainPassword.Text))
            {
                if(tcgb.TabPages.Count == 9)
                {
                    btnJiemianShow.Enabled = true;
                    btnJiemianHide.Enabled = false;
                    tcgb.TabPages.Remove(tpLog);
                    tcgb.TabPages.Remove(tpMail);
                    tcgb.TabPages.Remove(tpXmlRpc);
                    tcgb.TabPages.Remove(tpCollect);
                    tcgb.TabPages.Remove(tpPic);
                    tcgb.TabPages.Remove(tpTrans);
                    tcgb.TabPages.Remove(tpTimer);
                    tcgb.TabPages.Remove(tpPublic);
                }
            }
            else
            {
                MessageBox.Show("密码错误!");
            }
        }

        private void cbMailSendType_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cbMailSendType.Text != "服务器转发")
            {
                tbMailSendUrl.ReadOnly = true;
            }
            else
            {
                tbMailSendUrl.ReadOnly = false;
            }
        }

        private void btnCollectStart_Click(object sender, EventArgs e)
        {
            JOs.collectStart();
        }

        private void btnPublicStart_Click(object sender, EventArgs e)
        {
            JOs.publicStart();
        }

        private void btnCollectStop_Click(object sender, EventArgs e)
        {
            JOs.collectStop();
        }

        private void btnPublicStop_Click(object sender, EventArgs e)
        {
            JOs.publicStop();
        }
    }
}
