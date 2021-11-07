using Microsoft.Win32;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFbase;
namespace ProjectVisitorsRegistration.Viewmodels
{
    public class Visitor
    {
        public ChangingItem<string> name { get; set; } = new ChangingItem<string>();
        public ChangingItem<string> sur { get; set; } = new ChangingItem<string>();
        public ChangingItem<string> datecame { get; set; } = new ChangingItem<string>();
        public ChangingItem<string> dategone { get; set; } = new ChangingItem<string>();
        public Visitor()
        {
            name.Item = "";
            sur.Item = "";
            datecame.Item = "";
            dategone.Item = "";
        }
    }
    public class MainWindowViewmodel : OnPropertyChangedHandler
    {
        public List<Visitor> visitors { get; set; } = new List<Visitor>();
        public ChangingItem<int> selected { get; set; } = new ChangingItem<int>();
        public ObservableCollection<Visitor> activevisitors { get => new ObservableCollection<Visitor>(visitors.FindAll(o => o.dategone.Item == "")); }
        public ObservableCollection<Visitor> gonevisitors { get => new ObservableCollection<Visitor>(visitors.FindAll(o => o.dategone.Item != "")); }
        public ICommand AddUser => new RelayCommand(o =>
        {
            NewUser window = new NewUser();
            window.ShowDialog();
            if (window.add)
            {
                visitors.Add(window.visitor);
                OnPropertyChanged(nameof(activevisitors));
            }

        }, o => true);
        public ICommand UserIsGone => new RelayCommand(o =>
        {
            visitors[visitors.IndexOf(activevisitors[selected.Item])].dategone.Item = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            OnPropertyChanged(nameof(activevisitors));
            OnPropertyChanged(nameof(gonevisitors));
        }, o =>
        {
            try
            {
                if (visitors.IndexOf(activevisitors[selected.Item])==-1)
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;

        }
        );
        public ICommand SaveToExcelGone => new RelayCommand(o =>
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "xlsx";
            sfd.Filter = "Excel file(*.xlsx;*xls)|*.xlsx;*.xls";
            if (sfd.ShowDialog()!=null)
            {
                Write(gonevisitors, sfd.FileName);
                System.Diagnostics.Process.Start(sfd.FileName);
            }
            

        }, o => gonevisitors.Count > 0);
        void Write(IEnumerable<Visitor> l, string name)
        {
            List<string> names = new List<string>() { "Имя","Фамилия","Время прихода","Время ухода" };
            var memoryStream = new MemoryStream();
            List<string> vs = new List<string>();
            using (var fs = new FileStream(name, FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Sheesh");
                List<String> columns = new List<string>();
                IRow row = excelSheet.CreateRow(0);
                for (int i = 0; i < names.Count; i++)
                {
                    columns.Add(names[i]);
                    row.CreateCell(i).SetCellValue(names[i]);
                }
                int rowIndex = 1;
                foreach (var item in l)
                {
                    row = excelSheet.CreateRow(rowIndex);
                    row.CreateCell(0).SetCellValue(item.name.Item);
                    row.CreateCell(1).SetCellValue(item.sur.Item);
                    row.CreateCell(2).SetCellValue(item.datecame.Item);
                    row.CreateCell(3).SetCellValue(item.dategone.Item);
                    rowIndex++;
                }
                workbook.Write(fs);
            }

        }
    }
    
}
