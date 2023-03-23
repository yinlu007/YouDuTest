using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Net;
using YouduTest.EntApp;
using YouduTest.EntApp.MessageEntity;
using YouduTest.EntApp.SessionEntity;
using YouduTest.EntApp.AES;

namespace YouduTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            m_appClient = new AppClient(ip, Convert.ToInt32(tel), appid, key);
        }
        AppClient m_appClient;
        string ip = ConfigurationManager.AppSettings["YOUDUINTEROP_IP"].ToString();
        string tel = ConfigurationManager.AppSettings["YOUDUINTEROP_TEL"].ToString();
        string appid = ConfigurationManager.AppSettings["YOUDUINTEROP_AppId"].ToString();
        string key = ConfigurationManager.AppSettings["YOUDUINTEROP_EncodingAESKey"].ToString();

        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGetInfo_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = ip;
            this.textBox2.Text = tel;
            this.textBox3.Text = appid;
            this.textBox4.Text = key;
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //标题
            string Title = this.textBox5.Text.Trim();
            //内容
            string Content = this.textBox6.Text.Trim();
            //账号
            string Touser = this.textBox9.Text.Trim();

            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("请首先获取配置信息");
                return;
            }

            if (string.IsNullOrEmpty(Title))
            {
                MessageBox.Show("标题不能为空");
                return;
            }
            if (string.IsNullOrEmpty(Content))
            {
                MessageBox.Show("内容不能为空");
                return;
            }
            if (string.IsNullOrEmpty(Touser))
            {
                MessageBox.Show("账号不能为空");
                return;
}
             //发送消息
            AppClient m_appClient = new AppClient(this.textBox1.Text, Convert.ToInt32( this.textBox2.Text), this.textBox3.Text, this.textBox4.Text);

            //调用接口发送系统消息
            var sysMsg = new SysmsgBody();
            sysMsg.Title = Title;
            sysMsg.addTextBody(Content);
            var msg = new EntApp.MessageEntity.Message(Touser, EntApp.MessageEntity.Message.MessageTypeSysmsg, sysMsg);
            this.textBox7.Text = msg.ToJson();
             m_appClient.SendMsg(msg);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox7.Text))
            {
                MessageBox.Show("明文不能为空");
                return;
            }
            string s= m_appClient.Encrypt(this.textBox7.Text);
            this.textBox8.Text = s;
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.textBox8.Text))
            {
                MessageBox.Show("密文不能为空");
                return;
            }
            string s = m_appClient.Decrypt(this.textBox8.Text);
            this.textBox7.Text = s;
        }

        /// <summary>
        /// 单人会话
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            //内容
            string Content = this.textBox6.Text.Trim();
            //账号
            string Touser = this.textBox9.Text.Trim();

            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("请首先获取配置信息");
                return;
            }
            if (string.IsNullOrEmpty(Content))
            {
                MessageBox.Show("内容不能为空");
                return;
            }
            if (string.IsNullOrEmpty(Touser))
            {
                MessageBox.Show("账号不能为空");
                return;
            }
            //发送消息
            AppClient m_appClient = new AppClient(this.textBox1.Text, Convert.ToInt32(this.textBox2.Text), this.textBox3.Text, this.textBox4.Text);

            //调用接口发送系统消息
            var body = new TextBody(Content);
            var msg = new IMMessage(Touser, "EAPSYS", "", EntApp.MessageEntity.Message.MessageTypeText, body);
            m_appClient.SendIMMsg(msg);
        }

        //多人会话
        private void button5_Click(object sender, EventArgs e)
        {
            //标题
            string Title = this.textBox5.Text.Trim();
            //内容
            string Content = this.textBox6.Text.Trim();
            //账号
            string Touser = this.textBox9.Text.Trim();

            if (string.IsNullOrEmpty(ip))
            {
                MessageBox.Show("请首先获取配置信息");
                return;
            }

            if (string.IsNullOrEmpty(Title))
            {
                MessageBox.Show("标题不能为空");
                return;
            }
            if (string.IsNullOrEmpty(Content))
            {
                MessageBox.Show("内容不能为空");
                return;
            }
            if (string.IsNullOrEmpty(Touser))
            {
                MessageBox.Show("账号不能为空");
                return;
            }

            string[] member = Touser.Split('|');//会话成员账号列表
            if (member.Length<=3)
            {
                MessageBox.Show("多人会话人数必须大于3");
                return;
            }

          //发送消息
            AppClient m_appClient = new AppClient(this.textBox1.Text, Convert.ToInt32(this.textBox2.Text), this.textBox3.Text, this.textBox4.Text);

            //先创建会话四个字段
            string title = Title;//会话标题
            string type = "multi"; //会话类型

            ////调用接口发送系统消息 先创建会话 再发送消息
            var SessionMsg = new EntApp.MessageEntity.SessionMessage(title, "EAPSYS", type, member);
            string SessionId = m_appClient.apiCreateSession(SessionMsg); // 会话sessionId 多人会话填写
            //调用接口发送系统消息
            var body = new TextBody(Content);
            var msg = new IMMessage("", "EAPSYS", SessionId, EntApp.MessageEntity.Message.MessageTypeText, body);
            m_appClient.SendIMMsg(msg);
        }
    }
}
