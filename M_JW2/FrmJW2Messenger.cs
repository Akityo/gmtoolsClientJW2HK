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
    [C_Global.CModuleAttribute("Message�ƺ�", "FrmJW2Messenger", "Message�ƺ�", "JW2 Group")]
    public partial class FrmJW2Messenger : Form
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
        int selectItem = 0;

        string itemID = null;
        bool pageRoleView = false;
        bool pageStoryState = false;
        bool pageBodyItem = false;
        bool pageHomeItem = false;
        bool pageBuyPresent = false;
        bool pageConsumerItemUser = false;
        bool pageBugleSendLog = false;
        bool pageUserFamilyInfo = false;
        #endregion

        public FrmJW2Messenger()
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
            FrmJW2Messenger mModuleFrm = new FrmJW2Messenger();
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

            //tpgCharacter.Text = config.ReadConfigValue("MSD", "UIC_UI_btnClose");
            //this.tpgCharacter.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgCharacter");
            //this.lblStoryState.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");
            this.tpgCharacter.Text = config.ReadConfigValue("MJW2", "NEW_UI_FrmJW2LoginInfo");
            this.tpgMessage.Text = config.ReadConfigValue("MJW2", "NEW_UI_tpgMessage");
            lblRoleView.Text=config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");
            lblStoryState.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");
            this.Text = config.ReadConfigValue("MJW2", "NEW_UI_tpgMessage");

            tbcResult.Enabled = true;
        }
        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmJW2Messenger_Load(object sender, EventArgs e)
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

        #region �л�ѡ���ѯ
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
                    if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_tpgMessage")))
                    {
                        MessageNumber();//��ѯ���¾���״̬
                    }
 
                }
               }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯMessage�ƺ�
        private void MessageNumber()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
               
                this.pageStoryState = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

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

                backgroundWorkerReMessage.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region backgroundworker��ѯmessage�ƺŷ�����Ϣ
        private void backgroundWorkerMessage_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_Messenger_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region backgroundworker��ѯmessage�ƺŽ�����Ϣ
        private void backgroundWorkerMessage_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;

                this.Cursor = Cursors.Default;//�ı����״̬

                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdStoryState, out iPageCount);

                if (iPageCount <= 1)
                {
                    this.pnlStoryState.Visible = false;
                }
                else
                {
                    for (int i = 0; i < iPageCount; i++)
                    {
                        this.cmbStorySate.Items.Add(i + 1);
                    }

                    this.cmbStorySate.SelectedIndex = 0;
                    this.pageStoryState = true;
                    this.pnlStoryState.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region backgroundworker��ҳ������Ϣ
        private void backgroundWorkerReMessage_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(tmp_ClientEvent, CEnum.ServiceKey.JW2_Messenger_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region backgroundworker��ҳ������Ϣ
        private void backgroundWorkerReMessage_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;

                this.Cursor = Cursors.Default;//�ı����״̬

                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdStoryState, out iPageCount);

               
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region �û����ϲ�ѯ��ҳ
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
                mContent[4].oContent = int.Parse(cmbRoleView.Text.ToString());

                mContent[5].eName = CEnum.TagName.PageSize;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent = Operation_JW2.iPageSize;



                backgroundWorkerReSearch.RunWorkerAsync(mContent);
            }
        }
        #endregion

        #region Message�ƺŷ�ҳ
        private void cmbStorySate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageStoryState)
            {

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

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
                mContent[3].oContent = int.Parse(this.cmbStorySate.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                backgroundWorkerReMessage.RunWorkerAsync(mContent);
            }
        }
        #endregion

        #region 
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
          
                this.pageStoryState = false;

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

        #region
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_ACCOUNT_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;
                this.tbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//�ı����״̬
                this.tbcResult.SelectedIndex = 0;
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

                    this.cmbRoleView.SelectedIndex = 0;
                    this.pageRoleView = true;
                    this.pnlRoleView.Visible = true;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void backgroundWorkerReSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_ACCOUNT_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerReSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;
                this.tbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//�ı����״̬
                this.tbcResult.SelectedIndex = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdRoleView, out iPageCount);

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void GrdRoleView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.tbcResult.SelectedIndex = 1;
        }
        #endregion
    }
}