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
using System.Data.OleDb;
namespace M_GD
{
    [C_Global.CModuleAttribute("���߹���", "FrmGDItemManage", "���߹���", "GD Group")]
    public partial class FrmGDItemManage : Form
    {
        #region ����

        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;

        private CEnum.Message_Body[,] mItemList = null;

        private int iPageCount = 0;//��ҳҳ��

        int userID = 0;
        string serverIP = null;
        string userName = null;
        string userNick = null;
        long itemID = 0;
        string itemName = null;
        string BodyName = null;
        int selectChar = 0;   //GrdCharacter�е�ǰѡ�е��� 
        int selectItem = 0;

        int itemCount = 0;

        bool pageMixItems = false;
        bool pageUnits = false;
        bool pageCombat = false;
        bool pageOperator = false;
        bool pagePaint = false;
        bool pageSkill = false;
        bool pageSticker = false;
        private string batchAddItem = null;
        Hashtable hItemList = new Hashtable();

        #endregion

        public FrmGDItemManage()
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
            FrmGDItemManage mModuleFrm = new FrmGDItemManage();
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

            //this.tpgMixTreeItems.Text = config.ReadConfigValue("MSD", "UIC_UI_tpgMixTreeItems");
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

            //this.tpgAddItem.Text = config.ReadConfigValue("MSD", "AI_UI_tpgAddItem");
            //this.lblCharName.Text = config.ReadConfigValue("MSD", "UIC_UI_lblAccount");
            //this.lblItemName.Text = config.ReadConfigValue("MSD", "AI_UI_lblItemName");
            //this.lblItemNum.Text = config.ReadConfigValue("MSD", "AI_UI_lblItemNum");
            //this.lblItemList.Text = config.ReadConfigValue("MSD", "AI_UI_lblItemList");
            //this.lblTitle.Text = config.ReadConfigValue("MSD", "AI_UI_lblTitle");
            //this.lblContent.Text = config.ReadConfigValue("MSD", "AI_UI_lblContent");
            //this.btnAddItem.Text = config.ReadConfigValue("MSD", "AI_UI_btnAddItem");
            //this.btnReset.Text = config.ReadConfigValue("MSD", "BP_UI_btnReset");
            //this.colItemName.Text = config.ReadConfigValue("MSD", "AI_UI_colItemName");
            //this.colItemNum.Text = config.ReadConfigValue("MSD", "AI_UI_colItemNum");
        }

        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDItemManage_Load(object sender, EventArgs e)
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
                    PartInfo();
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

