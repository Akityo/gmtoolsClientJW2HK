using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using C_Global;
using C_Event;

namespace M_Audition
{
    /// <summary>
    /// Frm_SDO_Part ��ժҪ˵����
    /// </summary>
    [C_Global.CModuleAttribute("�ʺŽ�/��ͣ", "Frm_AU_Close", "AU������ -- �ʺŽ�/��ͣ", "AU Group")]
    public partial class Frm_AU_Close : Form
    {
        private CEnum.Message_Body[,] mServerInfo = null;
        private CSocketEvent m_ClientEvent = null;
        private int iUserID = 0;
        private string strUserNick = null;

        private int iPageCount = 0;
        private bool bFirst = false;

        public Frm_AU_Close()
        {
            InitializeComponent();
        }

        #region �Զ�������¼�
        /// <summary>
        /// ��������еĴ���
        /// </summary>
        /// <param name="oParent">MDI ����ĸ�����</param>
        /// <param name="oSocket">Socket</param>
        /// <returns>����еĴ���</returns>
        public Form CreateModule(object oParent, object oEvent)
        {
            //������¼����
            Frm_AU_Close mModuleFrm = new Frm_AU_Close();
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

        private void Frm_SDO_Close_Load(object sender, EventArgs e)
        {
            CEnum.Message_Body[] mContent = new CEnum.Message_Body[2];
            mContent[0].eName = CEnum.TagName.ServerInfo_GameDBID;
            mContent[0].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[0].oContent = 2;

            mContent[1].eName = CEnum.TagName.ServerInfo_GameID;
            mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[1].oContent = m_ClientEvent.GetInfo("GameID_AU");

            mServerInfo = Operation_Audition.GetServerList(this.m_ClientEvent, mContent);

            CmbServer = Operation_Audition.BuildCombox(mServerInfo, CmbServer);

            LblUser.Text = "";
            PnlInput.Visible = true;
            GrdList.DataSource = null;
            PnlPage.Visible = false;
        }

        private void Btn_Search_Click(object sender, EventArgs e)
        {
            LblUser.Text = "";
            PnlInput.Visible = true;
            GrdList.DataSource = null;

            CmbPage.Items.Clear();
            TbcResult.SelectedTab = TpgList;

            CEnum.Message_Body[] mContent = new CEnum.Message_Body[3];

            mContent[0].eName = CEnum.TagName.AU_ServerIP;
            mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
            mContent[0].oContent = Operation_Audition.GetItemAddr(mServerInfo, CmbServer.Text);

            mContent[1].eName = CEnum.TagName.Index;
            mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[1].oContent = 1;

            mContent[2].eName = CEnum.TagName.PageSize;
            mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
            mContent[2].oContent = Operation_Audition.iPageSize;

            CEnum.Message_Body[,] mGetResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNTREMOTE_QUERY, mContent);

            //��������ʾ
            if (mGetResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
            {
                MessageBox.Show(mGetResult[0, 0].oContent.ToString());
                return;
            }

            Operation_Audition.BuildDataTable(this.m_ClientEvent, mGetResult, GrdList, out iPageCount);

            if (iPageCount <= 0)
            {
                PnlPage.Visible = false;
            }
            else
            {
                for (int i = 0; i < iPageCount; i++)
                {
                    CmbPage.Items.Add(i+1);
                }

                CmbPage.SelectedIndex = 0;
                bFirst = true;
                PnlPage.Visible = true;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrdList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //return;
            
            DataTable mTable = (DataTable)GrdList.DataSource;

            try
            {
                //iUserID = int.Parse(mTable.Rows[e.RowIndex][0].ToString());
                strUserNick = mTable.Rows[e.RowIndex][0].ToString();
                LblUser.Text = "׼�������ʺ�Ϊ��" + strUserNick;
                TxtNick.Text = strUserNick;
                TbcResult.SelectedTab = TpgClose;

                CEnum.Message_Body[] mMemo = new CEnum.Message_Body[3];

                mMemo[0].eName = CEnum.TagName.AU_ServerIP;
                mMemo[0].eTag = CEnum.TagFormat.TLV_STRING;
                mMemo[0].oContent = Operation_Audition.GetItemAddr(mServerInfo, CmbServer.Text);

                mMemo[1].eName = CEnum.TagName.AU_ACCOUNT;
                mMemo[1].eTag = CEnum.TagFormat.TLV_STRING;
                mMemo[1].oContent = strUserNick;

                mMemo[2].eName = CEnum.TagName.UserByID;
                mMemo[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mMemo[2].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                CEnum.Message_Body[,] mGetResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNTLOCAL_QUERY, mMemo);

                if (mGetResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                {
                    TxtMemo.Text = mGetResult[0, 0].oContent.ToString();
                    label2.Text = "����";
                }
                else
                {
                    label2.Text = mGetResult[0, 4].oContent.ToString();
                    TxtMemo.Text = mGetResult[0, 3].oContent.ToString();
                }


                if (strUserNick != null)
                {
                    PnlInput.Visible = true;
                }
            }
            catch
            {
                MessageBox.Show("������ѡ��һ���û���");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            strUserNick = TxtNick.Text.Trim();
            if (strUserNick != null)
            {
                if (MessageBox.Show("����ʺ�", "ȷ�Ͻ����û������", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CEnum.Message_Body[,] mResult = null;

                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[4];
                    mContent[0].eName = CEnum.TagName.AU_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = Operation_Audition.GetItemAddr(mServerInfo, CmbServer.Text);

                    mContent[1].eName = CEnum.TagName.AU_ACCOUNT;
                    mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[1].oContent = strUserNick;

                    mContent[2].eName = CEnum.TagName.UserByID;
                    mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[2].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                    mContent[3].eName = CEnum.TagName.AU_Reason;
                    mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[3].oContent = TxtContent.Text;

                    mResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNT_QUERY, mContent);

                    if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                    {
                        MessageBox.Show(mResult[0, 0].oContent.ToString());
                        return;
                    }

                    if (mResult[0, 0].eName == CEnum.TagName.Status)
                    {
                        MessageBox.Show(mResult[0, 0].oContent.ToString());
                        return;
                    }

                    mContent = new CEnum.Message_Body[5];
                    mContent[0].eName = CEnum.TagName.AU_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = Operation_Audition.GetItemAddr(mServerInfo, CmbServer.Text);

                    mContent[1].eName = CEnum.TagName.AU_UserNick;
                    mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[1].oContent = mResult[0, 2].oContent.ToString();

                    mContent[2].eName = CEnum.TagName.UserByID;
                    mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[2].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                    mContent[3].eName = CEnum.TagName.AU_Reason;
                    mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[3].oContent = TxtContent.Text;

                    mContent[4].eName = CEnum.TagName.AU_ACCOUNT;
                    mContent[4].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[4].oContent = strUserNick;

                    mResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNT_BANISHMENT_QUERY, mContent);

                    if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                    {
                        MessageBox.Show(mResult[0, 0].oContent.ToString());
                        return;
                    }

                    if (mResult[0, 0].eName == CEnum.TagName.AU_STOPSTATUS && mResult[0, 0].oContent.ToString() == "1")
                    {

                        mResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNT_OPEN, mContent);

                        if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                        {
                            MessageBox.Show(mResult[0, 0].oContent.ToString());
                            return;
                        }

                        if (mResult[0, 0].eName == CEnum.TagName.Status && mResult[0, 0].oContent.Equals("FAILURE"))
                        {
                            MessageBox.Show("����ʧ�ܣ�");
                        }
                        else
                        {
                            GrdList.DataSource = null;
                            MessageBox.Show("�����ɹ������ʺ��Ѿ��ɹ�����⣡");
                        }
                    }
                    else
                    {
                        MessageBox.Show("�����û�д���ͣ��״̬�����ʧ�ܣ�");
                    }

                    PnlInput.Visible = false;

                    mContent = new CEnum.Message_Body[3];

                    mContent[0].eName = CEnum.TagName.AU_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = Operation_Audition.GetItemAddr(mServerInfo, CmbServer.Text);

                    mContent[1].eName = CEnum.TagName.Index;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = 1;

                    mContent[2].eName = CEnum.TagName.PageSize;
                    mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[2].oContent = Operation_Audition.iPageSize;

                    CEnum.Message_Body[,] mGetResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNTREMOTE_QUERY, mContent);

                    //��������ʾ
                    if (mGetResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                    {
                        MessageBox.Show(mGetResult[0, 0].oContent.ToString());
                        return;
                    }

                    Operation_Audition.BuildDataTable(this.m_ClientEvent, mGetResult, GrdList, out iPageCount);

                    TxtAccount.Clear();
                    TxtContent.Clear();
                    TxtMemo.Clear();
                }
            }
            else
            {
                MessageBox.Show("��������ԭ��");
            }
        }

        private void BtnPost_Click(object sender, EventArgs e)
        {
            if (TxtContent.Text == "" || TxtContent.Text == null)
            {
                MessageBox.Show("�������ͣԭ��");
                return;
            }
            if (TxtAccount.Text.Trim().Length > 0)
            {
                if (MessageBox.Show("ͣ���ʺ�", "ȷ�Ͻ����û�ͣ����", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    CEnum.Message_Body[,] mResult = null;
                    
                    CEnum.Message_Body[] mContent = new CEnum.Message_Body[4];
                    mContent[0].eName = CEnum.TagName.AU_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = Operation_Audition.GetItemAddr(mServerInfo, CmbServer.Text);

                    mContent[1].eName = CEnum.TagName.AU_ACCOUNT;
                    mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[1].oContent = TxtAccount.Text;

                    mContent[2].eName = CEnum.TagName.UserByID;
                    mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[2].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                    mContent[3].eName = CEnum.TagName.AU_Reason;
                    mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[3].oContent = TxtContent.Text;

                    mResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNT_QUERY, mContent);

                    if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                    {
                        MessageBox.Show(mResult[0, 0].oContent.ToString());
                        return;
                    }

                    if (mResult[0, 0].eName == CEnum.TagName.Status)
                    {
                        MessageBox.Show(mResult[0, 0].oContent.ToString());
                        return;
                    }

                    mContent = new CEnum.Message_Body[5];
                    mContent[0].eName = CEnum.TagName.AU_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = Operation_Audition.GetItemAddr(mServerInfo, CmbServer.Text);

                    mContent[1].eName = CEnum.TagName.AU_UserNick;
                    mContent[1].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[1].oContent = mResult[0,2].oContent.ToString();

                    mContent[2].eName = CEnum.TagName.UserByID;
                    mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[2].oContent = int.Parse(m_ClientEvent.GetInfo("USERID").ToString());

                    mContent[3].eName = CEnum.TagName.AU_Reason;
                    mContent[3].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[3].oContent = TxtContent.Text;

                    mContent[4].eName = CEnum.TagName.AU_ACCOUNT;
                    mContent[4].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[4].oContent = TxtAccount.Text;

                    //������
                    mResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNT_BANISHMENT_QUERY, mContent);

                    if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                    {
                        MessageBox.Show(mResult[0, 0].oContent.ToString());
                        return;
                    }

                    if (mResult[0, 0].eName == CEnum.TagName.AU_STOPSTATUS && mResult[0, 0].oContent.ToString() == "0")
                    {
                        mResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNT_CLOSE, mContent);

                        if (mResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                        {
                            //MessageBox.Show("����ʧ�ܣ�");
                            MessageBox.Show(mResult[0, 0].oContent.ToString());
                            return;
                        }
                        if (mResult[0, 0].eName == CEnum.TagName.Status && mResult[0, 0].oContent.Equals("FAILURE"))
                        {
                            MessageBox.Show("��ͣ�ʺ�ʧ��,���Ժ��ԣ�");
                        }
                        else
                        {
                            MessageBox.Show("�����ɹ������ʺ��Ѿ��ɹ���ͣ�⣡");
                        }

                        TxtAccount.Text = "";
                        TxtContent.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("���ʺ��Ѿ���ͣ�⣬�����ٴδ���");
                    }

                    mContent = new CEnum.Message_Body[3];

                    mContent[0].eName = CEnum.TagName.AU_ServerIP;
                    mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                    mContent[0].oContent = Operation_Audition.GetItemAddr(mServerInfo, CmbServer.Text);

                    mContent[1].eName = CEnum.TagName.Index;
                    mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[1].oContent = 1;

                    mContent[2].eName = CEnum.TagName.PageSize;
                    mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                    mContent[2].oContent = Operation_Audition.iPageSize;

                    CEnum.Message_Body[,] mGetResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNTREMOTE_QUERY, mContent);

                    //��������ʾ
                    if (mGetResult[0, 0].eName == CEnum.TagName.ERROR_Msg)
                    {
                        MessageBox.Show(mGetResult[0, 0].oContent.ToString());
                        return;
                    }

                    Operation_Audition.BuildDataTable(this.m_ClientEvent, mGetResult, GrdList, out iPageCount);

                    TxtAccount.Clear();
                    TxtContent.Clear();
                    TxtMemo.Clear();
                }
            }
            else
            {
                MessageBox.Show("�������ʺ�!");
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            TxtAccount.Text = "";
            TxtContent.Text = "";
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            //TxtMemo.Text = "";
            TxtNick.Clear();
        }

        private void CmbPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bFirst)
            {
                CEnum.Message_Body[] mContent = new CEnum.Message_Body[3];

                mContent[0].eName = CEnum.TagName.AU_ServerIP;
                mContent[0].eTag = CEnum.TagFormat.TLV_STRING;
                mContent[0].oContent = Operation_Audition.GetItemAddr(mServerInfo, CmbServer.Text);

                mContent[1].eName = CEnum.TagName.Index;
                mContent[1].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[1].oContent = (int.Parse(CmbPage.Text) - 1) * Operation_Audition.iPageSize + 1;

                mContent[2].eName = CEnum.TagName.PageSize;
                mContent[2].eTag = CEnum.TagFormat.TLV_INTEGER;
                mContent[2].oContent = Operation_Audition.iPageSize;

                CEnum.Message_Body[,] mGetResult = Operation_Audition.GetResult(this.m_ClientEvent, CEnum.ServiceKey.AU_ACCOUNTREMOTE_QUERY, mContent);

                Operation_Audition.BuildDataTable(this.m_ClientEvent, mGetResult, GrdList, out iPageCount);
            }
        }
    }
}