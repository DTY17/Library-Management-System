using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF.Model;

namespace WPF.User_Control
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private BookModel book;
        private MemberModel member;
        private RecordModel record;
        public Home()
        {
            book = new BookModel();
            member = new MemberModel();
            record = new RecordModel();
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            BookLabel.Text = getBookCount().ToString();
            MemberLabel.Text = getMemberCount().ToString();
            NotReturnedLabel.Text = getNotReturnBookCount().ToString();
        }

        private int getMemberCount()
        {
            return member.getMemberCount();
        }

        private int getBookCount()
        {
            return book.getBookCount();
        }

        private int getNotReturnBookCount()
        {
            return record.GetNotReturnBookCount();
        }
    }
}