        #region ��ѯ���������Ϣ
        private void PartInfo()
        {
            try
            {
                tbcResult.SelectedTab = tpgCharacter;//ѡ���ɫ��Ϣѡ�
                this.GrdCharacter.DataSource = null;
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
                        tbcResult.SelectedTab = tpgMixTreeItems;
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
                    userNick = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_pilot")].ToString();
                    if (tbcResult.SelectedTab.Text.Equals("��ҙC�w�M��"))
                    {
                        MixTreeItems();//��ѯ�������
                    }
                    else if (tbcResult.SelectedTab.Text.Equals("��ҙC�w��Ϣ"))
                    {
                        UnitsItem();//��ѯ������Ϣ
                    }
                    else if (tbcResult.SelectedTab.Text.Equals("��ґ��Y����"))
                    {
                        CombatItems();//��ѯս������
                    }
                    else if (tbcResult.SelectedTab.Text.Equals("��Ҹ�����Ϣ"))
                    {
                        OperaterInfo();//��ѯ������Ϣ
                    }
                    else if (tbcResult.SelectedTab.Text.Equals("��҉T�b����"))
                    {
                        PaintItems();//��ѯͿװ����
                    }
                    else if (tbcResult.SelectedTab.Text.Equals("��Ҽ��ܵ���"))
                    {
                        SkillItems();//��ѯ���ܵ���
                    }
                    else if (tbcResult.SelectedTab.Text.Equals("��Ҙ˻`����"))
                    {
                        StickerItems();//��ѯ��ǩ����
                    }
                    else if (tbcResult.SelectedTab.Text.Equals("��ӵ���"))
                    {
                        ItemList();//��ȡ�����б�
                        this.lblSearch.Visible = false;
                        this.txtSearch.Visible = false;
                        this.btnBluSearch.Visible = false;
                        txtCharName.Text = userName;
                        listViewAddItem.Clear();
                        listViewAddItem.Columns.Add(config.ReadConfigValue("GLOBAL", "AI_UI_colItemName"), 232);
                        listViewAddItem.Columns.Add(config.ReadConfigValue("GLOBAL", "AI_UI_colItemNum"), 65);
                    }
                }
                else
                {
                    GrdMixItems.DataSource = null;
                    pnlMixItems.Visible = false;

                    GrdUnits.DataSource = null;
                    pnlUnits.Visible = false;

                    GrdCombatItems.DataSource = null;
                    pnlCombatItems.Visible = false;

                    GrdOperators.DataSource = null;
                    pnlOperators.Visible = false;

                    GrdPaintItems.DataSource = null;
                    pnlPaintItems.Visible = false;

                    GrdSkillItems.DataSource = null;
                    pnlSkillItems.Visible = false;

                    GrdStickerItems.DataSource = null;
                    pnlStickerItems.Visible = false;

                    cmbItemName.Items.Clear();
                    listViewAddItem.Clear();
                    listViewAddItem.Columns.Add(config.ReadConfigValue("GLOBAL", "AI_UI_colItemName"), 232);
                    listViewAddItem.Columns.Add(config.ReadConfigValue("GLOBAL", "AI_UI_colItemNum"), 65);
                    NudItemNum.Value = 1;
                    txtCharName.Clear();
                    txtTitle.Clear();
                    txtContent.Clear();
                    hItemList.Clear();
                    itemCount = 0;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion



        #region ��ѯ���������Ϣ
        private void MixTreeItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdMixItems.DataSource = null;

                this.pnlMixItems.Visible = false;
                this.cmbMixItems.Items.Clear();
                this.pageMixItems = false;

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

                backgroundWorkerMixTree.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerMixTree_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_MixTree_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerMixTree_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdMixItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlMixItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbMixItems.Items.Add(i + 1);
                }

                cmbMixItems.SelectedIndex = 0;
                pageMixItems = true;
                pnlMixItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ���������Ϣ
        private void cmbMixItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageMixItems)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdMixItems.DataSource = null;

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
                    mContent[3].oContent = int.Parse(cmbMixItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReMixTree.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReMixTree_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_MixTree_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ���������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReMixTree_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdMixItems, out iPageCount);
        }
        #endregion



        #region ��ѯ������Ϣ
        private void UnitsItem()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdUnits.DataSource = null;

                this.pnlUnits.Visible = false;
                this.cmbUnits.Items.Clear();
                this.pageUnits = false;

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

                backgroundWorkerUnits.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerUnits_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserUnits_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerUnits_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdUnits, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlUnits.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbUnits.Items.Add(i + 1);
                }

                cmbUnits.SelectedIndex = 0;
                pageUnits = true;
                pnlUnits.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ������Ϣ
        private void cmbUnits_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageUnits)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdUnits.DataSource = null;

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
                    mContent[3].oContent = int.Parse(cmbUnits.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReUnits.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReUnits_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserUnits_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReUnits_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdUnits, out iPageCount);
        }
        #endregion



        #region ��ѯս��������Ϣ
        private void CombatItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdCombatItems.DataSource = null;

                this.pnlCombatItems.Visible = false;
                this.cmbCombatItems.Items.Clear();
                this.pageCombat = false;

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

                backgroundWorkerCombatItems.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯս��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerCombatItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserCombatitems_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯս��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerCombatItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdCombatItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlCombatItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbCombatItems.Items.Add(i + 1);
                }

                cmbCombatItems.SelectedIndex = 0;
                pageCombat = true;
                pnlCombatItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯս��������Ϣ
        private void cmbCombatItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageCombat)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdCombatItems.DataSource = null;

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
                    mContent[3].oContent = int.Parse(cmbCombatItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReCombatItems.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯս��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReCombatItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserCombatitems_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯս��������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReCombatItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdCombatItems, out iPageCount);
        }
        #endregion



        #region ��ѯ������Ϣ
        private void OperaterInfo()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdOperators.DataSource = null;

                this.pnlOperators.Visible = false;
                this.cmbOperators.Items.Clear();
                this.pageOperator = false;

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

                backgroundWorkerOperators.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerOperators_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Operator_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerOperators_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdOperators, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlOperators.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbOperators.Items.Add(i + 1);
                }

                cmbOperators.SelectedIndex = 0;
                pageOperator = true;
                pnlOperators.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ������Ϣ
        private void cmbOperators_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageOperator)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdOperators.DataSource = null;

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
                    mContent[3].oContent = int.Parse(cmbOperators.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReOperators.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReOperators_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Operator_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerReOperators_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdOperators, out iPageCount);
        }
        #endregion



        #region ��ѯͿװ����
        private void PaintItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdPaintItems.DataSource = null;

                this.pnlPaintItems.Visible = false;
                this.cmbPaintItems.Items.Clear();
                this.pagePaint = false;

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

                backgroundWorkerPaintItems.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯͿװ����backgroundWorker��Ϣ����
        private void backgroundWorkerPaintItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Paint_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯͿװ����backgroundWorker��Ϣ����
        private void backgroundWorkerPaintItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdPaintItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlPaintItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbPaintItems.Items.Add(i + 1);
                }

                cmbPaintItems.SelectedIndex = 0;
                pagePaint = true;
                pnlPaintItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯͿװ����
        private void cmbPaintItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pagePaint)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdPaintItems.DataSource = null;

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
                    mContent[3].oContent = int.Parse(cmbPaintItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerRePaintItems.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯͿװ����backgroundWorker��Ϣ����
        private void backgroundWorkerRePaintItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Paint_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯͿװ����backgroundWorker��Ϣ����
        private void backgroundWorkerRePaintItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdPaintItems, out iPageCount);
        }
        #endregion



        #region ��ѯ���ܵ���
        private void SkillItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdSkillItems.DataSource = null;

                this.pnlSkillItems.Visible = false;
                this.cmbSkillItems.Items.Clear();
                this.pageSkill = false;

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

                backgroundWorkerSkillItems.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ���ܵ���backgroundWorker��Ϣ����
        private void backgroundWorkerSkillItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Skill_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ���ܵ���backgroundWorker��Ϣ����
        private void backgroundWorkerSkillItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdSkillItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlSkillItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbSkillItems.Items.Add(i + 1);
                }

                cmbSkillItems.SelectedIndex = 0;
                pageSkill = true;
                pnlSkillItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ���ܵ���
        private void cmbSkillItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageSkill)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdSkillItems.DataSource = null;

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
                    mContent[3].oContent = int.Parse(cmbSkillItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReSkillItems.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ���ܵ���backgroundWorker��Ϣ����
        private void backgroundWorkerReSkillItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Skill_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ���ܵ���backgroundWorker��Ϣ����
        private void backgroundWorkerReSkillItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdSkillItems, out iPageCount);
        }
        #endregion



        #region ��ѯ��ǩ����
        private void StickerItems()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                this.GrdStickerItems.DataSource = null;

                this.pnlStickerItems.Visible = false;
                this.cmbStickerItems.Items.Clear();
                this.pageSticker = false;

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

                backgroundWorkerStickerItems.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ��ǩ����backgroundWorker��Ϣ����
        private void backgroundWorkerStickerItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Sticker_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ��ǩ����backgroundWorker��Ϣ����
        private void backgroundWorkerStickerItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdStickerItems, out iPageCount);

            if (iPageCount <= 1)
            {
                this.pnlStickerItems.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    this.cmbStickerItems.Items.Add(i + 1);
                }

                cmbStickerItems.SelectedIndex = 0;
                pageSticker = true;
                pnlStickerItems.Visible = true;
            }
        }
        #endregion



        #region ��ҳ��ѯ��ǩ����
        private void cmbStickerItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pageSticker)
                {
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬
                    this.GrdStickerItems.DataSource = null;

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
                    mContent[3].oContent = int.Parse(cmbStickerItems.Text.Trim());

                    mContent[4].eName = CEnum.TagName.PageSize;
                    mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[4].oContent = Operation_GD.iPageSize;

                    backgroundWorkerReStickerItems.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ҳ��ѯ��ǩ����backgroundWorker��Ϣ����
        private void backgroundWorkerReStickerItems_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_Sticker_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ҳ��ѯ��ǩ����backgroundWorker��Ϣ����
        private void backgroundWorkerReStickerItems_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

            Operation_GD.BuildDataTable(this.m_ClientEvent, mResult, GrdStickerItems, out iPageCount);
        }
        #endregion



        #region ˫����ҵ�����Ϣ��ɾ����ҵ���

        #region ɾ����һ�����ϵ���
        private void GrdMixItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectItem = int.Parse(e.RowIndex.ToString());//������
                    if (this.GrdMixItems.DataSource != null)
                    {
                        DataTable sTable = (DataTable)GrdMixItems.DataSource;
                        itemID = long.Parse(sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ID")].ToString());//��������ʺ�ID
                        itemName = sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ItemName")].ToString();
                        if (itemName == "�޵���")
                        {
                            MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_NoItemToDelete"));
                            return;
                        }
                        if (MessageBox.Show("�Ƿ�ȷ��Ҫɾ�����ߣ�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.GrpSearch.Enabled = false;
                            this.tbcResult.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;//�ı����״̬

                            CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

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

                            mContent[4].eName = CEnum.TagName.SD_ID;
                            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[4].oContent = itemID;

                            mContent[5].eName = CEnum.TagName.SD_ItemName;
                            mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[5].oContent = itemName;

                            mContent[6].eName = CEnum.TagName.SD_Type;
                            mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[6].oContent = 1;
                            
                            backgroundWorkerDeleteItem.RunWorkerAsync(mContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ɾ���������
        private void GrdUnits_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectItem = int.Parse(e.RowIndex.ToString());//������
                    if (this.GrdUnits.DataSource != null)
                    {
                        DataTable sTable = (DataTable)GrdUnits.DataSource;
                        itemID = long.Parse(sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ID")].ToString());//��������ʺ�ID
                        itemName = sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ItemName")].ToString();
                        //if (itemName == "�޵���")
                        //{
                        //    MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_NoItemToDelete"));
                        //    return;
                        //}
                        if (MessageBox.Show("�Ƿ�ȷ��Ҫɾ�����ߣ�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.GrpSearch.Enabled = false;
                            this.tbcResult.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;//�ı����״̬

                            CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

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

                            mContent[4].eName = CEnum.TagName.SD_ID;
                            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[4].oContent = itemID;

                            mContent[5].eName = CEnum.TagName.SD_ItemName;
                            mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[5].oContent = itemName;

                            mContent[6].eName = CEnum.TagName.SD_Type;
                            mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[6].oContent = 2;

                            backgroundWorkerDeleteItem.RunWorkerAsync(mContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ɾ��ս������
        private void GrdCombatItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectItem = int.Parse(e.RowIndex.ToString());//������
                    if (this.GrdCombatItems.DataSource != null)
                    {
                        DataTable sTable = (DataTable)GrdCombatItems.DataSource;
                        itemID = long.Parse(sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ID")].ToString());//��������ʺ�ID
                       
                        itemName = sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ItemName")].ToString();
                        if (itemName == "�޵���")
                        {
                         
                          
                            MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_NoItemToDelete"));
                            return;
                      
                            
                        }
                       
                        if (MessageBox.Show("�Ƿ�ȷ��Ҫɾ�����ߣ�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.GrpSearch.Enabled = false;
                            this.tbcResult.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;//�ı����״̬

                            CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

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

                            mContent[4].eName = CEnum.TagName.SD_ID;
                            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[4].oContent = itemID;

                            mContent[5].eName = CEnum.TagName.SD_ItemName;
                            mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[5].oContent = itemName;

                            mContent[6].eName = CEnum.TagName.SD_Type;
                            mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[6].oContent = 3;

                            backgroundWorkerDeleteItem.RunWorkerAsync(mContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ɾ����Ҹ�����Ϣ
        private void GrdOperators_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectItem = int.Parse(e.RowIndex.ToString());//������
                    if (this.GrdOperators.DataSource != null)
                    {
                        DataTable sTable = (DataTable)GrdOperators.DataSource;
                        itemID = long.Parse(sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ID")].ToString());//��������ʺ�ID
                        itemName = sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ItemName")].ToString();
                        if (itemName == "�޵���")
                        {
                            MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_NoItemToDelete"));
                            return;
                        }
                        if (MessageBox.Show("�Ƿ�ȷ��Ҫɾ�����ߣ�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.GrpSearch.Enabled = false;
                            this.tbcResult.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;//�ı����״̬

                            CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

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

                            mContent[4].eName = CEnum.TagName.SD_ID;
                            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[4].oContent = itemID;

                            mContent[5].eName = CEnum.TagName.SD_ItemName;
                            mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[5].oContent = itemName;

                            mContent[6].eName = CEnum.TagName.SD_Type;
                            mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[6].oContent = 4;

                            backgroundWorkerDeleteItem.RunWorkerAsync(mContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ɾ�����Ϳװ����
        private void GrdPaintItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectItem = int.Parse(e.RowIndex.ToString());//������
                    if (this.GrdPaintItems.DataSource != null)
                    {
                        DataTable sTable = (DataTable)GrdPaintItems.DataSource;
                        itemID = long.Parse(sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ID")].ToString());//��������ʺ�ID
                        itemName = sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ItemName")].ToString();
                        if (itemName == "�޵���")
                        {
                            MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_NoItemToDelete"));
                            return;
                        }
                        if (MessageBox.Show("�Ƿ�ȷ��Ҫɾ�����ߣ�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.GrpSearch.Enabled = false;
                            this.tbcResult.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;//�ı����״̬

                            CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

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

                            mContent[4].eName = CEnum.TagName.SD_ID;
                            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[4].oContent = itemID;

                            mContent[5].eName = CEnum.TagName.SD_ItemName;
                            mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[5].oContent = itemName;

                            mContent[6].eName = CEnum.TagName.SD_Type;
                            mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[6].oContent = 5;

                            backgroundWorkerDeleteItem.RunWorkerAsync(mContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ɾ����Ҽ��ܵ���
        private void GrdSkillItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectItem = int.Parse(e.RowIndex.ToString());//������
                    if (this.GrdSkillItems.DataSource != null)
                    {
                        DataTable sTable = (DataTable)GrdSkillItems.DataSource;
                        itemID = long.Parse(sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ID")].ToString());//��������ʺ�ID
                        itemName = sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ItemName")].ToString();
                        if (itemName == "�޵���")
                        {
                            MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_NoItemToDelete"));
                            return;
                        }
                        if (MessageBox.Show("�Ƿ�ȷ��Ҫɾ�����ߣ�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.GrpSearch.Enabled = false;
                            this.tbcResult.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;//�ı����״̬

                            CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

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

                            mContent[4].eName = CEnum.TagName.SD_ID;
                            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[4].oContent = itemID;

                            mContent[5].eName = CEnum.TagName.SD_ItemName;
                            mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[5].oContent = itemName;

                            mContent[6].eName = CEnum.TagName.SD_Type;
                            mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[6].oContent = 6;

                            backgroundWorkerDeleteItem.RunWorkerAsync(mContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ɾ����ұ�ǩ����
        private void GrdStickerItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectItem = int.Parse(e.RowIndex.ToString());//������
                    if (this.GrdStickerItems.DataSource != null)
                    {
                        DataTable sTable = (DataTable)GrdStickerItems.DataSource;
                        itemID = long.Parse(sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ID")].ToString());//��������ʺ�ID
                        itemName = sTable.Rows[selectItem][config.ReadConfigValue("GLOBAL", "SD_ItemName")].ToString();
                        if ("�޵���" == itemName)
                        {
                            MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_NoItemToDelete"));
                            return;
                        }
                        if (MessageBox.Show("�Ƿ�ȷ��Ҫɾ�����ߣ�", "��ʾ", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            this.GrpSearch.Enabled = false;
                            this.tbcResult.Enabled = false;
                            this.Cursor = Cursors.WaitCursor;//�ı����״̬

                            CEnum.Message_Body[] mContent = new CEnum.Message_Body[7];

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

                            mContent[4].eName = CEnum.TagName.SD_ID;
                            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[4].oContent = itemID;

                            mContent[5].eName = CEnum.TagName.SD_ItemName;
                            mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                            mContent[5].oContent = itemName;

                            mContent[6].eName = CEnum.TagName.SD_Type;
                            mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                            mContent[6].oContent = 7;

                            backgroundWorkerDeleteItem.RunWorkerAsync(mContent);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #endregion

        #region ɾ����ҵ���backgroundWorker��Ϣ����
        private void backgroundWorkerDeleteItem_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UserAdditem_Del, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ɾ����ҵ���backgroundWorker��Ϣ����
        private void backgroundWorkerDeleteItem_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);
            PartInfo();
        }
        #endregion



        #region ��ѯ�����б�
        private void ItemList()
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[3];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.SD_Type;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = 1;

                mContent[2].eName = CEnum.TagName.SD_ItemName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent ="";
                backgroundWorkerItemList.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ѯ�����б�backgroundWorker��Ϣ����
        private void backgroundWorkerItemList_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_ItemList_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ�����б�backgroundWorker��Ϣ����
        private void backgroundWorkerItemList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            mItemList = mResult;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            cmbItemName = Operation_GD.BuildComboxAno(mResult, cmbItemName);
        }
        #endregion



        #region ѡ�е���ʱ������������Ϊ1
        private void cmbItemName_SelectedIndexChanged(object sender, EventArgs e)
        {
            NudItemNum.Value = 1;
        }
        #endregion



        #region �����͵����б�����ӵ���
        private void btnAddList_Click(object sender, EventArgs e)
        {
            try
            {
                int index = 0;

                for (int i = 0; i < mItemList.GetLength(0); i++)
                {
                    if (mItemList[i, 1].oContent.ToString().Trim() == cmbItemName.Text.Trim())
                    {
                        index = i;
                    }
                }
                if (cmbItemName.Text.Trim().Length > 0)
                {
                    if (listViewAddItem.Items.Count == 0)
                    {
                        listViewAddItem.BeginUpdate();
                        ListViewItem Item = new ListViewItem();
                        Item.SubItems[0].Text = cmbItemName.Text.Trim();
                        Item.SubItems.Add(NudItemNum.Value.ToString());
                        listViewAddItem.Items.Add(Item);
                        hItemList.Add(cmbItemName.Text.Trim(), mItemList[index, 0].oContent.ToString().Trim() + " " + NudItemNum.Value.ToString() + " " + mItemList[index, 1].oContent.ToString().Trim());
                        itemCount += int.Parse(NudItemNum.Value.ToString());
                        listViewAddItem.EndUpdate();
                        return;
                    }
                    for (int i = 0; i < listViewAddItem.Items.Count; i++)
                    {
                        if (listViewAddItem.Items[i].SubItems[0].Text == cmbItemName.Text.Trim())
                        {
                            listViewAddItem.BeginUpdate();
                            listViewAddItem.Items[i].Selected = true;
                            hItemList.Remove(cmbItemName.Text.Trim());
                            itemCount -= int.Parse(listViewAddItem.SelectedItems[0].SubItems[1].Text);
                            listViewAddItem.Items.Remove(listViewAddItem.SelectedItems[0]);
                            ListViewItem Item = new ListViewItem();
                            Item.SubItems[0].Text = cmbItemName.Text.Trim();
                            Item.SubItems.Add(NudItemNum.Value.ToString());
                            listViewAddItem.Items.Add(Item);
                            hItemList.Add(cmbItemName.Text.Trim(), mItemList[index, 0].oContent.ToString().Trim() + " " + NudItemNum.Value.ToString() + " " + mItemList[index, 1].oContent.ToString().Trim());
                            itemCount += int.Parse(NudItemNum.Value.ToString());
                            listViewAddItem.EndUpdate();
                            return;
                        }
                    }
                    listViewAddItem.BeginUpdate();
                    ListViewItem Item1 = new ListViewItem();
                    Item1.SubItems[0].Text = cmbItemName.Text.Trim();
                    Item1.SubItems.Add(NudItemNum.Value.ToString());
                    listViewAddItem.Items.Add(Item1);
                    hItemList.Add(cmbItemName.Text.Trim(), mItemList[index, 0].oContent.ToString().Trim() + " " + NudItemNum.Value.ToString() + " " + mItemList[index, 1].oContent.ToString().Trim());
                    itemCount += int.Parse(NudItemNum.Value.ToString());
                    listViewAddItem.EndUpdate();
                    return;
                }
                else
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_AddItem"));
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region �����͵����б���ɾ������
        private void btnDelList_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < listViewAddItem.Items.Count; i++)
                {
                    if (listViewAddItem.Items[i].Selected)
                    {
                        listViewAddItem.BeginUpdate();
                        hItemList.Remove(listViewAddItem.SelectedItems[0].SubItems[0].Text);
                        itemCount -= int.Parse(listViewAddItem.SelectedItems[0].SubItems[1].Text);
                        listViewAddItem.Items.Remove(listViewAddItem.SelectedItems[0]);
                        listViewAddItem.EndUpdate();
                        return;
                    }
                }
                MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_deleteItem"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion



        #region ��ӵ���
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCharName.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_inputName"));
                    return;
                }
                if (listViewAddItem.Items.Count == 0)
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_AddItem"));
                    return;
                }
                if (txtContent.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "AI_Hint_contentNull"));
                    return;
                }
                if (this.txtTitle.Text.Trim() == "")
                {
                    MessageBox.Show("�����뷢����!");
                    return;
                }

                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                string itemComb = null;

                foreach (DictionaryEntry dEnum in hItemList)
                {
                    itemComb += dEnum.Value.ToString();
                    itemComb += "|";
                }

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

                mContent[4].eName = CEnum.TagName.SD_ItemName;
                mContent[4].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[4].oContent = itemComb;

                mContent[5].eName = CEnum.TagName.SD_Title;
                mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[5].oContent = txtTitle.Text.Trim();

                mContent[6].eName = CEnum.TagName.SD_Content;
                mContent[6].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[6].oContent = txtContent.Text.Trim();

                mContent[7].eName = CEnum.TagName.f_pilot;
                mContent[7].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[7].oContent = userNick;

                backgroundWorkerAddItem.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region ��ӵ���backgroundWorker��Ϣ����
        private void backgroundWorkerAddItem_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_UserAdditem_Add, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ӵ���backgroundWorker��Ϣ����
        private void backgroundWorkerAddItem_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);

            ItemList();
            listViewAddItem.Clear();
            listViewAddItem.Columns.Add(config.ReadConfigValue("GLOBAL", "AI_UI_colItemName"), 232);
            listViewAddItem.Columns.Add(config.ReadConfigValue("GLOBAL", "AI_UI_colItemNum"), 65);
            NudItemNum.Value = 1;
            txtTitle.Clear();
            txtContent.Clear();
            hItemList.Clear();
            itemCount = 0;
        }
        #endregion



        #region ������Ϣ
        private void btnReset_Click(object sender, EventArgs e)
        {
            ItemList();
            listViewAddItem.Clear();
            listViewAddItem.Columns.Add(config.ReadConfigValue("MMagic", "AI_UI_colItemName"), 232);
            listViewAddItem.Columns.Add(config.ReadConfigValue("MMagic", "AI_UI_colItemNum"), 65);
            NudItemNum.Value = 1;
            txtTitle.Clear();
            txtContent.Clear();
            hItemList.Clear();
            itemCount = 0;
        }
        #endregion



        #region ����ر�
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        private void tbcResult_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chbSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (chbSearch.Checked == true)
            {
                //this.chbSearch.Text = config.ReadConfigValue("MMagic", "AI_UI_chbItemType");
                //this.lblItemType.Visible = false;
                //this.cmbItemType.Visible = false;
                this.lblSearch.Visible = true;
                this.txtSearch.Visible = true;
                this.txtSearch.Text = "";
                this.btnBluSearch.Visible = true;
                //this.cmbItemName.Items.Clear();
                //this.NudItemNum.Value = 1;
            }
            else
            {
                //this.chbSearch.Text = config.ReadConfigValue("MMagic", "AI_UI_chbBluSearch");
                //this.lblItemType.Visible = true;
                //this.cmbItemType.Visible = true;
                this.lblSearch.Visible = false;
                this.txtSearch.Visible = false;
                this.txtSearch.Text = "";
                this.btnBluSearch.Visible = false;
                //this.cmbItemName.Items.Clear();
                //this.NudItemNum.Value = 1;
            }
        }

        private void btnBluSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[3];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = serverIP;

                mContent[1].eName = CEnum.TagName.SD_Type;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = 2;

                mContent[2].eName = CEnum.TagName.SD_ItemName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = this.txtSearch.Text.ToString();

                backgroundWorkerBluSearch.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerBluSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_ItemList_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerBluSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            mItemList = mResult;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }

            cmbItemName = Operation_GD.BuildCombox(mResult, cmbItemName);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDlgBatch.Filter = "*.xls|*.*|�����ļ�|*.*";

            if (openFileDlgBatch.ShowDialog() == DialogResult.OK)
            {
                this.txtFilePath.Text = openFileDlgBatch.FileName;
            }
        }

        private void btnBatchAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtFilePath.Text.Trim().Length <= 0)
                {
                    MessageBox.Show(config.ReadConfigValue("MMagic", "AI_Hint_HintFilePath"));
                    return;
                }

                batchAddItem = null;

                string Path = this.txtFilePath.Text;
                DataSet ds = null;
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();//��connection����
                DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                string strExcel = "";
                OleDbDataAdapter myCommand = null;//����OLEDB������

                strExcel = "select * from [" + dt.Rows[0][2].ToString().Trim() + "]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);//COMMAND����
                ds = new DataSet();
                myCommand.Fill(ds, "counts");//���DataSet

                for (int i = 0; i < ds.Tables["counts"].Rows.Count; i++)
                {
                    //�½�DataRow��
                    //paramList.Add(ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim());
                    batchAddItem += ds.Tables[0].Rows[i].ItemArray[0].ToString().Trim();
                    batchAddItem += "|";
                    batchAddItem += ds.Tables[0].Rows[i].ItemArray[1].ToString().Trim();
                    batchAddItem += "|";
                    batchAddItem += ds.Tables[0].Rows[i].ItemArray[2].ToString().Trim();
                    //batchAddItem += "|";
                    //batchAddItem += ds.Tables[0].Rows[i].ItemArray[3].ToString().Trim();
                    batchAddItem += ",";
                }
                if (batchAddItem.Length <= 0)
                {
                    MessageBox.Show(config.ReadConfigValue("MMagic", "AI_Error_FilePath"));
                    return;
                }

                Cursor = Cursors.WaitCursor;
                this.GrpSearch.Enabled = false;
                this.tbcResult.Enabled = false;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[4];

                mContent[0].eName = CEnum.TagName.SD_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text.Trim());

                mContent[1].eName = CEnum.TagName.SD_ItemName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = batchAddItem.Trim();

                mContent[2].eName = CEnum.TagName.UserByID;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                mContent[3].eName = CEnum.TagName.SD_ServerName;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = cmbServer.Text.Trim();

                this.backgroundWorkerBatchAdd.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerBatchAdd_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_UserAdditem_Add_All, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerBatchAdd_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.GrpSearch.Enabled = true;
                this.tbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//�ı����״̬

                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

                Operation_GD.showResult(mResult);
                this.txtFilePath.Text = "";
                batchAddItem = null;
                initServerIP();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #region �õ���Ϸ�������б�
        private void initServerIP()
        {
            try
            {
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
    }
}