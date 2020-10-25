using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ProjectERP
{
    public partial class Form2 : Form
    {
        string strConn = "Server=localhost;Database=erpproject;Uid=root;Pwd=1234;Charset=utf8";
        public static string userId;
        public static string userPw;
        public Form2()
        {
            InitializeComponent();
        }

        

        //부서 - 등록 : gridView에 값 뿌려주려고 컬럼값 가져오는 쿼리 아직 미완성
        private List<string> getColumnName()
        {
            List<string> colList = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(strConn))
            {
                conn.Open();
                string sql = "SHOW COLUMNS FROM division";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    colList.Add(rdr["Field"].ToString());
                }
                rdr.Close();
            }
            return colList;
        }
        //사원 - 등록 : 로그아웃 버튼인데 체크박스 자동값 불러오기 테스트로 만들어본거임
        private void button10_Click_1(object sender, EventArgs e)
        {
            Form1.userId = userId;
            Form1.userPw = userPw;
            this.Visible = false;
            Form1 form1 = new Form1();
            form1.Owner = this;
            form1.Show();
        }
    }
}
