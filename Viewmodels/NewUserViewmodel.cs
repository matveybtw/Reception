using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFbase;

namespace ProjectVisitorsRegistration.Viewmodels
{
    public class NewUserViewmodel:OnPropertyChangedHandler
    {
        public Visitor visitor { get; set; } = new Visitor();
        public Visitor v { get; set; }
        public NewUser window { get; set; }
        public NewUserViewmodel(NewUser newUser , ref Visitor vv)
        {
            window = newUser;
            v = vv;
        }
        public ICommand Save => new RelayCommand(o =>
        {
            window.add = true;
            visitor.datecame.Item = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            v= visitor;
            window.Close(visitor);
        }, o=> visitor.name.Item.Length>0&& visitor.sur.Item.Length>0);
        public ICommand Cancel => new RelayCommand(o =>
        {
            window.add = false;
            window.Close();
        }, o=>true);


    }
}
