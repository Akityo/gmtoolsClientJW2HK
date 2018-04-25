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
    [C_Global.CModuleAttribute("�����Ա��Ϣ", "FrmJW2FamilyInfoRead", "�����Ա��Ϣ", "JW2 Group")]
    public partial class FrmJW2FamilyInfoRead : Form
    {
        #region ����
        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;
        private CEnum.Message_Body[,] mResult = null;
        private CEnum.Message_Body[,] mItemResult = null;
        private int RolePage = 0;
        private int RoleIndex = 0;
        private int iIndexID = 0;
        private bool RoleFirst = false;
        private int iPageCount = 0;
        private bool bFirst = false;
        private int uid;
        private string gradename;
        private int currDgSelectRow;
        private bool pageRoleView;
        private bool pageFamilyMember;
        private bool pageApplicationMember;
        private bool pageBaseInfo;
        private bool pageBaseOrder;
        private bool pageFamilyPetInfo = false;
        private string FamilyID;
        DataTable dgTable = new DataTable();
        #endregion

        public FrmJW2FamilyInfoRead()
        {
            InitializeComponent();
        }

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
            this.GrpSearch.Text = config.ReadConfigValue("MSDO", "AF_UI_GrpSearch");
            this.LblServer.Text = config.ReadConfigValue("MSDO", "AF_UI_LblServer");
            this.BtnSearch.Text = config.ReadConfigValue("MSDO", "AF_UI_BtnSearch");

            this.GrpResult.Text = config.ReadConfigValue("MSDO", "AF_UI_GrpResult");

            this.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyInfoRead");

            LblAccount.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyName");
            button1.Text = config.ReadConfigValue("MJW2", "NEW_UI_button1");

            lblStoryState.Text= config.ReadConfigValue("MJW2", "UIC_UI_lblPage");

            label1.Text = config.ReadConfigValue("MJW2", "UIC_UI_lblPage");

            label2.Text = config.ReadConfigValue("MJW2", "UIC_UI_lblPage");


            label4.Text = config.ReadConfigValue("MJW2", "UIC_UI_lblPage");

            label5.Text = config.ReadConfigValue("MJW2", "UIC_UI_lblPage");
            lblFamilyPetInfo.Text = config.ReadConfigValue("MJW2", "UIC_UI_lblPage");


            tpgFamilyInfo.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyInfo");
            tpgFamilyMemberInfo.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyMemberInfo");

            tpgBaseOrder.Text = config.ReadConfigValue("MJW2", "NEW_UI_tpgBaseOrder");
            tpgBaseInfo.Text = config.ReadConfigValue("MJW2", "NEW_UI_tpgBaseInfo");

            tpgApplicationMemeber.Text = config.ReadConfigValue("MJW2", "NEW_UI_tpgApplicationMemeber");
            tpgFamilyPet.Text = config.ReadConfigValue("MJW2", "NEW_UI_tpgFamilyPet");
        }


        #endregion

        #region ��������
        /// <summary>
        /// ��������еĴ���
        /// </summary>
        /// <param name="oParent">MDI ����ĸ�����</param>
        /// <param name="oSocket">Socket</param>
        /// <returns>����еĴ���</returns>
        public Form CreateModule(object oParent, object oEvent)
        {
            //������¼����
            FrmJW2FamilyInfoRead mModuleFrm = new FrmJW2FamilyInfoRead();
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

        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmJW2FamilyInfoRead_Load(object sender, EventArgs e)
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
            catch
            { }
        }
        #endregion

        #region ����������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                mServerInfo = Operation_JW2.GetServerList(this.m_ClientEvent, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ����������б�backgroundWorker��Ϣ����
        private void backgroundWorkerFormLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CmbServer = Operation_JW2.BuildCombox(mServerInfo, CmbServer);
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text));
        }
        #endregion



        #region ��ѯ������Ϣ
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.tabControl1.SelectedIndex = 0;
                /*
                 * �����һ����ʾ������
                 */
                if (CmbServer.Text == "")
                {
                    MessageBox.Show(config.ReadConfigValue("MSDO", "LO_Code_Msg1"));
                    return;
                }

                BtnSearch.Enabled = false;
                Cursor = Cursors.WaitCursor;
                RoleInfoView.DataSource = null;

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = CmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_FAMILYNAME;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = this.lblAccountOrNick.Text.ToString();


              

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                backgroundWorkerSearch.RunWorkerAsync(mContent);


                tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text));

                //this.backgroundWorkerSearch.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(tmp_ClientEvent, CEnum.ServiceKey.JW2_FAMILYINFO_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ��ѯ������ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, dataGridView2, out iPageCount);
                }
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
            catch
            {

            }
        }
        #endregion



        #region �л���Ϸ������
        private void CmbServer_SelectedIndexChanged(object sender, EventArgs e)
        {
            tmp_ClientEvent = m_ClientEvent.GetSocket(m_ClientEvent, Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text));
        }
        #endregion



        #region �رմ���
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion



        #region ˫��������Ϣ��ѯ�����Ա��Ϣ
        private void RoleInfoView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.tabControl1.SelectedIndex = 1;
                //this.CmbPage.Items.Clear();
                if (e.RowIndex != -1)
                {
                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[2];


                    using (DataTable dt = (DataTable)RoleInfoView.DataSource)
                    {
                        uid = int.Parse(dt.Rows[e.RowIndex][0].ToString());//������Id
                    }

                    //��ѯ������
                    mContent[0].eName = CEnum.TagName.AU_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

                    mContent[1].eName = CEnum.TagName.AU_famid;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = uid;


                    this.backgroundWorkerFamilyMember.RunWorkerAsync(mContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region ˫��������Ϣ��ѯ�����Ա��ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerFamilyMember_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(tmp_ClientEvent, CEnum.ServiceKey.JW2_FAMILYMEMBER_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region ˫��������Ϣ��ѯ�����Ա��ϢbackgroundWorker��Ϣ����
        private void backgroundWorkerFamilyMember_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdFamilyMember, out iPageCount);
                }


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
                    this.pageFamilyMember = true;
                    this.pnlStoryState.Visible = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region �л���ҳ
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageRoleView)
            {
            
            CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

            mContent[0].eName = CEnum.TagName.JW2_ServerIP;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

            mContent[1].eName = CEnum.TagName.JW2_ServerName;
            mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[1].oContent = CmbServer.Text.ToString();

            mContent[2].eName = CEnum.TagName.JW2_FAMILYNAME;
            mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[2].oContent = this.lblAccountOrNick.Text.ToString();

            mContent[3].eName = CEnum.TagName.Index;
            mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[3].oContent = int.Parse(cmbRoleView.Text.ToString());

            mContent[4].eName = CEnum.TagName.PageSize;
            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[4].oContent = Operation_JW2.iPageSize;

            this.backgroundWorkerReSearch.RunWorkerAsync(mContent);
            }


        }
        #endregion

        #region backgroundWorker��ҳ������Ϣ
        private void backgroundWorkerReSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(tmp_ClientEvent, CEnum.ServiceKey.JW2_FAMILYINFO_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region backgroundworker��ҳ������Ϣ
        private void backgroundWorkerReSearch_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            BtnSearch.Enabled = true;
            Cursor = Cursors.Default;
            int pg = 0;
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }
            else
            {

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, dataGridView2, out iPageCount);
            }
        }
        #endregion

        #region ����ĳһ��Ԫ�񱣴�����
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            currDgSelectRow = e.RowIndex;
        }
        #endregion

        #region
        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (dataGridView2.DataSource != null)
                {
                    DataTable mTable = (DataTable)dataGridView2.DataSource;
                  
                    FamilyID = mTable.Rows[currDgSelectRow][0].ToString();//��������ʺ�ID
                  


                }
                if (tabControl1.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_tpgFamilyMemberInfo")))
                {
                    FamilyMemeber();//��ԃ����ɆT�YӍ
                }
                else if (tabControl1.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_tpgBaseInfo")))
                {
                    BaseInfo();//��ԃ�����YӍ

                }
                else if (tabControl1.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_tpgBaseOrder")))
                {
                    BaseOrder();//��ԃ��������
                }
                else if (tabControl1.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_tpgApplicationMemeber")))
                {
                    ApplicationMember();//��ԃ��Ո�гɆT
                }
                else if (tabControl1.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_tpgFamilyPet")))
                {
                    FamilyPetInfo();//��ԃ����ɆT�YӍ
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            
            }

        }
        #endregion

        private void FamilyPetInfo()
        {
            CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

            mContent[0].eName = CEnum.TagName.JW2_ServerIP;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

            mContent[1].eName = CEnum.TagName.JW2_ServerName;
            mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[1].oContent = CmbServer.Text.ToString();

            mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
            mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[2].oContent = int.Parse(FamilyID);

            mContent[3].eName = CEnum.TagName.Index;
            mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[3].oContent = 1;

            mContent[4].eName = CEnum.TagName.PageSize;
            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[4].oContent = Operation_JW2.iPageSize;

            this.backgroundWorkerFamilyPetInfo.RunWorkerAsync(mContent);
        }

        #region
        private void FamilyMemeber()
        {
            CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

            mContent[0].eName = CEnum.TagName.JW2_ServerIP;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

            mContent[1].eName = CEnum.TagName.JW2_ServerName;
            mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[1].oContent = CmbServer.Text.ToString();

            mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
            mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[2].oContent = int.Parse(FamilyID);

            mContent[3].eName = CEnum.TagName.Index;
            mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[3].oContent = 1;

            mContent[4].eName = CEnum.TagName.PageSize;
            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[4].oContent = Operation_JW2.iPageSize;

            backgroundWorkerFamilyMember.RunWorkerAsync(mContent);
        }
        #endregion

        #region
        private void BaseInfo()
        {
            try
            {
           
            CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

            mContent[0].eName = CEnum.TagName.JW2_ServerIP;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

            mContent[1].eName = CEnum.TagName.JW2_ServerName;
            mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[1].oContent = CmbServer.Text.ToString();

            mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
            mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[2].oContent = int.Parse(FamilyID);

            mContent[3].eName = CEnum.TagName.Index;
            mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[3].oContent = 1;

            mContent[4].eName = CEnum.TagName.PageSize;
            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[4].oContent = Operation_JW2.iPageSize;

            backgroundWorkerBaseInfo.RunWorkerAsync(mContent);
                 }
            catch (System.Exception ex)
            {
            	MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void BaseOrder()
        {
          try
            {
           
            CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

            mContent[0].eName = CEnum.TagName.JW2_ServerIP;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

            mContent[1].eName = CEnum.TagName.JW2_ServerName;
            mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[1].oContent = CmbServer.Text.ToString();

            mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
            mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[2].oContent = int.Parse(FamilyID);

            mContent[3].eName = CEnum.TagName.Index;
            mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[3].oContent = 1;

            mContent[4].eName = CEnum.TagName.PageSize;
            mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[4].oContent = Operation_JW2.iPageSize;

            this.backgroundWorkerBaseOrder.RunWorkerAsync(mContent);

                 }
            catch (System.Exception ex)
            {
            	MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void ApplicationMember()
        {
            try
            {
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = CmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = int.Parse(FamilyID);

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = 1;

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerApplicationMember.RunWorkerAsync(mContent);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
        #endregion

        #region
        private void backgroundWorkerReFamilyMember_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(tmp_ClientEvent, CEnum.ServiceKey.JW2_FAMILYMEMBER_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerReFamilyMember_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
          
            BtnSearch.Enabled = true;
            Cursor = Cursors.Default;
            int pg = 0;
            CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mResult[0, 0].oContent.ToString());
                return;
            }
            else
            {

                Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdFamilyMember, out iPageCount);
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }
        #endregion

        #region
    private void cmbStorySate_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
           
            if (pageFamilyMember)
            {
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = CmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = int.Parse(FamilyID);

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = int.Parse(cmbStorySate.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerReFamilyMember.RunWorkerAsync(mContent);
            }
        }
        catch (System.Exception ex)
        {
            MessageBox.Show(ex.ToString());
        }
    }
    #endregion

        #region
         private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
          {
            this.tabControl1.SelectedIndex = 1;
           }
         #endregion

        #region
        private void backgroundWorkerBaseInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_BasicInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerBaseInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdBaseInfo, out iPageCount);
                }


                if (iPageCount <= 1)
                {
                    this.pnlBaseInfo.Visible = false;
                }
                else
                {
                    for (int i = 0; i < iPageCount; i++)
                    {
                        this.cmbBaseInfo.Items.Add(i + 1);
                    }

                    this.cmbBaseInfo.SelectedIndex = 0;
                    this.pageBaseInfo = true;
                    this.pnlBaseInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void backgroundWorkerReBaseInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_BasicInfo_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerReBaseInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdBaseInfo, out iPageCount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void backgroundWorkerBaseOrder_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_BasicRank_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerBaseOrder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdBaseOrder, out iPageCount);
                }
                if (iPageCount <= 1)
                {
                    this.pnlBaseOrder.Visible = false;
                }
                else
                {
                    for (int i = 0; i < iPageCount; i++)
                    {
                        this.cmbBaseOrder.Items.Add(i + 1);
                    }

                    this.cmbBaseOrder.SelectedIndex = 0;
                    this.pageBaseOrder = true;
                    this.pnlBaseOrder.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void backgroundWorkerReBaseOrder_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_BasicRank_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerReBaseOrder_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdBaseOrder, out iPageCount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void backgroundWorkerApplicationMember_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_FamilyMember_Applying, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerApplicationMember_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdApplicationMember, out iPageCount);
                }


                if (iPageCount <= 1)
                {
                    this.pnlApplicationMember.Visible = false;
                }
                else
                {
                    for (int i = 0; i < iPageCount; i++)
                    {
                        this.cmbApplicationMember.Items.Add(i + 1);
                    }

                    this.cmbApplicationMember.SelectedIndex = 0;
                    this.pageApplicationMember = true;
                    this.pnlApplicationMember.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void backgroundWorkerReApplicationMember_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_FamilyMember_Applying, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerReApplicationMember_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdApplicationMember, out iPageCount);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void cmbBaseInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageBaseInfo)
            {
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = CmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = int.Parse(FamilyID);

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = int.Parse(this.cmbBaseInfo.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerReBaseInfo.RunWorkerAsync(mContent);
            }
        }
        #endregion

        #region
        private void cmbBaseOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageBaseOrder)
            {
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = CmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = int.Parse(FamilyID);

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = int.Parse(cmbBaseOrder.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerReBaseOrder.RunWorkerAsync(mContent);
            }
        }
        #endregion

        #region
        private void cmbApplicationMember_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageApplicationMember)
            {
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[5];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = CmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = int.Parse(FamilyID);

                mContent[3].eName = CEnum.TagName.Index;
                mContent[3].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[3].oContent = int.Parse(this.cmbApplicationMember.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerReApplicationMember.RunWorkerAsync(mContent);
            }
        }
        #endregion

        private void backgroundWorkerFamilyPetInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_FamilyPet_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerFamilyPetInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdFamilyPetInfo, out iPageCount);
                }


                if (iPageCount <= 1)
                {
                    this.pnlFamilyPetInfo.Visible = false;
                }
                else
                {
                    for (int i = 0; i < iPageCount; i++)
                    {
                        this.cmbFamilyPetInfo.Items.Add(i + 1);
                    }

                    this.cmbFamilyPetInfo.SelectedIndex = 0;
                    this.pageFamilyPetInfo = true;
                    this.pnlFamilyPetInfo.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void backgroundWorkerReFamilyPetInfo_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_FamilyPet_Query, (CEnum.Message_Body[])e.Argument);
            }
        }

        private void backgroundWorkerReFamilyPetInfo_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                BtnSearch.Enabled = true;
                Cursor = Cursors.Default;
                int pg = 0;
                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    return;
                }
                else
                {

                    Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdFamilyPetInfo, out iPageCount);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

    }
}