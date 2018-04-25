using System;
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


namespace M_JW2
{
    [C_Global.CModuleAttribute("�û���¼��Ϣ", "FrmJW2LoginInfo", "�û���¼��Ϣ", "GD Group")]
    public partial class FrmJW2LoginInfo : Form
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
        bool pageRoleView = false;

        #endregion

        public FrmJW2LoginInfo()
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
            FrmJW2LoginInfo mModuleFrm = new FrmJW2LoginInfo();
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
            this.GrpSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_GrpSearch");
            this.lblServer.Text = config.ReadConfigValue("MSD", "UIC_UI_lblServer");
            this.lblAccount.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            this.lblNick.Text = config.ReadConfigValue("MSD", "UIC_UI_lblNick");
            this.btnSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_btnSearch");
            this.btnClose.Text = config.ReadConfigValue("MSD", "UIC_UI_btnClose");

            label1.Text = config.ReadConfigValue("MJW2", "NEW_UI_IPAddress");

            LblDate.Text = config.ReadConfigValue("MJW2", "GI_UI_lblStartTime");
            LblLink.Text = config.ReadConfigValue("MJW2", "GI_UI_lblEndTime");


            this.tpgLoginInfo.Text=config.ReadConfigValue("MJW2", "NEW_UI_tpgLoginInfo");
            lblRoleView.Text = config.ReadConfigValue("MJW2", "NEW_UI_tpgLoginInfo");
            lblLoginInfo.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");
            lblRoleView.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");

            this.Text = config.ReadConfigValue("MJW2", "NEW_UI_FrmJW2LoginInfo");
            //this.tpgCharacter.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgCharacter");

