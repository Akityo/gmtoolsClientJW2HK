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

using System.IO;
using System.Globalization;
namespace M_JW2
{
    [C_Global.CModuleAttribute("���������޸�", "FrmJW2FamilyInfo", "���������޸�", "JW2 Group")]
    public partial class FrmJW2FamilyInfo : Form
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
        private string FamilyID;
        private string userFamilyName;
        private bool pageFamilyPetInfo = false;
        DataTable dgTable = new DataTable();
        #endregion

        public FrmJW2FamilyInfo()
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

            this.LblAccount.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyName");
            this.label3.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyInfoTip");
            this.button1.Text = config.ReadConfigValue("MJW2", "NEW_UI_button1");
            this.tabPage1.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyNameModi");
            this.tpgFamilyPet.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyPetInfo");

            label1.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");

            lblFamilyPetInfo.Text = config.ReadConfigValue("MJW2", "NEW_UI_lblRoleView");

            label7.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyName");
            label6.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyNameNow");

            button3.Text = config.ReadConfigValue("MJW2", "NEW_UI_Commit");

            button2.Text = config.ReadConfigValue("MJW2", "BP_UI_btnReset");
            this.Text = config.ReadConfigValue("MJW2", "NEW_UI_FamilyInfo");
         
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
            FrmJW2FamilyInfo mModuleFrm = new FrmJW2FamilyInfo();
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
        private void FrmJW2FamilyInfo_Load(object sender, EventArgs e)
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
                this.tabControl1.TabPages.Remove(tpgFamilyPet);

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
                dataGridView2.DataSource = null;
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
            //try
            //{
            //    BtnSearch.Enabled = true;
            //    Cursor = Cursors.Default;
            //    int pg = 0;
            //    CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
            //    if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            //    {
            //        MessageBox.Show(mResult[0, 0].oContent.ToString());
            //        return;
            //    }
            //    else
            //    {

            //        Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdFamilyMember, out iPageCount);
            //    }


            //    if (iPageCount <= 1)
            //    {
            //        this.pnlStoryState.Visible = false;
            //    }
            //    else
            //    {
            //        for (int i = 0; i < iPageCount; i++)
            //        {
            //            this.cmbStorySate.Items.Add(i + 1);
            //        }

