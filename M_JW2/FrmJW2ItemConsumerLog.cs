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
    [C_Global.CModuleAttribute("����������־", "FrmJW2ItemConsumerLog", "����������־", "JW2 Group")]
    public partial class FrmJW2ItemConsumerLog : Form
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
        bool pageFamilyBuyLog = false;
        string FamilyID;
        int currDgSelectRow;

        #endregion

        public FrmJW2ItemConsumerLog()
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
            FrmJW2ItemConsumerLog mModuleFrm = new FrmJW2ItemConsumerLog();
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

            this.Text = config.ReadConfigValue("MJW2", "NEW_UI_FrmJW2FamilyBuyLog");

            this.GrpSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_GrpSearch");
            this.lblServer.Text = config.ReadConfigValue("MSD", "UIC_UI_lblServer");
            this.lblAccount.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyName");

            this.btnSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_btnSearch");
            this.btnClose.Text = config.ReadConfigValue("MSD", "UIC_UI_btnClose");


            this.Text = config.ReadConfigValue("MJW2", "NEW_UI_FrmJW2FamilyBuyLog");
            LblDate.Text = config.ReadConfigValue("MJW2", "NEWNEW_UI_BeginTime");
            LblLink.Text = config.ReadConfigValue("MJW2", "NEW_UI_EndTime");

            lblRoleView.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");
            label1.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");



            label2.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");

            tpgConsumerItemLog.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");
            tbcResult.Enabled = true;
        }
        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmJW2ItemConsumerLog_Load(object sender, EventArgs e)
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

        #region
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

                    if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_tpgConsumerItemLog")))
                    {

                    }
                    else if (this.tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_tpgItemComboLog")))
                    {


                    }


                }
                else
                {

                    GrdRoleView.DataSource = null;
                    GrdRoleView.Visible = false;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


        #region
        private void GrdRoleView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.tbcResult.SelectedIndex = 1;
        }
        #endregion
    }
}