            //this.tpgLoginInfo.Text = config.ReadConfigValue("MSD", "LI_UI_tpgLoginInfo");
            //this.lblLoginInfo.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            tbcResult.Enabled = false;
        }
        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmJW2LoginInfo_Load(object sender, EventArgs e)
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
                mContent[1].oContent = m_ClientEvent.GetInfo("GameID_JW2");

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
                mServerInfo = Operation_JW2.GetServerList(this.m_ClientEvent, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ������Ϸ�������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            cmbServer = Operation_JW2.BuildCombox(mServerInfo, cmbServer);
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion



        #region �л���ͬ����Ϸ������
        private void cmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text));
        }
        #endregion



        #region ��ѯ���������Ϣ
        private void PartInfo()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                //this.GrdMixItems.DataSource = null;

                //this.pnlMixItems.Visible = false;
                //this.cmbMixItems.Items.Clear();
                this.pageRoleView = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[6];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = cmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_ACCOUNT;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = this.txtAccount.Text.ToString();


                mContent[3].eName = CEnum.TagName.JW2_UserNick;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = txtNick.Text.ToString();

                mContent[4].eName = CEnum.TagName.Index;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = 1;

                mContent[5].eName = CEnum.TagName.PageSize;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent = Operation_JW2.iPageSize;

                backgroundWorkerSearch.RunWorkerAsync(mContent);
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
                e.Result = Operation_JW2.GetResult(tmp_ClientEvent, CEnum.ServiceKey.JW2_ACCOUNT_QUERY, (CEnum.Message_Body[])e.Argument);
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

            Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdRoleView, out iPageCount);
            if (iPageCount <= 1)
            {
                this.pnlRoleView.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbRoleView.Items.Add(i + 1);
                }

                cmbRoleView.SelectedIndex = 0;
                pageRoleView = true;
                pnlRoleView.Visible = true;
            }

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
            this.tbcResult.SelectedIndex = 1;  
        }
        #endregion

        #region �л�ѡ����в���
        private void tbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {

                if (GrdRoleView.DataSource != null)
                {
                    DataTable mTable = (DataTable)GrdRoleView.DataSource;
                    serverIP = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);
                    userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "JW2_UserSN")].ToString());//��������ʺ�ID
                    userName = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "JW2_UserID")].ToString();
                    if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_tpgLoginInfo")))
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
                //this.GrdMixItems.DataSource = null;

                //this.pnlMixItems.Visible = false;
                //this.cmbMixItems.Items.Clear();
                this.pageLoginInfo = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.JW2_UserSN;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.JW2_UserID;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                mContent[5].eName = CEnum.TagName.BeginTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[5].oContent = DtpBegin.Text.ToString();

                mContent[6].eName = CEnum.TagName.EndTime;
                mContent[6].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[6].oContent =DtpEnd.Text.ToString();

                mContent[7].eName = CEnum.TagName.JW2_IP;
                mContent[7].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[7].oContent = "";
                this.backgroundWorkerLogin.RunWorkerAsync(mContent);
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
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_LOGINOUT_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҵ�¼��ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
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

            Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdLoginInfo, out iPageCount);

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

                //cmbLoginInfo.SelectedIndex = 0;
                pageLoginInfo = true;
                pnlLoginInfo.Visible = true;
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.ToString());
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
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                //mContent[1].eName = CEnum.TagName.JW2_UserSN;
                //mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                //mContent[1].oContent = userID;

                mContent[2].eName = CEnum.TagName.JW2_UserID;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = userName;

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = int.Parse(this.cmbLoginInfo.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                mContent[5].eName = CEnum.TagName.BeginTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[5].oContent =DtpBegin.Text.ToString();

                mContent[6].eName = CEnum.TagName.EndTime;
                mContent[6].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[6].oContent = DtpEnd.Text.ToString();


                if (this.textBox1.Text.ToString() != "")
                {
                    mContent[1].eName = CEnum.TagName.JW2_UserSN;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = 0;

                    mContent[7].eName = CEnum.TagName.JW2_IP;
                    mContent[7].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[7].oContent = this.textBox1.Text.ToString();
                }
                else
                {
                    mContent[1].eName = CEnum.TagName.JW2_UserSN;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent =userID;

                    mContent[7].eName = CEnum.TagName.JW2_IP;
                    mContent[7].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[7].oContent = "";
                }

                this.backgroundWorkerReLogin.RunWorkerAsync(mContent);
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
        }
        #endregion

        #region ��ҳ��ѯ��ҵ�¼��ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_LOGINOUT_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ��ҵ�¼��ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
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

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdLoginInfo, out iPageCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion



        #region ����ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.GrdRoleView.DataSource = null;
                this.tbcResult.SelectedIndex = 0;
                if (cmbServer.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MJW2", "UIC_Hint_selectServer"));
                    return;
                }

                serverIP = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);
                userName = txtAccount.Text.Trim();
                userNick = txtNick.Text.Trim();
                if (textBox1.Text.ToString() != "")
                {
                    this.tbcResult.SelectedIndex = 1;
                    this.cmbLoginInfo.Items.Clear();
                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];

                    mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.JW2_UserSN;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = 0;

                    mContent[2].eName = CEnum.TagName.JW2_UserID;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = userName;

                    mContent[3].eName = CEnum.TagName.Index;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = 1;

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_JW2.iPageSize;

                    mContent[5].eName = CEnum.TagName.BeginTime;
                    mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[5].oContent = DtpBegin.Text.ToString();

                    mContent[6].eName = CEnum.TagName.EndTime;
                    mContent[6].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[6].oContent = DtpEnd.Text.ToString();

                    mContent[7].eName = CEnum.TagName.JW2_IP;
                    mContent[7].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[7].oContent = textBox1.Text.ToString();

                    this.backgroundWorkerLogin.RunWorkerAsync(mContent);

                }else if (txtAccount.Text.Trim().Length > 0 || txtNick.Text.Trim().Length > 0)
                {

                    PartInfo();
                }
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbRoleView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageRoleView)
            {


                CEnum.Message_Body[] mContent = new CEnum.Message_Body[6];
                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, cmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = cmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_ACCOUNT;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = this.txtAccount.Text.ToString();


                mContent[3].eName = CEnum.TagName.JW2_UserNick;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = txtNick.Text.ToString();

                mContent[4].eName = CEnum.TagName.Index;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = 1;

                mContent[5].eName = CEnum.TagName.PageSize;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent = Operation_JW2.iPageSize;

                backgroundWorkerReSearch.RunWorkerAsync(mContent);
            }
        }

        private void backgroundWorkerReSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_LOGINOUT_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerReSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
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

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdRoleView, out iPageCount);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

 

      
      

    
    }
}