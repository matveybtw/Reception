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
using System.Windows.Shapes;
using ProjectVisitorsRegistration.Viewmodels;
namespace ProjectVisitorsRegistration
{
    public partial class NewUser : Window
    {
        public bool add;
        public Visitor visitor;
        public NewUser()
        {
            InitializeComponent();
            DataContext = new NewUserViewmodel(this, ref visitor);
        }
        public void Close(Visitor v)
        {
            visitor = v;
            this.Close();
        }
    }
}


