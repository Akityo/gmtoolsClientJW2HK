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
    [C_Global.CModuleAttribute("�����־��ѯ", "FrmGDPlayerLog", "�����־��ѯ", "GD Group")]
    public partial class FrmGDPlayerLog : Form
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
        string userAccount = null;
        int selectChar = 0;   //GrdCharacter�е�ǰѡ�е��� 

        bool pageCapsulePickedLog = false;
        bool pageUpdateLog = false;
        bool pageConversionLog = false;
        bool pageSyntheticLog = false;
        bool pageMissionLog = false;
        bool pagePlayerUpdate = false;
        bool pageDistintegrationOfTheBody = false;
        bool pageLuckyCapsulePickedLog = false;
        bool pageDaibiBodyLog = false;
        //bool pageLuckyCapsulePickedLog = false;
        int currDgSelectRow = 0;    //�����Ϣdatagrid �е�ǰѡ�е���
        #endregion

        public FrmGDPlayerLog()
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
            FrmGDPlayerLog mModuleFrm = new FrmGDPlayerLog();
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
            //this.lblServer.Text = config.ReadConfigValue("MSD", "UIC_UI_lblServer");
            //this.lblAccount.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            //this.lblNick.Text = config.ReadConfigValue("MSD", "UIC_UI_lblNick");
            //this.btnSearch.Text = config.ReadConfigValue("MSD", "UIC_UI_btnSearch");
            //this.btnClose.Text = config.ReadConfigValue("MSD", "UIC_UI_btnClose");

            //this.tpgCharacter.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgCharacter");

            //this.tpgPurchaseLog.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgMixTreeItems");
            //this.lblMixItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgUserUnits.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgUserUnits");
            //this.lblUnits.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgCombatItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgCombatItems");
            //this.lblCombatItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgOperators.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgOperators");
            //this.lblOperators.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgSkillItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgSkillItems");
            //this.lblSkillItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgPaintItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgPaintItems");
            //this.lblPaintItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //this.tpgStickItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgStickerItems");
            //this.lblStickerItems.Text = config.ReadConfigValue("MSD", "UIC_UI_lblPage");

            //tbcResult.Enabled = false;
        }
        #endregion

        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDPlayerLog_Load(object sender, EventArgs e)
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
            CmbServer = Operation_GD.BuildCombox(mServerInfo, CmbServer);
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_GD.GetItemAddr(mServerInfo, CmbServer.Text));
        }
        #endregion

        #region ��ѯ�����־��Ϣ
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                TbcResult.SelectedTab = TpgCharacter;//ѡ���ɫ��Ϣѡ�
                this.GrdCharacter.DataSource = null;

                if (CmbServer.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "UIA_Hint_selectServer"));
                    return;
                }

                serverIP = Operation_GD.GetItemAddr(mServerInfo, CmbServer.Text);
                userName = TxtAccount.Text.Trim();
                userNick = txtNick.Text.Trim();

                if (TxtAccount.Text.Trim().Length > 0 || txtNick.Text.Trim().Length > 0)
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
        #endregion

        #region ��ѯ�����־��Ϣbackgroundworker��Ϣ����
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Account_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ�����־��Ϣbackgroundworker��Ϣ����
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
        #endregion

        #region �л�ѡ�
        private void TbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (GrdCharacter.DataSource != null)
                {
                    DataTable mTable = (DataTable)GrdCharacter.DataSource;
                    userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                    userName = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "SD_UserName")].ToString();
                    if (TbcResult.SelectedTab.Text.Equals("Ť����ȡ��¼"))
                    {
                        CapsulePickedLog();//Ť����ȡ��¼
                    }
                    else if (TbcResult.SelectedTab.Text.Equals("����������¼"))
                    {
                        UpdateLog();//������¼
                    }
                    else if (TbcResult.SelectedTab.Text.Equals("���������¼"))
                    {
                        PlayerUpdateLog();//��һ����¼
                    }
                    else if (TbcResult.SelectedTab.Text.Equals("��װ��¼"))
                    {
                        ConversionLog();//��װ��¼
                    }
                    else if (TbcResult.SelectedTab.Text.Equals("�ϳɼ�¼"))
                    {
                        SyntheticLog();//�ϳɼ�¼
                    }
                    else if (TbcResult.SelectedTab.Text.Equals("�����ѯ"))
                    {
                        MissionLog();//�����ѯ
                    }
                    else if (TbcResult.SelectedTab.Text.Equals("��������¼"))
                    {
                        DistintegrationOfTheBody();//��������¼
                    }
                    else if (TbcResult.SelectedTab.Text.Equals("���ҳ�ȡ�����¼"))
                    {
                        DaibiBodyLog();//���ҳ�ȡ�����¼
                    }
                    else if (TbcResult.SelectedTab.Text.Equals("����Ť������ȡ��¼"))
                    {
                        LuckyCapsulePickedLog();//����Ť������ȡ��¼
                    }
                    else if (TbcResult.SelectedTab.Text.Equals("������ɿ�ʹ�ü�¼"))
                    {
                        BodyGNKUseLog();//������ɿ�ʹ�ü�¼
                    }
                }
                else
                {
                    this.GrdCapsulePickedLog.DataSource = null;
                    this.pnlCapsulePickedLog.Visible = false;

                    //this.GrdConversionLog.DataSource = null;
                    //this.pnlConversionLog.Visible = false;

                    this.GrdMissionLog.DataSource = null;
                    this.pnlMissionLog.Visible = false;

                    //this.GrdSyntheticLog.DataSource = null;
                    //this.pnlSyntheticLog.Visible = false;

                    this.GrdPlayerUpdateLog.DataSource = null;
                    this.pnlPlayerUpdateLog.Visible = false;

                    //this.GrdDistintegrationOfTheBody.DataSource = null;
                    //this.pnlDistintegrationOfTheBody.Visible = false;

                    this.GrdPlayerUpdateLog.DataSource = null;
                    this.pnlPlayerUpdateLog.Visible = false;

                    this.GrdDaibiBodyLog.DataSource = null;
                    this.pnlDaibiBodyLog.Visible = false;

                    this.GrdLuckyCapsulePickedLog.DataSource = null;
                    this.pnlLuckyCapsulePickedLog.Visible = false;

                    this.GrdBodyGNKUseLog.DataSource = null;
                    this.pnlBodyGNKUseLog.Visible = false;
                 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯŤ����ȡ��¼
        private void CapsulePickedLog()
       {
           try
           {
               this.GrpSearch.Enabled = false;
               this.TbcResult.Enabled = false;
               this.Cursor = Cursors.WaitCursor;//�ı����״̬
               this.GrdCapsulePickedLog.DataSource = null;

               this.pnlCapsulePickedLog.Visible = false;
               this.CmbCapsulePickedLog.Items.Clear();
               this.pageCapsulePickedLog = false;

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

               mContent[4].eName = CEnum.TagName.SD_StartTime;
               mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
               mContent[4].oContent = DtpBegin.Value;

               mContent[5].eName = CEnum.TagName.SD_EndTime;
               mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
               mContent[5].oContent = DtpEnd.Value;

               mContent[6].eName = CEnum.TagName.PageSize;
               mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
               mContent[6].oContent = Operation_GD.iPageSize;

               mContent[7].eName = CEnum.TagName.SD_Type;
               mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
               mContent[7].oContent =1;

               this.backgroundWorkerCapsulePickedLog.RunWorkerAsync(mContent);
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
       }
        #endregion

        #region ��ѯŤ����ȡ��¼backgroundworker��Ϣ����
       private void backgroundWorkerCapsulePickedLog_DoWork(object sender, DoWorkEventArgs e)
       {
           lock (typeof(C_Event.CSocketEvent))
           {
               e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
           }
       }
       #endregion

        #region ��ѯŤ����ȡ��¼backgroundworker��Ϣ����
       private void backgroundWorkerCapsulePickedLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

           Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdCapsulePickedLog, out iPageCount);

           if (iPageCount <= 1)
           {
               this.pnlCapsulePickedLog.Visible = false;
           }
           else
           {
               for (int i = 0; i < iPageCount; i++)
               {
                   this.CmbCapsulePickedLog.Items.Add(i + 1);
               }

               this.CmbCapsulePickedLog.SelectedIndex = 0;
               this.pageCapsulePickedLog = true;
               this.pnlCapsulePickedLog.Visible = true;
           }
       }
       #endregion

        #region ��ѯ������¼
       private void UpdateLog()
       {
           try
           {
               this.GrpSearch.Enabled = false;
               this.TbcResult.Enabled = false;
               this.Cursor = Cursors.WaitCursor;//�ı����״̬
               this.GrdUpdate.DataSource = null;

               this.pnlUpdate.Visible = false;
               this.cmbUpdate.Items.Clear();
               this.pageUpdateLog = false;

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

               mContent[4].eName = CEnum.TagName.SD_StartTime;
               mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
               mContent[4].oContent = DtpBegin.Value;

               mContent[5].eName = CEnum.TagName.SD_EndTime;
               mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
               mContent[5].oContent = DtpEnd.Value;

               mContent[6].eName = CEnum.TagName.PageSize;
               mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
               mContent[6].oContent = Operation_GD.iPageSize;

               mContent[7].eName = CEnum.TagName.SD_Type;
               mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
               mContent[7].oContent = 2;

               this.backgroundWorkerUpdateLog.RunWorkerAsync(mContent);
           }
           catch (Exception ex)
           {
               MessageBox.Show(ex.Message);
           }
       }
       #endregion

        #region ��ѯ������¼backgroundworker��Ϣ����
       private void backgroundWorkerUpdateLog_DoWork(object sender, DoWorkEventArgs e)
       {
           lock (typeof(C_Event.CSocketEvent))
           {
               e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
           }
       }
       #endregion

        #region ��ѯ������¼backgroundworker��Ϣ����
       private void backgroundWorkerUpdateLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

           Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdUpdate, out iPageCount);

           if (iPageCount <= 1)
           {
               this.pnlUpdate.Visible = false;
           }
           else
           {
               for (int i = 0; i < iPageCount; i++)
               {
                   this.cmbUpdate.Items.Add(i + 1);
               }

               this.cmbUpdate.SelectedIndex = 0;
               this.pageUpdateLog = true;
               this.pnlUpdate.Visible = true;
           }
       }
       #endregion

        #region ��ѯ��װ��¼
       private void ConversionLog()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.TbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                //this.GrdConversionLog.DataSource = null;

                //this.pnlConversionLog.Visible = false;
                //this.CmbConversionLog.Items.Clear();
                this.pageConversionLog = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

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

                this.backgroundWorkerConversionLog.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       #endregion

        #region ��ѯ��װ��¼backgroundworker��Ϣ����
        private void backgroundWorkerConversionLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserUnits_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��װ��¼backgroundworker��Ϣ����
        private void backgroundWorkerConversionLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.TbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdConversionLog, out iPageCount);

            //if (iPageCount <= 1)
            //{
            //    this.pnlConversionLog.Visible = false;
            //}
            //else
            //{
            //    for (int i = 0; i < iPageCount; i++)
            //    {
            //        this.CmbConversionLog.Items.Add(i + 1);
            //    }

            //    this.CmbConversionLog.SelectedIndex = 0;
            //    this.pageConversionLog = true;
            //    this.pnlConversionLog.Visible = true;
            //}
        }
        #endregion

        #region ��ѯ�ϳɼ�¼
        private void SyntheticLog()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.TbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                //this.GrdSyntheticLog.DataSource = null;

                //this.pnlSyntheticLog.Visible = false;
                //this.CmbSyntheticLog.Items.Clear();
                this.pageSyntheticLog = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

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

                this.backgroundWorkerSyntheticLog.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ�ϳɼ�¼backgroundworker��Ϣ����
        private void backgroundWorkerSyntheticLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserUnits_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ�ϳɼ�¼backgroundworker��Ϣ����
        private void backgroundWorkerSyntheticLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.TbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdSyntheticLog, out iPageCount);

            //if (iPageCount <= 1)
            //{
            //    this.pnlSyntheticLog.Visible = false;
            //}
            //else
            //{
            //    for (int i = 0; i < iPageCount; i++)
            //    {
            //        this.CmbSyntheticLog.Items.Add(i + 1);
            //    }

            //    this.CmbSyntheticLog.SelectedIndex = 0;
            //    this.pageSyntheticLog = true;
            //    this.pnlSyntheticLog.Visible = true;
            //}
        }
        #endregion

        #region ��ѯ�����¼
        private void MissionLog()
     {
         try
         {
             this.GrpSearch.Enabled = false;
             this.TbcResult.Enabled = false;
             this.Cursor = Cursors.WaitCursor;//�ı����״̬
             this.GrdMissionLog.DataSource = null;

             this.pnlMissionLog.Visible = false;
             this.CmbMissionLog.Items.Clear();
             this.pageMissionLog = false;

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

             mContent[4].eName = CEnum.TagName.SD_StartTime;
             mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
             mContent[4].oContent = DtpBegin.Value;

             mContent[5].eName = CEnum.TagName.SD_EndTime;
             mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
             mContent[5].oContent = DtpEnd.Value;

             mContent[6].eName = CEnum.TagName.PageSize;
             mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
             mContent[6].oContent = Operation_GD.iPageSize;

             mContent[7].eName = CEnum.TagName.SD_Type;
             mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
             mContent[7].oContent = 6;

             this.backgroundWorkerMissionLog.RunWorkerAsync(mContent);
         }
         catch (Exception ex)
         {
             MessageBox.Show(ex.Message);
         }
     }
        #endregion

        #region �����ѯbackgroundworker��Ϣ����
     private void backgroundWorkerMissionLog_DoWork(object sender, DoWorkEventArgs e)
     {
         lock (typeof(C_Event.CSocketEvent))
         {
             e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
         }
     }
     #endregion

        #region �����ѯbackgroundworker��Ϣ����
     private void backgroundWorkerMissionLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

         Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdMissionLog, out iPageCount);

         if (iPageCount <= 1)
         {
             this.pnlMissionLog.Visible = false;
         }
         else
         {
             for (int i = 0; i < iPageCount; i++)
             {
                 this.CmbMissionLog.Items.Add(i + 1);
             }

             this.CmbMissionLog.SelectedIndex = 0;
             this.pageMissionLog = true;
             this.pnlMissionLog.Visible = true;
         }
     }
     #endregion

        #region ��ѯ��������¼
        private void DistintegrationOfTheBody()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.TbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdDistintegrationOfTheBody.DataSource = null;

                this.pnlDistintegrationOfTheBody.Visible = false;
                this.CmbDistintegrationOfTheBody.Items.Clear();
                this.pageDistintegrationOfTheBody = false;

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

                mContent[4].eName = CEnum.TagName.SD_StartTime;
                mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[4].oContent = DtpBegin.Value;

                mContent[5].eName = CEnum.TagName.SD_EndTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[5].oContent = DtpEnd.Value;

                mContent[6].eName = CEnum.TagName.PageSize;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = Operation_GD.iPageSize;

                mContent[7].eName = CEnum.TagName.SD_Type;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = 9;

                this.backgroundWorkerDistintegrationOfTheBody.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ��������¼backgroundworker��Ϣ����
        private void backgroundWorkerDistintegrationOfTheBody_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��������¼backgroundworker��Ϣ����
        private void backgroundWorkerDistintegrationOfTheBody_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdDistintegrationOfTheBody, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlDistintegrationOfTheBody.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.CmbDistintegrationOfTheBody.Items.Add(i + 1);
                }

                this.CmbDistintegrationOfTheBody.SelectedIndex = 0;
                this.pageDistintegrationOfTheBody = true;
                this.pnlDistintegrationOfTheBody.Visible = true;
            }
        }
        #endregion

        #region ����ر�
     private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
     #endregion

        #region ��ѯŤ����ȡ��¼��ҳ
        private void CmbCapsulePickedLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageCapsulePickedLog)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdCapsulePickedLog.DataSource = null;

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
                    mContent[3].oContent = int.Parse(this.CmbCapsulePickedLog.Text.ToString());

                    mContent[4].eName = CEnum.TagName.SD_StartTime;
                    mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[4].oContent = DtpBegin.Value;

                    mContent[5].eName = CEnum.TagName.SD_EndTime;
                    mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[5].oContent = DtpEnd.Value;

                    mContent[6].eName = CEnum.TagName.PageSize;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = Operation_GD.iPageSize;

                    mContent[7].eName = CEnum.TagName.SD_Type;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 1;

                    this.backgroundWorkerReCapsulePickedLog.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


        #region ��ѯŤ����ȡ��¼��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReCapsulePickedLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯŤ����ȡ��¼��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReCapsulePickedLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdCapsulePickedLog, out iPageCount);
        }
        #endregion

        #region ��ѯ������¼��ҳ
        private void CmbUpdateLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pagePlayerUpdate)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdPlayerUpdateLog.DataSource = null;

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

                    mContent[3].eName = CEnum.TagName.SD_StartTime;
                    mContent[3].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[3].oContent = DtpBegin.Value;

                    mContent[4].eName = CEnum.TagName.SD_EndTime;
                    mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[4].oContent = DtpEnd.Value;

                    mContent[5].eName = CEnum.TagName.Index;
                    mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[5].oContent = int.Parse(this.CmbPlayerUpdateLog.Text.ToString());

                    mContent[6].eName = CEnum.TagName.PageSize;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = Operation_GD.iPageSize;

                    mContent[7].eName = CEnum.TagName.SD_Type;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 3;

                    this.backgroundWorkerReUpdateLog.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ������¼��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReUpdateLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ������¼��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReUpdateLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdUpdate, out iPageCount);
        }
        #endregion

        #region ��ѯ��װ��¼��ҳ
        private void CmbConversionLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageConversionLog)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    //this.GrdConversionLog.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

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
                    //mContent[3].oContent = int.Parse(this.CmbConversionLog.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    this.backgroundWorkerReConversionLog.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ��װ��¼��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReConversionLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_Item_Sticker_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��װ��¼��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReConversionLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.TbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdConversionLog, out iPageCount);
        }
        #endregion

        #region ��ѯ�ϳɼ�¼��ҳ
        private void CmbSyntheticLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pageSyntheticLog)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    //this.GrdSyntheticLog.DataSource = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

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
                    //mContent[3].oContent = int.Parse(this.CmbSyntheticLog.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                   this.backgroundWorkerReSyntheticLog.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ�ϳɼ�¼��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReSyntheticLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_Item_Sticker_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ�ϳɼ�¼��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReSyntheticLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //this.GrpSearch.Enabled = true;
            //this.TbcResult.Enabled = true;
            //this.Cursor = Cursors.Default;//�ı����״̬

            //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //{
            //    MessageBox.Show(mResult[0, 0].oContent.ToString());
            //    return;
            //}

            //Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdSyntheticLog, out iPageCount);
        }
        #endregion

        #region ��ѯ�����¼��ҳ
        private void CmbMissionLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageMissionLog)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdMissionLog.DataSource = null;

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

                    mContent[3].eName = CEnum.TagName.SD_StartTime;
                    mContent[3].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[3].oContent = DtpBegin.Value;

                    mContent[4].eName = CEnum.TagName.SD_EndTime;
                    mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[4].oContent = DtpEnd.Value;

                    mContent[5].eName = CEnum.TagName.Index;
                    mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[5].oContent = int.Parse(this.CmbMissionLog.Text.ToString());

                    mContent[6].eName = CEnum.TagName.PageSize;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = Operation_GD.iPageSize;

                    mContent[7].eName = CEnum.TagName.SD_Type;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 6;

                    this.backgroundWorkerReMissionLog.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ������־��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReMissionLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ������־��ҳbackgroundworker��Ϣ����
        private void backgroundWorkerReMissionLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdMissionLog, out iPageCount);
        }
        #endregion

        private void GrdCharacter_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectChar = int.Parse(e.RowIndex.ToString());//������
                    if (GrdCharacter.DataSource != null)
                    {
                        this.TbcResult.SelectedTab = this.TpgCapsulePickedLog;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerPlayerUpdateLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerPlayerUpdateLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdPlayerUpdateLog, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlPlayerUpdateLog.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.CmbPlayerUpdateLog.Items.Add(i + 1);
                }

                this.CmbPlayerUpdateLog.SelectedIndex = 0;
                this.pagePlayerUpdate = true;
                this.pnlPlayerUpdateLog.Visible = true;
            }
        }

        private void PlayerUpdateLog()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.TbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdPlayerUpdateLog.DataSource = null;

                this.pnlPlayerUpdateLog.Visible = false;
                this.CmbPlayerUpdateLog.Items.Clear();
                this.pageUpdateLog = false;

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

                mContent[3].eName = CEnum.TagName.SD_StartTime;
                mContent[3].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[3].oContent = DtpBegin.Value;

                mContent[4].eName = CEnum.TagName.SD_EndTime;
                mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[4].oContent = DtpEnd.Value;

                mContent[5].eName = CEnum.TagName.Index;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent = 1;

                mContent[6].eName = CEnum.TagName.PageSize;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = Operation_GD.iPageSize;

                mContent[7].eName = CEnum.TagName.SD_Type;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = 3;
                this.backgroundWorkerPlayerUpdateLog.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        
        
        }

        private void cmbUpdate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pageUpdateLog)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdUpdate.DataSource = null;

                    this.pnlUpdate.Visible = false;
                    this.cmbUpdate.Items.Clear();
                    this.pageUpdateLog = false;

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

                    mContent[3].eName = CEnum.TagName.SD_StartTime;
                    mContent[3].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[3].oContent = DtpBegin.Value;

                    mContent[4].eName = CEnum.TagName.SD_EndTime;
                    mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[4].oContent = DtpEnd.Value;

                    mContent[5].eName = CEnum.TagName.Index;
                    mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[5].oContent = int.Parse(this.cmbUpdate.Text.ToString());

                    mContent[6].eName = CEnum.TagName.PageSize;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = Operation_GD.iPageSize;

                    mContent[7].eName = CEnum.TagName.SD_Type;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 2;

                    this.backgroundWorkerReUpdateLog.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }

        

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

        private void backgroundWorkerRePlayerUpdateLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerRePlayerUpdateLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdPlayerUpdateLog, out iPageCount);

         
        }


        #region ��ѯ��������¼��ҳ
        private void CmbDistintegrationOfTheBody_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pageDistintegrationOfTheBody)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdDistintegrationOfTheBody.DataSource = null;

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
                    mContent[3].oContent = int.Parse(this.CmbDistintegrationOfTheBody.Text.ToString());

                    mContent[4].eName = CEnum.TagName.SD_StartTime;
                    mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[4].oContent = DtpBegin.Value;

                    mContent[5].eName = CEnum.TagName.SD_EndTime;
                    mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[5].oContent = DtpEnd.Value;

                    mContent[6].eName = CEnum.TagName.PageSize;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent =Operation_GD.iPageSize;

                    mContent[7].eName = CEnum.TagName.SD_Type;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 9;

                    this.backgroundWorkerReDistintegrationOfTheBody.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ��������¼backgroundworker��Ϣ����
        private void backgroundWorkerReDistintegrationOfTheBody_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��������¼backgroundworker��Ϣ����
        private void backgroundWorkerReDistintegrationOfTheBody_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdDistintegrationOfTheBody, out iPageCount);
        }
        #endregion

        private void CmbDaibiBodyLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pageDaibiBodyLog)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdDaibiBodyLog.DataSource = null;

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
                    mContent[3].oContent = int.Parse(this.CmbDaibiBodyLog.Text.ToString());

                    mContent[4].eName = CEnum.TagName.SD_StartTime;
                    mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[4].oContent = DtpBegin.Value;

                    mContent[5].eName = CEnum.TagName.SD_EndTime;
                    mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[5].oContent = DtpEnd.Value;

                    mContent[6].eName = CEnum.TagName.PageSize;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = Operation_GD.iPageSize;

                    mContent[7].eName = CEnum.TagName.SD_Type;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 8;

                    this.backgroundWorkerReDaibiBodyLog.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerReDaibiBodyLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerReDaibiBodyLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdDaibiBodyLog, out iPageCount);
        }


        private void DaibiBodyLog()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.TbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdDaibiBodyLog.DataSource = null;

                this.pnlDaibiBodyLog.Visible = false;
                this.CmbDaibiBodyLog.Items.Clear();
                this.pageDaibiBodyLog = false;

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

                mContent[4].eName = CEnum.TagName.SD_StartTime;
                mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[4].oContent = DtpBegin.Value;

                mContent[5].eName = CEnum.TagName.SD_EndTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[5].oContent = DtpEnd.Value;

                mContent[6].eName = CEnum.TagName.PageSize;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = Operation_GD.iPageSize;

                mContent[7].eName = CEnum.TagName.SD_Type;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = 8;

                this.backgroundWorkerDaibiBodyLog.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LuckyCapsulePickedLog()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.TbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdLuckyCapsulePickedLog.DataSource = null;

                this.pnlLuckyCapsulePickedLog.Visible = false;
                this.cmbLuckyCapsulePickedLog.Items.Clear();
                this.pageDaibiBodyLog = false;

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

                mContent[4].eName = CEnum.TagName.SD_StartTime;
                mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[4].oContent = DtpBegin.Value;

                mContent[5].eName = CEnum.TagName.SD_EndTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[5].oContent = DtpEnd.Value;

                mContent[6].eName = CEnum.TagName.PageSize;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = Operation_GD.iPageSize;

                mContent[7].eName = CEnum.TagName.SD_Type;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = 10;

                this.backgroundWorkerDaibiBodyLog.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BodyGNKUseLog()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.TbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdBodyGNKUseLog.DataSource = null;

                this.pnlBodyGNKUseLog.Visible = false;
                this.cmbBodyGNKUseLog.Items.Clear();
                this.pageDaibiBodyLog = false;

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

                mContent[4].eName = CEnum.TagName.SD_StartTime;
                mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[4].oContent = DtpBegin.Value;

                mContent[5].eName = CEnum.TagName.SD_EndTime;
                mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                mContent[5].oContent = DtpEnd.Value;

                mContent[6].eName = CEnum.TagName.PageSize;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = Operation_GD.iPageSize;

                mContent[7].eName = CEnum.TagName.SD_Type;
                mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[7].oContent = 11;

                this.backgroundWorkerDaibiBodyLog.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerDaibiBodyLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerDaibiBodyLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdDaibiBodyLog, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlDaibiBodyLog.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.CmbDaibiBodyLog.Items.Add(i + 1);
                }

                this.CmbDaibiBodyLog.SelectedIndex = 0;
                this.pageDaibiBodyLog = true;
                this.pnlDaibiBodyLog.Visible = true;
            }
        }

        private void cmbLuckyCapsulePickedLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pageLuckyCapsulePickedLog)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdDaibiBodyLog.DataSource = null;

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
                    mContent[3].oContent = int.Parse(this.cmbLuckyCapsulePickedLog.Text.ToString());

                    mContent[4].eName = CEnum.TagName.SD_StartTime;
                    mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[4].oContent = DtpBegin.Value;

                    mContent[5].eName = CEnum.TagName.SD_EndTime;
                    mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[5].oContent = DtpEnd.Value;

                    mContent[6].eName = CEnum.TagName.PageSize;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = Operation_GD.iPageSize;

                    mContent[7].eName = CEnum.TagName.SD_Type;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 10;

                    this.backgroundWorkerReLuckyCapsulePickedLog.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbBodyGNKUseLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.pageDaibiBodyLog)
                {
                    this.GrpSearch.Enabled = false;
                    this.TbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdDaibiBodyLog.DataSource = null;

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
                    mContent[3].oContent = int.Parse(this.cmbBodyGNKUseLog.Text.ToString());

                    mContent[4].eName = CEnum.TagName.SD_StartTime;
                    mContent[4].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[4].oContent = DtpBegin.Value;

                    mContent[5].eName = CEnum.TagName.SD_EndTime;
                    mContent[5].eTag = CEnum.TagFormat.TLV_TIMESTAMP;
                    mContent[5].oContent = DtpEnd.Value;

                    mContent[6].eName = CEnum.TagName.PageSize;
                    mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[6].oContent = Operation_GD.iPageSize;

                    mContent[7].eName = CEnum.TagName.SD_Type;
                    mContent[7].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[7].oContent = 11;

                    this.backgroundWorkerReDaibiBodyLog.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerLuckyCapsulePickedLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerLuckyCapsulePickedLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdLuckyCapsulePickedLog, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlLuckyCapsulePickedLog.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbLuckyCapsulePickedLog.Items.Add(i + 1);
                }

                this.CmbDaibiBodyLog.SelectedIndex = 0;
                this.pageLuckyCapsulePickedLog = true;
                this.pnlLuckyCapsulePickedLog.Visible = true;
            }
        }

        private void backgroundWorkerReLuckyCapsulePickedLog_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_LogInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerReLuckyCapsulePickedLog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, this.GrdLuckyCapsulePickedLog, out iPageCount);

        }
  
    }

}