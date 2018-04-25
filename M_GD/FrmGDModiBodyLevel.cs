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
    [C_Global.CModuleAttribute("�û��������", "FrmGDModiBodyLevel", "�û��������", "GD Group")]
    public partial class FrmGDModiBodyLevel : Form
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

        string userBodyLevel;
        string userStrongLevel;
        int selectChar2 = 0;
        string BodyName;
        string bodyId;
        bool pageUnits = false;
        CEnum.Message_Body[,] mResult = null;
        #endregion

        public FrmGDModiBodyLevel()
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
            FrmGDModiBodyLevel mModuleFrm = new FrmGDModiBodyLevel();
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

            tbcResult.Enabled = true;
        }
        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDModiBodyLevel_Load(object sender, EventArgs e)
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

        private void backgroundWorkerModiBodyLevel_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_UpdateUnitsExp_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerModiBodyLevel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);

            this.tbcResult.SelectedIndex=1;
            //this.txtCharLvl.Text = "";
            this.NudNewLvl.SelectedIndex = 0;
        }

        private void btnModiLvl_Click(object sender, EventArgs e)
        {
            try
            {
                //if (this.txtAccount.Text.Trim() == "")
                //{
                //    MessageBox.Show(config.ReadConfigValue("MSD", "AM_Hint_inputName"));
                //    return;
                //}

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
                mContent[3].oContent = txtAccount.Text.Trim();
       
                mContent[4].eName = CEnum.TagName.SD_UnitLevelNumber;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;

                if (this.NudNewLvl.Text.ToString() == "1(����)")
                    mContent[4].oContent = 1;
                else if (this.NudNewLvl.Text.ToString() == "2(����)")
                    mContent[4].oContent = 2;
                else if (this.NudNewLvl.Text.ToString() == "3(�м�)")
                    mContent[4].oContent = 3;
                else if (this.NudNewLvl.Text.ToString() == "4(�߼�)")
                    mContent[4].oContent = 4;
                else if (this.NudNewLvl.Text.ToString() == "5(ǿ��1)")
                    mContent[4].oContent = 5;
                else if (this.NudNewLvl.Text.ToString() == "6(ǿ��2)")
                    mContent[4].oContent = 6;
                else if (this.NudNewLvl.Text.ToString() == "7(ǿ��3)")
                    mContent[4].oContent = 7;
                else if (this.NudNewLvl.Text.ToString() == "8(ǿ��4)")
                    mContent[4].oContent = 8;
        
                mContent[5].eName = CEnum.TagName.SD_CustomLvMax;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent =int.Parse(this.numericUpDown1.Text.ToString());

                mContent[6].eName = CEnum.TagName.SD_ItemID;
                mContent[6].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[6].oContent = int.Parse(BodyName);

                lock (typeof(C_Event.CSocketEvent))
                {
                    mResult = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_UpdateUnitsExp_Query, mContent);
                }

                this.GrpSearch.Enabled = true;
                this.tbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//�ı����״̬

                //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if(mResult[0,0].oContent.ToString()=="")
                {
                    MessageBox.Show("�û��岻���޸ĵȼ���");
                    //return;
                }
                else
                Operation_GD.showResult(mResult);

                //this.tbcResult.SelectedIndex = 1;
                this.btnSearch_Click(null,null);
                //this.txtCharLvl.Text = "";
                this.NudNewLvl.SelectedIndex = 0;
                //this.backgroundWorkerModiBodyLevel.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnResetLvl_Click(object sender, EventArgs e)
        {
            this.NudNewLvl.SelectedIndex = 0;
            this.numericUpDown1.SelectedIndex = 0;
        }

        private void tbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (GrdCharacter.DataSource != null)
                {
                    DataTable mTable = (DataTable)GrdCharacter.DataSource;
                    userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                    userName = mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "SD_UserName")].ToString();
              
                    if (tbcResult.SelectedTab.Text.Equals("��һ�����Ϣ"))
                    {
                        BodyInfo();//��ѯ������Ϣ
                    }
                    else if (tbcResult.SelectedTab.Text.Equals("�޸ĵȼ�"))
                    {
                        if (GrdUnits.DataSource != null)
                        {
                            DataTable mTable2 = (DataTable)GrdUnits.DataSource;
                            userBodyLevel = mTable2.Rows[selectChar2][7].ToString();
                            userStrongLevel = mTable2.Rows[selectChar2][13].ToString();
                            this.BodyName = mTable2.Rows[selectChar2][4].ToString();
                            this.bodyId = mTable2.Rows[selectChar2][5].ToString();
                            this.txtSkill.Text = this.bodyId;
                            this.NudNewLvl.Items.Clear();
                            this.NudNewLvl.Items.Add("1(����)");
                            this.NudNewLvl.Items.Add("2(����)");
                            this.NudNewLvl.Items.Add("3(�м�)");
                            this.NudNewLvl.Items.Add("4(�߼�)");
                            this.NudNewLvl.Items.Add("5(ǿ��1)");
                            this.NudNewLvl.Items.Add("6(ǿ��2)");
                            this.NudNewLvl.Items.Add("7(ǿ��3)");
                            this.NudNewLvl.Items.Add("8(ǿ��4)");
                            this.NudNewLvl.SelectedIndex = 0;
                            this.numericUpDown1.Items.Clear();
                            this.numericUpDown1.Items.Add("2");
                            this.numericUpDown1.Items.Add("3");
                            this.numericUpDown1.Items.Add("4");
                            this.numericUpDown1.SelectedIndex = 0;
                            this.txtCurLvl.Text = userBodyLevel;
                            this.txtStrongLevel.Text = userStrongLevel;


                        }
                    }

                    //else if (tbcResult.SelectedTab.Text.Equals("������ϸ��Ϣ"))
                    //{
                    //    UserUnitsDetail();//��ѯ������ϸ��Ϣ

                    //}
                }
             
                else
                {
                    GrdUnits.DataSource = null;
                    pnlUnits.Visible = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BodyInfo()
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
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtAccount.Text.Trim() == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSD", "AM_Hint_inputName"));
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
                mContent[3].oContent = txtAccount.Text.Trim();

                mContent[4].eName = CEnum.TagName.f_level;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = 20;

                mContent[5].eName = CEnum.TagName.SD_ServerName;
                mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[5].oContent = this.cmbServer.Text.ToString();

                this.backgroundWorkerModiStrongLevel.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void backgroundWorkerModiStrongLevel_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Account_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerModiStrongLevel_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.GrpSearch.Enabled = true;
            this.tbcResult.Enabled = true;
            this.Cursor = Cursors.Default;//�ı����״̬

            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

            Operation_GD.showResult(mResult);

            this.tbcResult.SelectedIndex = 1;
            //this.txtCharLvl.Text = "";
            this.NudNewLvl.SelectedIndex = 0;
            this.btnModiLvl.Enabled = true;
            this.NudNewLvl.Enabled = true;
            this.btnResetLvl.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
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
                        tbcResult.SelectedTab = this.tpgUserUnits;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorkerUnits_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserUnits_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

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

        private void backgroundWorkerReUnits_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_Item_UserUnits_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

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

        private void GrdUnits_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1)//˫����������
                {
                    selectChar2 = int.Parse(e.RowIndex.ToString());//������
                    if (GrdCharacter.DataSource != null)
                    {
                        tbcResult.SelectedTab = this.tpgModiBodyLevel;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tpgModiBodyLevel_Click(object sender, EventArgs e)
        {

        }

       
    }
}