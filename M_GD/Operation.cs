using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using C_Global;
using C_Event;
using Language;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace M_GD
{
    class Operation_GD
    {
        public CSocketEvent m_ClientEvent = null;
        public static int iPageSize = 50;

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
        }

        #endregion

        #region ��ȡ��������ַ�б�
        /// <summary>
        /// ��ȡ��������ַ�б�
        /// </summary>
        /// <param name="mEvent">Socket�¼�</param>
        /// <param name="mContent">��Ϣ����</param>
        /// <returns>������</returns>
        public static CEnum.Message_Body[,] GetServerList(CSocketEvent mEvent, CEnum.Message_Body[] mContent)
        {
            CEnum.Message_Body[,] mReturn = null;

            mReturn =
                mEvent.RequestResult(CEnum.ServiceKey.SERVERINFO_IP_QUERY, CEnum.Msg_Category.COMMON, mContent);

            return mReturn;
        }
        #endregion

        #region ��ȡ��Ϣ�б�
        /// <summary>
        /// ��ȡ��Ϣ�б�
        /// </summary>
        /// <param name="mEvent">Socket�¼�</param>
        /// <param name="mKey">������</param>
        /// <param name="mContent">��Ϣ����</param>
        /// <returns>������</returns>
        public static CEnum.Message_Body[,] GetResult(CSocketEvent mEvent, CEnum.ServiceKey mKey, CEnum.Message_Body[] mContent)
        {
            CEnum.Message_Body[,] mReturn = null;

            mReturn =
                mEvent.RequestResult(mKey, CEnum.Msg_Category.SD_ADMIN, mContent);

            return mReturn;
        }
        #endregion

        #region ���� ComboBox ����
        /// <summary>
        /// ���� ComboBox ����
        /// </summary>
        /// <param name="val">ComboBox ����</param>
        /// <param name="mCmbBox">ComboBox �ؼ�</param>
        /// <returns>ComboBox �ؼ�</returns>
        public static System.Windows.Forms.ComboBox BuildCombox(CEnum.Message_Body[,] val, System.Windows.Forms.ComboBox mCmbBox)
        {
            try
            {
                mCmbBox.Items.Clear();
                for (int i = 0; i < val.GetLength(0); i++)
                {
                    mCmbBox.Items.Add(val[i, 1].oContent);
                }

                mCmbBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return mCmbBox;
        }

        public static System.Windows.Forms.ComboBox BuildComboxs(CEnum.Message_Body[,] val, System.Windows.Forms.ComboBox mCmbBox)
        {
            try
            {
                mCmbBox.Items.Clear();
                for (int i = 0; i < val.GetLength(0); i++)
                {
                    mCmbBox.Items.Add(val[i, 1].oContent);
                }

                //mCmbBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return mCmbBox;
        }

        public static System.Windows.Forms.ComboBox BuildComboxAno(CEnum.Message_Body[,] val, System.Windows.Forms.ComboBox mCmbBox)
        {
            try
            {
                mCmbBox.Items.Clear();
                for (int i = 1; i < val.GetLength(0); i++)
                {
                    mCmbBox.Items.Add(val[i, 1].oContent);
                }

                mCmbBox.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return mCmbBox;
        }

        #endregion

        #region ���� DataGridView����
        public static DataTable BuildDataTable(CSocketEvent mEvent, CEnum.Message_Body[,] val, System.Windows.Forms.DataGridView mGrid, out int PageCount)
        {
            ConfigValue config = (ConfigValue)mEvent.GetInfo("INI");

            DataTable mDataTable = BuildColumn(mEvent, val);

            mGrid.DataSource = BuildRow(mEvent, val, mDataTable, out PageCount);

            return null;
        }
        #endregion

        #region ���� DataGridView��
        /// <summary>
        /// ���� DataGridView ��
        /// </summary>
        /// <param name="val">����</param>
        /// <param name="mDataTable">DataTable</param>
        /// <returns>DataTable</returns>
        private static DataTable BuildColumn(CSocketEvent mEvent, CEnum.Message_Body[,] val)
        {
            DataTable mDataTable = new DataTable();
            try
            {

                ConfigValue config = (ConfigValue)mEvent.GetInfo("INI");

                for (int i = 0; i < val.GetLength(1); i++)
                {
                    if (val[0, i].eName != CEnum.TagName.PageCount)
                    {
                        string middle = (string)config.ReadConfigValue("GLOBAL", val[0, i].eName.ToString());
                        mDataTable.Columns.Add(middle, typeof(string));

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return mDataTable;
        }
        #endregion

        #region ���� DataGridView��
        /// <summary>
        /// ���� DataGridView ��
        /// </summary>
        /// <param name="val">����</param>
        /// <param name="mDataRow">DataRow</param>
        /// <returns>DataRow</returns>
        private static DataTable BuildRow(CSocketEvent mEvent, CEnum.Message_Body[,] m_val, DataTable mTable, out int PageCount)
        {
            try
            {
                PageCount = 0;
                ConfigValue config = (ConfigValue)mEvent.GetInfo("INI");
                CEnum.Message_Body[,] val = m_val;
                for (int i = 0; i < val.GetLength(0); i++)
                {
                    DataRow mRow = mTable.NewRow();

                    for (int j = 0; j < val.GetLength(1); j++)
                    {
                        if (val[i, j].eName != CEnum.TagName.PageCount)
                        {
                            //if (val[i, j].eName == CEnum.TagName.Magic_Sex)
                            //{
                            //    val[i, j].oContent = val[i, j].oContent.ToString() == "0" ? config.ReadConfigValue("MLord", "UIC_Code_male") : config.ReadConfigValue("MLord", "UIC_Code_female");
                            //}
                            if (val[i, j].eName == CEnum.TagName.SD_N12)
                            {
                                if (val[i, j].oContent.ToString() == "0")
                                {
                                    val[i, j].oContent = "G��";
                                }
                                else if (val[i, j].oContent.ToString() == "1")
                                {
                                    val[i, j].oContent = "M��";
                                }
                            }
                            if (val[i, j].eName == CEnum.TagName.SD_ItemName)
                            {
                                if (val[i, j].oContent.ToString() == "")
                                {
                                    val[i, j].oContent = "�޵���";
                                }
                                else
                                {
                                    mRow[(string)config.ReadConfigValue("GLOBAL", val[i, j].eName.ToString())] = val[i, j].oContent.ToString();
                                }
                            }
                            if (val[i, j].eName == CEnum.TagName.SD_ItemName1)
                            {
                                if (val[i, j].oContent.ToString() == "")
                                {
                                    val[i, j].oContent = "�޵���";
                                }
                                else
                                {
                                    mRow[(string)config.ReadConfigValue("GLOBAL", val[i, j].eName.ToString())] = val[i, j].oContent.ToString();
                                }
                            }
                            if (val[i, j].eName == CEnum.TagName.SD_ItemName2)
                            {
                                if (val[i, j].oContent.ToString() == "")
                                {
                                    val[i, j].oContent = "�޵���";
                                }
                                else
                                {
                                    mRow[(string)config.ReadConfigValue("GLOBAL", val[i, j].eName.ToString())] = val[i, j].oContent.ToString();
                                }
                            }
                            if (val[i, j].eName == CEnum.TagName.SD_ItemName3)
                            {
                                if (val[i, j].oContent.ToString() == "")
                                {
                                    val[i, j].oContent = "�޵���";
                                }
                                else
                                {
                                    mRow[(string)config.ReadConfigValue("GLOBAL", val[i, j].eName.ToString())] = val[i, j].oContent.ToString();
                                }
                            }
                            if (val[i, j].eName == CEnum.TagName.SD_Type)
                            {
                                if (val[i, j].oContent.ToString() == "0")
                                {
                                    val[i, j].oContent = config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeCheck");
                                }
                                if (val[i, j].oContent.ToString() == "1")
                                {
                                    val[i, j].oContent = config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeCommon");
                                }
                                if (val[i, j].oContent.ToString() == "2")
                                {
                                    val[i, j].oContent = config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeEvent");
                                }
                                if (val[i, j].oContent.ToString() == "3")
                                {
                                    val[i, j].oContent = config.ReadConfigValue("GLOBAL", "NM_UI_cmbNoticeSystem");
                                }
                            }
                            if (val[i, j].eName == CEnum.TagName.SD_BuyType)
                            {
                                if (val[i, j].oContent.ToString() == "0")
                                {
                                    val[i, j].oContent = config.ReadConfigValue("GLOBAL", "NM_UI_cmbBuyGet");
                                }
                                if (val[i, j].oContent.ToString() == "1")
                                {
                                    val[i, j].oContent = config.ReadConfigValue("GLOBAL", "NM_UI_cmbComboxGet");
                                }
                                if (val[i, j].oContent.ToString() == "99")
                                {
                                    val[i, j].oContent = config.ReadConfigValue("GLOBAL", "NM_UI_cmbOfficeSend");
                                }
                            }
                            if (val[i, j].eName == CEnum.TagName.SD_Star)
                            {
                                if (val[i, j].oContent.ToString() == "2")
                                {
                                    val[i, j].oContent = "C��";
                                }
                                if (val[i, j].oContent.ToString() == "4")
                                {
                                    val[i, j].oContent = "A��";
                                }
                                if (val[i, j].oContent.ToString() == "5")
                                {
                                    val[i, j].oContent = "S��";
                                }
                                if (val[i, j].oContent.ToString() == "3")
                                {
                                    val[i, j].oContent = "B��";
                                }
                            }
                             
                            if (val[i, j].eName == CEnum.TagName.f_gender)
                            {
                                if (val[i, j].oContent.ToString() == "1")
                                {
                                    val[i, j].oContent ="��";
                                }
                                else if(val[i, j].oContent.ToString() == "0")
                                {
                                    val[i, j].oContent = "Ů";
                                }
                            }
                   
                            //if (val[i, j].eName == CEnum.TagName.SD_Type)
                            //{
                            //    if (val[i, j].oContent.ToString() == "0")
                            //    {
                            //        val[i, j].oContent = "point";
                            //    }
                            //    if (val[i, j].oContent.ToString() == "1")
                            //    {
                            //        val[i, j].oContent = "cash";
                            //    }
                            //}
                            if (val[i, j].eName == CEnum.TagName.SD_Status)
                            {
                                if (val[i, j].oContent.ToString() == "0")
                                {
                                    val[i, j].oContent = "δ����";
                                }
                                if (val[i, j].oContent.ToString() == "1")
                                {
                                    val[i, j].oContent = "�ѷ���";
                                }
                                if (val[i, j].oContent.ToString() == "2")
                                {
                                    val[i, j].oContent = "������";
                                }
                            }
                            //if (val[i, j].eName == CEnum.TagName.Magic_GuildID)
                            //{
                            //    if (val[i, j].oContent.ToString() == "0")
                            //    {
                            //        val[i, j].oContent = config.ReadConfigValue("MLord", "UIC_Code_NoGuild");
                            //    }
                            //    else
                            //    {
                            //        mRow[(string)config.ReadConfigValue("GLOBAL", val[i, j].eName.ToString())] = val[i, j].oContent.ToString();
                            //    }
                            //}
                            if (val[i, j].oContent == null)
                            {
                                mRow[(string)config.ReadConfigValue("GLOBAL", val[i, j].eName.ToString())] = "N/A";
                            }
                            else
                            {
                                mRow[(string)config.ReadConfigValue("GLOBAL", val[i, j].eName.ToString())] = val[i, j].oContent.ToString();
                            }
                        }
                        else
                        {
                            PageCount = int.Parse(val[i, j].oContent.ToString());
                        }
                    }

                    mTable.Rows.Add(mRow);

                }

                return mTable;
            }
            catch (Exception ex)
            {
                PageCount = 0;
                MessageBox.Show(ex.Message);
                return null;
            }

        }
        #endregion

        #region ��ȡ ComboBox ѡ��������
        /// <summary>
        /// ��ȡ ComboBox ѡ��������
        /// </summary>
        /// <param name="val">��¼��</param>
        /// <param name="Condition">��������</param>
        /// <returns></returns>
        public static string GetItemAddr(CEnum.Message_Body[,] val, string Condition)
        {
            string strResult = null;

            for (int i = 0; i < val.GetLength(0); i++)
            {
                for (int j = 0; j < val.GetLength(1); j++)
                {
                    if (val[i, j].eName == CEnum.TagName.ServerInfo_IP && val[i, j + 1].oContent.Equals(Condition))
                    {
                        strResult = val[i, j].oContent.ToString();
                    }
                }
            }

            return strResult;
        }
        #endregion

        #region ����ComboBoxѡ�����ȡ����ID
        /// <summary>
        /// ����ComboBoxѡ�����ȡ����ID
        /// </summary>
        /// <param name="val">��¼��</param>
        /// <param name="Condition">��������</param>
        /// <returns></returns>
        public static string GetContent(CEnum.Message_Body[,] val, string Condition)
        {
            string strResult = null;

            for (int i = 0; i < val.GetLength(0); i++)
            {
                for (int j = 0; j < val.GetLength(1); j++)
                {
                    if (val[i, j + 1].oContent.Equals(Condition))
                    {
                        strResult = val[i, j].oContent.ToString();
                        break;
                    }
                }
            }

            return strResult;
        }
        #endregion

        #region ��ʾ�������
        public static void showResult(CEnum.Message_Body[,] val)
        {
            if (val[0, 0].oContent.ToString().Trim() == "SCUESS")
            {
                MessageBox.Show("�����ɹ�");
            }
            else if (val[0, 0].oContent.ToString().Trim() == "FAILURE")
            {
                MessageBox.Show("����ʧ��");
            }
            else
            {
                MessageBox.Show(val[0, 0].oContent.ToString().Trim());
            }
        }
        #endregion
    }
}
