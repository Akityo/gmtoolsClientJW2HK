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
    [C_Global.CModuleAttribute("GM�ʺŹ���", "FrmGDGmManage", "GM�ʺŹ���", "GD Group")]
    public partial class FrmGDGmManage : Form
    {
        #region ����

        private CEnum.Message_Body[,] mServerInfo = null;
        private CEnum.Message_Body[,] mResult= null;
        private CSocketEvent m_ClientEvent = null;
        private CSocketEvent tmp_ClientEvent = null;

        private int iPageCount = 0;//��ҳҳ��

        int userID = 0;
        private string userNick;

        string serverIP = null;
        string userName = null;
        string userNick2 = null;
        int selectChar = 0;   //GrdCharacter�е�ǰѡ�е��� 
        int selectItem = 0;
        //private string userName = null;
        string itemID = null;
       
        #endregion

        public FrmGDGmManage()
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
            FrmGDGmManage mModuleFrm = new FrmGDGmManage();
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
            tbcResult.Enabled = true;
        }
        #endregion



        #region ��½�������(�õ���Ϸ�������б�)
        private void FrmGDGmManage_Load(object sender, EventArgs e)
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
                this.tbcResult.SelectedIndex = 0;
                //if (cmbServer.Text == "")
                //{
                //    MessageBox.Show(config.ReadConfigValue("MSD", "UIA_Hint_selectServer"));
                //    return;
                //}

                serverIP = Operation_GD.GetItemAddr(mServerInfo, cmbServer.Text);
                userName = txtAccount.Text.Trim();
                userNick = txtNick.Text.Trim();

                //if (txtAccount.Text.Trim().Length > 0 || txtNick.Text.Trim().Length > 0)
                //{
                    this.GrpSearch.Enabled = false;
                    this.tbcResult.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;//�ı����״̬

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[3];
                    //��ѯ��ҽ�ɫ��Ϣ
                    mContent[0].eName = CEnum.TagName.SD_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = serverIP;

                    if (this.txtAccount.Text.ToString() == "")
                    {
                        mContent[1].eName = CEnum.TagName.SD_UserName;
                        mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                        mContent[1].oContent = "";

                        mContent[2].eName = CEnum.TagName.SD_Type;
                        mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                        mContent[2].oContent = 1;
                    }
                    else
                    {
                        mContent[1].eName = CEnum.TagName.SD_UserName;
                        mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                        mContent[1].oContent = userName;

                        mContent[2].eName = CEnum.TagName.SD_Type;
                        mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                        mContent[2].oContent =2;
                    
                    
                    
                    }

                    backgroundWorkerSearch.RunWorkerAsync(mContent);
                //}
                //else
                //{
                //    MessageBox.Show(config.ReadConfigValue("MMagic", "UIA_Hint_inPutAccont"));
                //    return;
                //}
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
                e.Result = Operation_GD.GetResult(tmp_ClientEvent, CEnum.ServiceKey.SD_GetGmAccount_Query, (CEnum.Message_Body[])e.Argument);
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
                    this.tbcResult.SelectedIndex = 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtNewAccount.Text.Trim() == "")
                {
                    MessageBox.Show("�µ�GM�ʺ�������Ϊ�գ�");
                    return;
                }
                //if (this.txtCurAccount.Text.Trim()==this.txtNewAccount.Text.Trim())
                //{
                //    MessageBox.Show("�µ�GM�ʺ��������뵱ǰ��GM�ʺ�����ͬ��");
                //    return;
                //}
                if(this.txtPassword.Text.ToString()=="")
                {
                    MessageBox.Show("�µ�GM�ʺ����벻��Ϊ�գ�");
                    return;
                }
                if(this.txtPassword.Text.ToString()!=this.txtRepPass.Text.ToString())
                {
                    MessageBox.Show("�µ�GM�ʺ������������ʺ����벻ͬ��");
                    return;
                }
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

                mContent[5].eName = CEnum.TagName.f_idx;
                mContent[5].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[5].oContent = userID;

                mContent[2].eName = CEnum.TagName.SD_UserName;
                mContent[2].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[2].oContent = this.txtNewAccount.Text.Trim();

                mContent[3].eName = CEnum.TagName.SD_UserName_Old;
                mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[3].oContent = txtCurAccount.Text.ToString();

                mContent[4].eName = CEnum.TagName.SD_passWd;
                mContent[4].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[4].oContent = txtPassword.Text.ToString();

                mContent[6].eName = CEnum.TagName.f_pilot;
                mContent[6].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[6].oContent = textBox1.Text.ToString();

             

                lock (typeof(C_Event.CSocketEvent))
                {
                    mResult = Operation_GD.GetResult(m_ClientEvent, CEnum.ServiceKey.SD_UpdateGmAccount_Query, mContent);
                }

                this.GrpSearch.Enabled = true;
                this.tbcResult.Enabled = true;
                this.Cursor = Cursors.Default;//�ı����״̬

                //CEnum.Message_Body[,] mResult = (CEnum.Message_Body[,])e.Result;
                if (mResult[0, 0].oContent.ToString() == "SCUESS")
                {
                    Operation_GD.showResult(mResult);
               
                    //return;
                }
                else
                    MessageBox.Show("�޸�ʧ��");

                //this.tbcResult.SelectedIndex = 1;
                this.btnSearch_Click(null, null);
                //this.txtCharLvl.Text = "";
                //this.NudNewLvl.SelectedIndex = 0;
                //this.backgroundWorkerModiBodyLevel.RunWorkerAsync(mContent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.txtNewAccount.Text = "";
            this.txtPassword.Text = "";
            this.txtRepPass.Text = "";
            textBox1.Text = "";
        }

        private void tbcResult_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                if (GrdCharacter.DataSource != null)
                {
                    DataTable mTable = (DataTable)GrdCharacter.DataSource;
                    userID = int.Parse(mTable.Rows[selectChar][config.ReadConfigValue("GLOBAL", "f_idx")].ToString());//��������ʺ�ID
                    userName = mTable.Rows[selectChar][1].ToString();
                    userNick2 = mTable.Rows[selectChar][2].ToString();
                    //if (tbcResult.SelectedTab.Text.Equals("������Ϣ"))
                    //{
                    //    FriendQuery();//��ѯ������Ϣ
                    //}
                    this.txtCurAccount.Text = userName;
                    this.textBox2.Text = userNick2;
                    
                }
                else
                {
                    //this.GrdFriend.DataSource = null;
                    //this.pnlFriend.Visible = false;


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}