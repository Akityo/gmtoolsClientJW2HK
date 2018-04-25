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
    [C_Global.CModuleAttribute("����ʺŽ�/��ͣ", "FrmGDBanPlayer", "����ʺŽ�/��ͣ", "GD Group")]
    public partial class FrmGDBanPlayer : Form
    {
        #region ����

        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;

        private int iPageCount = 0;//��ҳҳ��
                
        int userID = 0;
        string userName = null;
        string userNick = null;
        string serverIP = null;
        string serverName = null;

        int selectChar = 0;//GrdResult�е�ǰѡ�е���
        int selectList = 0;

        private bool pageBanList = false;//��ҳ���ظ���Query
                
        #endregion

        public FrmGDBanPlayer()
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
            FrmGDBanPlayer mModuleFrm = new FrmGDBanPlayer();
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

        #region ��ʼ��������Ϣ
        /// <summary>
        ///�����ֿ�
        /// </summary>
        ConfigValue config = null;

        /// <summary>
        /// ��ʼ��������Ϣ
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

            //this.tpgBanPlayer.Text = config.ReadConfigValue("MSD", "BP_UI_tpgBanPlayer");
            //this.lblBanAccount.Text = config.ReadConfigValue("MSD", "BP_UI_lblBanAccount");
            //this.lblBanEndTime.Text = config.ReadConfigValue("MSD", "BP_UI_lblBanEndTime");
            //this.lblBanReason.Text = config.ReadConfigValue("MSD", "BP_UI_lblBanReason");
            //this.btnBanAccount.Text = config.ReadConfigValue("MSD", "BP_UI_btnBanAccount");
            //this.btnResetBan.Text = config.ReadConfigValue("MSD", "BP_UI_btnReset");

            //this.tpgAllBanPlayer.Text = config.ReadConfigValue("MSD", "BP_UI_tpgAllBanPlayer");
            //this.lblListPage.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgUnBind.Text = config.ReadConfigValue("MSD", "BP_UI_tpgUnBind");
            //this.lblUnBindAccount.Text = config.ReadConfigValue("MSD", "BP_UI_lblUnBindAccount");
            //this.lblUnBindReason.Text = config.ReadConfigValue("MSD", "BP_UI_lblUnBindReason");
            //this.btnUnBind.Text = config.ReadConfigValue("MSD", "BP_UI_btnUnBind");
            //this.btnReset.Text = config.ReadConfigValue("MSD", "BP_UI_btnReset");

            //this.tbcResult.Enabled = false;
        }

        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDBanPlayer_Load(object sender, EventArgs e)
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



        #region ͨ����ѯ��ť���в�ѯ
        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.GrdResult.DataSource = null;
                this.GrdCharacter.DataSource = null;
                this.pnlListPage.Visible = false;

                if (cmbServer.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "UIA_Hint_selectServer"));
                    return;
                }

                serverIP = Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text);
                serverName = cmbServer.Text;
                userName = txtAccount.Text.Trim();
                userNick = txtNick.Text.Trim();

                if (txtAccount.Text.Trim().Length > 0 || txtNick.Text.Trim().Length > 0)
                {
                    PartInfo();
                }
                else
                {
                    BanList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ���������Ϣ
        private void PartInfo()
        {
            try
            {
                this.tbcResult.SelectedTab = tpgCharacter;
                this.GrdResult.DataSource = null;

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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdResult, out iPageCount);
        }
        #endregion



        #region ������һ�����Ϣ�����к�
        private void GrdResult_CellClick(object sender, DataGridViewCellEventArgs e)
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
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region ˫����һ�����Ϣ�����������
        private void GrdResult_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectChar = int.Parse(e.RowIndex.ToString());//������
                    if (GrdResult.DataSource != null)
                    {
                        tbcResult.SelectedTab = tpgBanPlayer;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region �л�ѡ����в���
        private void tbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("GLOBAL", "BP_UI_tpgBanPlayer")))
                {
                    if (GrdResult.DataSource != null)
                    {
                        DataTable mTable = (DataTable)GrdResult.DataSource;
                        userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                        userName = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "SD_UserName")].ToString();
                        userNick = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_pilot")].ToString();
                        txtBanAccount.Text = userName;
                        DptBanEndTime.Value = DateTime.Now;
                        txtBanReason.Text = "";
                    }
                    else
                    {
                        txtBanAccount.Text = "";
                        DptBanEndTime.Value = DateTime.Now;
                        txtBanReason.Text = "";
                    }
                }
                else if (tbcResult.SelectedTab.Text.Equals(config.ReadConfigValue("GLOBAL", "BP_UI_tpgUnBind")))
                {
                    if (GrdCharacter.DataSource != null )
                    {
                        
                        DataTable mTable = (DataTable)GrdCharacter.DataSource;
                        userID = int.Parse(mTable.Rows[selectList][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                        userName = mTable.Rows[selectList][config.ReadConfigValue("GLOBAL", "SD_UserName")].ToString();
                        txtUnBindAccount.Text = userName;
                        txtUnBindReason.Text = "";
                    }
                    else if(this.GrdIsBan.DataSource!=null)
                    {
                        DataTable mTable = (DataTable)GrdIsBan.DataSource;
                        userID = int.Parse(mTable.Rows[selectList][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                        userName = mTable.Rows[selectList][config.ReadConfigValue("GLOBAL", "SD_UserName")].ToString();
                        txtUnBindAccount.Text = userName;
                        txtUnBindReason.Text = "";
                    }
                    else
                    {
                        txtUnBindAccount.Text = "";
                        txtUnBindReason.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion



        #region ��ͣ����ʺ�
        private void btnBanAccount_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBanAccount.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("GLOBAL", "BP_Hint_inputUserName"));
                    return;
                }
                if (this.DptBanEndTime.Value <= DateTime.Now)
                {
                    MessageBox.Show(config.ReadConfigValue("GLOBAL", "BP_Hint_banEndTime"));
                    return;
                }
                if (txtBanReason.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("GLOBAL", "BP_Hint_inputBanReason"));
                    return;
                }
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[8];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.UserByID;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                mContent[2].eName = CEnum.TagName.f_idx;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = userID;

                mContent[3].eName = CEnum.TagName.SD_UserName;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = userName;

                mContent[4].eName = CEnum.TagName.SD_Content;
                mContent[4].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[4].oContent = txtBanReason.Text.Trim();

                mContent[5].eName = CEnum.TagName.EndTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[5].oContent = DptBanEndTime.Value.ToString();

                mContent[6].eName = CEnum.TagName.SD_ServerName;
                mContent[6].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[6].oContent = serverName;

                mContent[7].eName = CEnum.TagName.f_pilot;
                mContent[7].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[7].oContent = userNick;

                backgroundWorkerBanPlayer.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ͣ����ʺ�backgroundWorker��Ϣ����
        private void backgroundWorkerBanPlayer_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_BanUser_Ban, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ͣ����ʺ�backgroundWorker��Ϣ����
        private void backgroundWorkerBanPlayer_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);
            PartInfo();
            this.txtBanAccount.Text = "";
            this.DptBanEndTime.Value = DateTime.Now;
            this.txtBanReason.Text = "";
        }
        #endregion



        #region ��ѯ��ͣ�ʺ��б�
        private void BanList()
        {
            try
            {
                cmbListPage.Items.Clear();
                this.tbcResult.SelectedTab = tpgAllBanPlayer;
                this.GrdCharacter.DataSource = null;
                this.pnlListPage.Visible = false;

                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];
                
                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.SD_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = this.cmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.SD_Type;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = 0;

                mContent[3].eName = CEnum.TagName.SD_UserName;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = userName;

                mContent[4].eName = CEnum.TagName.f_pilot;
                mContent[4].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[4].oContent = this.txtNick.Text.ToString();

                mContent[5].eName = CEnum.TagName.Index;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent = 1;

                mContent[6].eName = CEnum.TagName.PageSize;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = Operation_GD.iPageSize;

                backgroundWorkerBanList.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ��ͣ�ʺ��б�backgroundWorker��Ϣ����
        private void backgroundWorkerBanList_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_BanUser_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��ͣ�ʺ��б�backgroundWorker��Ϣ����
        private void backgroundWorkerBanList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            if (iPageCount <= 1)
            {
                this.pnlListPage.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbListPage.Items.Add(i + 1);
                }

                this.cmbListPage.SelectedIndex = 0;
                pageBanList = true;
                pnlListPage.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ��ͣ�ʺ��б�
        private void cmbListPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageBanList)
                {
                    this.GrdCharacter.DataSource = null;
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.SD_ServerName;
                    mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[1].oContent = this.cmbServer.Text.ToString();

                    mContent[2].eName = CEnum.TagName.SD_Type;
                    mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[2].oContent = 0;

                    mContent[3].eName = CEnum.TagName.SD_UserName;
                    mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[3].oContent = userName;

                    mContent[4].eName = CEnum.TagName.f_pilot;
                    mContent[4].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[4].oContent = this.txtNick.Text.ToString();

                    mContent[5].eName = CEnum.TagName.Index;
                    mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[5].oContent = int.Parse(this.cmbListPage.Text.ToString());

                    mContent[6].eName = CEnum.TagName.PageSize;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReBanList.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ��ͣ�ʺ��б�backgroundWorker��Ϣ����
        private void backgroundWorkerReBanList_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_BanUser_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ��ͣ�ʺ��б�backgroundWorker��Ϣ����
        private void backgroundWorkerReBanList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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



        #region ������ͣ�˺��б����к�
        private void GrdCharacter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//����ĳһ��
                {
                    selectList = int.Parse(e.RowIndex.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region ˫����ͣ�˺��б���н��
        private void GrdCharacter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectList = int.Parse(e.RowIndex.ToString());//������
                    if (GrdCharacter.DataSource != null)
                    {
                        tbcResult.SelectedTab = tpgUnBind;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion



        #region �������ʺ�
        private void btnUnBind_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUnBindAccount.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "BP_Hint_SelectUserName"));
                    return;
                }
                if (txtUnBindReason.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "BP_Hint_inputUnBindReason"));
                    return;
                }
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[6];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.UserByID;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                mContent[2].eName = CEnum.TagName.f_idx;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = userID;

                mContent[3].eName = CEnum.TagName.SD_UserName;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = userName;

                mContent[4].eName = CEnum.TagName.SD_Content;
                mContent[4].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[4].oContent = txtUnBindReason.Text.Trim();

                mContent[5].eName = CEnum.TagName.SD_ServerName;
                mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[5].oContent = this.cmbServer.Text.ToString();

                backgroundWorkerUnBind.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region �������ʺ�backgroundWorker��Ϣ����
        private void backgroundWorkerUnBind_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_BanUser_UnBan, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region �������ʺ�backgroundWorker��Ϣ����
        private void backgroundWorkerUnBind_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);
            BanList();
            this.txtUnBindAccount.Text = "";
            this.txtUnBindReason.Text = "";        
        }
        #endregion



        #region ������Ϣ
        private void btnResetBan_Click(object sender, EventArgs e)
        {
            DptBanEndTime.Value = DateTime.Now;
            txtBanReason.Text = "";
        }
        
        private void btnReset_Click(object sender, EventArgs e)
        {
            txtUnBindReason.Text = "";
        }
        #endregion



        #region ����ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void btnSearchInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbServer.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MMagic", "UIA_Hint_selectServer"));
                    return;
                }
                if (txtQueryAccount.Text.Trim().Length > 0)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdIsBan.DataSource = null;

                    serverIP = Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text);

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[6];

                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    mContent[1].eName = CEnum.TagName.SD_ServerName;
                    mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[1].oContent = this.cmbServer.Text.ToString();

                    mContent[2].eName = CEnum.TagName.SD_UserName;
                    mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[2].oContent = txtQueryAccount.Text.Trim();

                    mContent[3].eName = CEnum.TagName.SD_Type;
                    mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[3].oContent = 1;

                    mContent[4].eName = CEnum.TagName.Index;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = 1;

                    mContent[5].eName = CEnum.TagName.PageSize;
                    mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[5].oContent = Operation_GD.iPageSize;


                    backgroundWorkerQueryBan.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerQueryBan_DoWork(object sender, DoWorkEventArgs e)
        {

            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_BanUser_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerQueryBan_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdIsBan, out iPageCount);

           
        }

        private void btnResetInfo_Click(object sender, EventArgs e)
        {
            this.txtQueryAccount.Text = "";
            this.GrdIsBan.DataSource = null;
        }

        private void GrdIsBan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectList = int.Parse(e.RowIndex.ToString());//������
                    //if (GrdCharacter.DataSource != null)
                    //{
                        tbcResult.SelectedTab = tpgUnBind;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}