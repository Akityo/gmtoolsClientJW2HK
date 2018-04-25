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
namespace M_GD
{
    [C_Global.CModuleAttribute("ϵͳ�ʼ���ѯ", "FrmGDSystemMail", "ϵͳ�ʼ���ѯ", "GD Group")]
    public partial class FrmGDSystemMail : Form
    {

        #region ����

        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;
        private int iPageCount = 0;//��ҳҳ��
        string userIDKey = null;
        string serverIP = null;
        int currCharSelRow = 0;
        int currDgSelectRow = 0;    //GrdCharacter�е�ǰѡ�е���
        int petID = 0;//����ID

        bool pageSystemMail = false;
     
        int selectChar = 0;   //GrdCharacter�е�ǰѡ�е��� 

        int userID = 0;
        string userName = null;
        string userNick = null;
        int type = 3;

        #endregion

        public FrmGDSystemMail()
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
            FrmGDSystemMail mModuleFrm = new FrmGDSystemMail();
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
            //this.GrpSearch.Text = config.ReadConfigValue("MMagic", "UIC_UI_GrpSearch");
        }


        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDSystemMail_Load(object sender, EventArgs e)
        {
            try
            {
                IntiFontLib();
                TbcResult.Enabled = false;
                this.label1.Text = "�������ʺ�";
                this.label3.Text = "�������ǳ�";
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

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrdCharacter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
             try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectChar = int.Parse(e.RowIndex.ToString());//������
                    if (GrdCharacter.DataSource != null)
                    {
                        this.TbcResult.SelectedTab = this.tpgSystemMail;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }

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

        private void TbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (GrdCharacter.DataSource != null)
                {
                    DataTable mTable = (DataTable)GrdCharacter.DataSource;
                    userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                    userName = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "SD_UserName")].ToString();
                    if (this.TbcResult.SelectedTab.Text.Equals("ϵͳ�ʼ�"))
                    {
                        SystemMailInfo();
                    }
                }
                else
                {
                    this.GrdSystemMail.DataSource = null;
                    this.pnlSystemMail.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    
        private void SystemMailInfo()
        {

            try
            {
                this.GrpSearch.Enabled = false;
                this.TbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdSystemMail.DataSource = null;

                this.pnlSystemMail.Visible = false;
                this.cmbSystemMail.Items.Clear();
                this.pageSystemMail = false;


                if (this.DtpBegin.Value >= DateTime.Now)
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "GI_Hint_StartTime"));
                    this.GrpSearch.Enabled = true;
                    this.TbcResult.Enabled = true;
                    return;
                }

                if (this.DtpEnd.Value <= this.DtpBegin.Value)
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "GI_Hint_TimeSpan"));
                    this.GrpSearch.Enabled = true;
                    this.TbcResult.Enabled = true;
                    return;
                }


                CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];

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
                mContent[5].oContent = this.DtpBegin.Value;

                mContent[6].eName = CEnum.TagName.SD_EndTime;
                mContent[6].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[6].oContent = this.DtpEnd.Value;

                mContent[7].eName = CEnum.TagName.SD_Type;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = type;

                backgroundWorkerSystemMail.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void chbRecvOrSend_CheckedChanged(object sender, EventArgs e)
        {
            if (chbRecvOrSend.Checked)
            {
                this.chbRecvOrSend.Text = config.ReadConfigValue("MSD", "GI_UI_chbRecv");
                label1.Text = "�������ʺ�";
                label3.Text = "�������ǳ�";
             
                type = 4;
            }
            else
            {
                this.chbRecvOrSend.Text = config.ReadConfigValue("MSD", "GI_UI_chbSend");
                label1.Text = "�������ʺ�";
                label3.Text = "�������ǳ�";
                
                type = 3;
            }
        }

        private void backgroundWorkerSystemMail_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_UserGrift_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerSystemMail_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.TbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, this.GrdSystemMail, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlSystemMail.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbSystemMail.Items.Add(i + 1);
                }

                this.cmbSystemMail.SelectedIndex = 0;
                this.pageSystemMail = true;
                this.pnlSystemMail.Visible = true;
            }
        }

        private void backgroundWorkerReSystemMail_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UserGrift_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerReSystemMail_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.TbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, this.GrdSystemMail, out iPageCount);

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.TbcResult.SelectedTab = this.TpgCharacter;//ѡ���ɫ��Ϣѡ�
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
                    this.TbcResult.Enabled = false;
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

        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Account_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.TbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdCharacter, out iPageCount);
        }

        private void cmbSystemMail_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageSystemMail)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdSystemMail.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];

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
                    mContent[3].oContent = int.Parse(this.cmbSystemMail.Text.ToString());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;
                    //mContent[4].oContent = 1;

                    mContent[5].eName = CEnum.TagName.SD_StartTime;
                    mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[5].oContent = this.DtpBegin.Value;

                    mContent[6].eName = CEnum.TagName.SD_EndTime;
                    mContent[6].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[6].oContent = this.DtpEnd.Value;

                    mContent[7].eName = CEnum.TagName.SD_Type;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = type;

                    this.backgroundWorkerReSystemMail.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    

    
       
    }
}