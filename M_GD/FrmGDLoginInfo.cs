using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using C_Controls.LabelTextBox;
using C_Global;
using C_Event;
using Language;

namespace M_GD
{
    [C_Global.CModuleAttribute("�û���¼��Ϣ", "FrmGDLoginInfo", "�û���¼��Ϣ", "GD Group")]
    public partial class FrmGDLoginInfo : Form
    {
        #region ����

        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;

        private int iPageCount = 0;//��ҳҳ��

        int userID = 0;
        string serverIP = null;
        string userName = null;
        string userNick = null;
        int selectChar = 0;   //GrdCharacter�е�ǰѡ�е��� 

        bool pageLoginInfo = false;

        #endregion

        public FrmGDLoginInfo()
        {
            InitializeComponent();
        }

        #region ��������еĴ���
        /// <summary>
        /// ��������еĴ���
        /// </summary>
        /// <param name="oParent">MDI ����ĸ�����</param>
        /// <param name="oSocket">Socket</param>
        /// <returns>����еĴ���</returns>
        public Form CreateModule(object oParent, object oEvent)
        {
            //������¼����
            FrmGDLoginInfo mModuleFrm = new FrmGDLoginInfo();
            mModuleFrm.m_ClientEvent = (CSocketEvent)oEvent;
            if (oParent != null)
            {
                mModuleFrm.MdiParent = (Form)oParent;
                mModuleFrm.Show();
            }
            else
            {
                mModuleFrm.ShowDialog();
            }

            return mModuleFrm;
        }
        #endregion

        #region ��ʼ�����Կ�
        /// <summary>
        ///�����ֿ�
        /// </summary>
        ConfigValue config = null;

        /// <summary>
        /// ��ʼ�����������Կ�
        /// </summary>
        private void IntiFontLib()
        {
            config = (ConfigValue)m_ClientEvent.GetInfo("INI");
            IntiUI();
        }

        private void IntiUI()
        {
            //this.GrpSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_GrpSearch");
            //this.lblServer.Text = config.ReadConfigValue("MSD", "UIC_UI_lblServer");
            //this.lblAccount.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            //this.lblNick.Text = config.ReadConfigValue("MSD", "UIC_UI_lblNick");
            //this.btnSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_btnSearch");
            //this.btnClose.Text = config.ReadConfigValue("MSD", "UIC_UI_btnClose");

            //this.tpgCharacter.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgCharacter");

            //this.tpgLoginInfo.Text = config.ReadConfigValue("MSD", "LI_UI_tpgLoginInfo");
            //this.lblLoginInfo.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            tbcResult.Enabled = false;
        }
        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDLoginInfo_Load(object sender, EventArgs e)
        {
            try
            {
                IntiFontLib();

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[2];
                mContent[0].eName = CEnum.TagName.ServerInfo_GameDBID;
                mContent[0].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[0].oContent = 1;

                mContent[1].eName = CEnum.TagName.ServerInfo_GameID;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = m_ClientEvent.GetInfo("GameID_SD");

                this.backgroundWorkerFormLoad.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ������Ϸ�������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                mServerInfo = Operation_GD.GetServerList(this.m_ClientEvent, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ������Ϸ�������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbServer = Operation_GD.BuildCombox(mServerInfo, cmbServer);
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion



        #region �л���ͬ����Ϸ������
        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion



        #region ��ѯ���������Ϣ
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                tbcResult.SelectedTab = tpgCharacter;//ѡ���ɫ��Ϣѡ�
                this.GrdCharacter.DataSource = null;

                if (cmbServer.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "UIA_Hint_selectServer"));
                    return;
                }

                serverIP = Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text);
                userName = txtAccount.Text.Trim();
                userNick = txtNick.Text.Trim();

                if (txtAccount.Text.Trim().Length > 0 || txtNick.Text.Trim().Length > 0)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[3];
                    //��ѯ��ҽ�ɫ��Ϣ
                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.SD_UserName;
                    mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[1].oContent = userName;

                    mContent[2].eName = CEnum.TagName.f_pilot;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userNick;

                    backgroundWorkerSearch.RunWorkerAsync(mContent);
                }
                else
                {
                    MessageBox.Show(config.ReadConfigValue("MMagic", "UIA_Hint_inPutAccont"));
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ��ɫ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Account_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��ɫ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdCharacter, out iPageCount);
        }
        #endregion



        #region ������һ�����Ϣ���浱ǰ�к�
        private void GrdCharacter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//����ĳһ��
                {
                    selectChar = int.Parse(e.RowIndex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ˫����һ�����Ϣ�鿴���������Ϣ
        private void GrdCharacter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectChar = int.Parse(e.RowIndex.ToString());//������
                    if (GrdCharacter.DataSource != null)
                    {
                        tbcResult.SelectedTab = tpgLoginInfo;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region �л�ѡ����в���
        private void tbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (GrdCharacter.DataSource != null)
                {
                    DataTable mTable = (DataTable)GrdCharacter.DataSource;
                    userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                    userName = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "SD_UserName")].ToString();
                    if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("MSD", "LI_UI_tpgLoginInfo")))
                    {
                        LoginInfo();
                    }
                }
                else
                {
                    this.GrdLoginInfo.DataSource = null;
                    this.pnlLoginInfo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion



        #region ��ҵ�¼��Ϣ
        private void LoginInfo()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdLoginInfo.DataSource = null;

                this.pnlLoginInfo.Visible = false;
                this.cmbLoginInfo.Items.Clear();
                this.pageLoginInfo = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.f_idx;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_GD.iPageSize;
                //mContent[4].oContent =1;

                mContent[5].eName = CEnum.TagName.SD_StartTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[5].oContent = DtpBegin.Value;

                mContent[6].eName = CEnum.TagName.SD_EndTime;
                mContent[6].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[6].oContent = DtpEnd.Value;

                backgroundWorkerLogin.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        
        #region ��ҵ�¼��ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UserLoginfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҵ�¼��ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdLoginInfo, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlLoginInfo.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbLoginInfo.Items.Add(i + 1);
                }

                cmbLoginInfo.SelectedIndex = 0;
                pageLoginInfo = true;
                pnlLoginInfo.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ��ҵ�¼��Ϣ
        private void cmbLoginInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageLoginInfo)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdLoginInfo.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.f_idx;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = userID;

                    mContent[2].eName = CEnum.TagName.SD_UserName;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userName;

                    mContent[3].eName = CEnum.TagName.Index;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = int.Parse(cmbLoginInfo.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;
                    //mContent[4].oContent = 1;

                    mContent[5].eName = CEnum.TagName.SD_StartTime;
                    mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[5].oContent = DtpBegin.Value;

                    mContent[6].eName = CEnum.TagName.SD_EndTime;
                    mContent[6].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[6].oContent = DtpEnd.Value;

                    backgroundWorkerReLogin.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ��ҵ�¼��ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UserLoginfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ��ҵ�¼��ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdLoginInfo, out iPageCount);
        }
        #endregion



        #region ����ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void DtpBegin_Leave(object sender, EventArgs e)
        {
            TimeSpan ts = new TimeSpan();
            ts = (this.DtpBegin.Value.Date - System.DateTime.Now.Date);
            if (System.Math.Abs(ts.TotalDays) > 45)
            {
                MessageBox.Show("�����¼����ʱ�䲻����45�죡");
                this.DtpBegin.Text = System.DateTime.Now.Date.ToString();
            }
        }
    }
}