            //        this.cmbStorySate.SelectedIndex = 0;
            //        this.pageFamilyMember = true;
            //        this.pnlStoryState.Visible = true;
            //    }
            //}
            //catch(Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
        }
        #endregion

        #region
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

        #region
        private void backgroundWorkerReSearch_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(tmp_ClientEvent, CEnum.ServiceKey.JW2_FAMILYINFO_QUERY, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
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

        #region
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
                    //serverIP = Operation_JW2.GetItemAddr(mServerInfo, CmbServer.Text);
                    FamilyID = mTable.Rows[currDgSelectRow][0].ToString();//��������ʺ�ID
                    userFamilyName = mTable.Rows[currDgSelectRow][1].ToString();//��������ʺ�ID
                    textBox1.Text = userFamilyName;

                }
                if (tabControl1.SelectedTab.Text.Equals(config.ReadConfigValue("MJW2", "NEW_UI_FamilyPetInfo")))
                {
                    FamilyPetInfo();//��ѯ�����Ա��Ϣ
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
                //mContent[3].oContent = int.Parse(cmbStorySate.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerReFamilyMember.RunWorkerAsync(mContent);
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

                    //Operation_JW2.BuildDataTable(this.m_ClientEvent, mResult, GrdApplicationMember, out iPageCount);
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
                //mContent[3].oContent = int.Parse(this.cmbBaseInfo.Text.ToString());

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
                //mContent[3].oContent = int.Parse(cmbBaseOrder.Text.ToString());

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
                //mContent[3].oContent = int.Parse(this.cmbApplicationMember.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerReApplicationMember.RunWorkerAsync(mContent);
            }
        }
        #endregion


        #region
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text.Trim().Length <= 0)
                {
                    MessageBox.Show(config.ReadConfigValue("MJW2", "NEW_UI_PleaseSelectRoleFirest"));


                    return;
                }
                this.GrpSearch.Enabled = false;
                this.tabControl1.Enabled = false;
                this.Cursor = Cursors.WaitCursor;//�ı����״̬

                CEnum.Message_Body[] mContent = new CEnum.Message_Body[6];

                mContent[0].eName = CEnum.TagName.JW2_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_JW2.GetItemAddr(mServerInfo, this.CmbServer.Text);

                mContent[1].eName = CEnum.TagName.JW2_ServerName;
                mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[1].oContent = this.CmbServer.Text.ToString();

                mContent[2].eName = CEnum.TagName.JW2_FAMILYID;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = int.Parse(FamilyID);

                mContent[3].eName = CEnum.TagName.JW2_OLD_FAMILYNAME;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = userFamilyName;

                mContent[4].eName = CEnum.TagName.UserByID;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                mContent[5].eName = CEnum.TagName.JW2_FAMILYNAME;
                mContent[5].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[5].oContent = textBox2.Text.ToString();

                this.backgroundWorkerModiFamilyName.RunWorkerAsync(mContent);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region
        private void backgroundWorkerModiFamilyName_DoWork(object sender, DoWorkEventArgs e)
        {
            lock (typeof(C_Event.CSocketEvent))
            {
                e.Result = Operation_JW2.GetResult(m_ClientEvent, CEnum.ServiceKey.JW2_UpDateFamilyName_Query, (CEnum.Message_Body[])e.Argument);
            }
        }
        #endregion

        #region
        private void backgroundWorkerModiFamilyName_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                this.GrpSearch.Enabled = true;
                this.tabControl1.Enabled = true;
                this.Cursor = Cursors.Default;//�ı����״̬

                CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;

                if (mResult[0, 0].oContent.ToString().Trim() == "SCUESS")
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    FileStream fs = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\errLog\\log.txt", FileMode.Append);
                    StreamWriter streamWriter = new StreamWriter(fs);
                    streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                    streamWriter.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + mResult[0, 0].oContent.ToString());
                    streamWriter.Flush();
                    fs.Close();
                    //Operation_JW2.errLog.WriteLog("���������޸ĳɹ�");
                }
                else if (mResult[0, 0].oContent.ToString().Trim() == "FAILURE")
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString());
                    FileStream fs = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\errLog\\log.txt", FileMode.Append);
                    StreamWriter streamWriter = new StreamWriter(fs);
                    streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                    streamWriter.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + mResult[0, 0].oContent.ToString());
                    streamWriter.Flush();
                    fs.Close();
                    //Operation_JW2.errLog.WriteLog("���������޸�ʧ��");
                }
                else
                {
                    MessageBox.Show(mResult[0, 0].oContent.ToString().Trim());

                    FileStream fs = new FileStream(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\errLog\\log.txt", FileMode.Append);
                    StreamWriter streamWriter = new StreamWriter(fs);
                    streamWriter.BaseStream.Seek(0, SeekOrigin.End);
                    streamWriter.WriteLine(DateTime.Now.ToString(CultureInfo.CurrentCulture) + mResult[0, 0].oContent.ToString());
                    streamWriter.Flush();
                    fs.Close();
                }
                this.BtnSearch_Click(null, null);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion

        #region
        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Text = "";
        }
        #endregion

        private void cmbFamilyItemBuyLog_SelectedIndexChanged(object sender, EventArgs e)
        {
          


        }

        private void cmbFamilyPetInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pageFamilyPetInfo)
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
                mContent[3].oContent = int.Parse(cmbFamilyPetInfo.Text.ToString());

                mContent[4].eName = CEnum.TagName.PageSize;
                mContent[4].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[4].oContent = Operation_JW2.iPageSize;

                this.backgroundWorkerReFamilyPetInfo.RunWorkerAsync(mContent);
            }
        }

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