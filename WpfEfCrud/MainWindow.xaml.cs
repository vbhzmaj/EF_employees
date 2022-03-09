using System;
using System.Collections.Generic;
using System.IO;
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
//using System.Windows.Shapes;
using Microsoft.Win32;


namespace WpfEfCrud
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string chosenPicture = "";
        private int change = -1;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void UpdateEmployee(bool employee)
        {
            GroupBox1.IsEnabled = employee;
            ButtonAdd.IsEnabled = !employee;
            ButtonEdit.IsEnabled = !employee;
        }

        private void ResetUI()
        {
            chosenPicture = "";
            Image1.Source = null;
            TextBoxId.Clear();
            TextBoxFirstName.Clear();
            TextBoxLastName.Clear();
            DatePicker1.SelectedDate = null;
            DataGrid1.SelectedIndex = -1;
        }
        private void ShowEmployees()
        {
            DataGrid1.Items.Clear();
            List<Employee> employeeList = EmployeeDA.GetEmployees();

            if (employeeList != null)
            {
                foreach (Employee emp in employeeList)
                {
                    DataGrid1.Items.Add(emp);
                }
            }
        }
        
        private bool EntryValidation()
        {
            if (string.IsNullOrWhiteSpace(TextBoxFirstName.Text))
            {
                MessageBox.Show("Type the first Name");
                TextBoxFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(TextBoxLastName.Text))
            {
                MessageBox.Show("Type the last Name");
                TextBoxLastName.Focus();
                return false;
            }

            return true;

        }
        
        private void InsertEmployee() 
        {
            if (!EntryValidation())
            {
                return;
            }

            Employee emp = new Employee();
            emp.FirstName = TextBoxFirstName.Text;
            emp.LastName = TextBoxLastName.Text;
            emp.BirthDate = DatePicker1.SelectedDate;

            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapFrame bf = BitmapFrame.Create((BitmapSource)Image1.Source);
            encoder.Frames.Add(bf);
            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                emp.Photo = ms.ToArray();
            }

            int result = EmployeeDA.AddEmployee(emp);

            if (result == 0)
            {
                ShowEmployees();
                DataGrid1.Focus();
                int index = DataGrid1.Items.Count - 1;
                DataGrid1.SelectedIndex = index;
                DataGrid1.ScrollIntoView(DataGrid1.Items[index]);
                chosenPicture = "";
                UpdateEmployee(false);
                MessageBox.Show("Employee added");
            }

            else
            {
                MessageBox.Show("Mistake by saving data");
            }
        }  

        private void EditEmployee() 
        {
            int indeks = DataGrid1.SelectedIndex;
            if (indeks < 0)
            {
                MessageBox.Show("Choose employee");
                return;
            }

           if (!EntryValidation())
            {
                return;
            }
            Employee emp = DataGrid1.SelectedItem as Employee;
            emp.FirstName = TextBoxFirstName.Text;
            emp.LastName = TextBoxLastName.Text;
            emp.BirthDate = DatePicker1.SelectedDate;

            if (Image1.Source != null)
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                BitmapFrame bf = BitmapFrame.Create((BitmapSource)Image1.Source);
                encoder.Frames.Add(bf);
                using (MemoryStream ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    emp.Photo = ms.ToArray();
                }
            }

            int result = EmployeeDA.EditEmployee(emp);

            if (result == 0)
            {
                ShowEmployees();
                DataGrid1.Focus();
                DataGrid1.SelectedIndex = indeks;
                DataGrid1.ScrollIntoView(DataGrid1.Items[indeks]);
                UpdateEmployee(false);
                MessageBox.Show("Data updated");
            }
            else
            {
                MessageBox.Show("Mistake in updating data");
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateEmployee(false);
            string folder = PhotoPath.GetPictureFolder();
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
                MessageBox.Show("Picture folder created");
            }
            ShowEmployees();
        }

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            ResetUI();
        }

        private void DataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataGrid1.SelectedIndex > -1)
            {
                chosenPicture = "";
                Employee emp = DataGrid1.SelectedItem as Employee;
                TextBoxId.Text = emp.EmployeeId.ToString();
                TextBoxFirstName.Text = emp.FirstName;
                TextBoxLastName.Text = emp.LastName;
                DatePicker1.SelectedDate = emp.BirthDate;
     
                if (emp.Photo != null)
                {
                    try
                    {
                        MemoryStream ms = new MemoryStream(emp.Photo);
                        BitmapImage bmp = new BitmapImage();
                        bmp.BeginInit();
                        bmp.StreamSource = ms;
                        bmp.CacheOption = BitmapCacheOption.OnLoad;
                        bmp.DecodePixelWidth = 150;
                        bmp.EndInit();
                        Image1.Source = bmp;
                    }
                    catch (Exception)
                    {

                        MessageBox.Show("Mistake in uploading photo");
                    }
                   
                }
                else
                {
                    MessageBox.Show("No picture available");
                }
                
            }
        }

        private void ButtonChoose_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.InitialDirectory = @"C:\Pictures";
            openDlg.Filter = @"Pictures|*.jpg;*.bmp;*.gif";

            if (openDlg.ShowDialog() == true)
            {
                chosenPicture = openDlg.FileName;

                Uri adress = new Uri(chosenPicture, UriKind.Absolute);
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = adress;
                bmp.CacheOption = BitmapCacheOption.OnLoad;
                bmp.EndInit();
                Image1.Source = bmp;
            }

        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ResetUI();
            UpdateEmployee(true);
            change = 0;

        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DataGrid1.SelectedIndex > -1)
            {
                Employee emp = (Employee)DataGrid1.SelectedItem;
                MessageBoxResult confirmDelete = MessageBox.Show("Are you sure to delete this item ??", "Confirm Delete!!", MessageBoxButton.YesNo);

                if (confirmDelete == MessageBoxResult.Yes)
                {
                    int result = EmployeeDA.DeleteEmployee(emp);

                    if (result == 0)
                    {
                        ShowEmployees();
                        ResetUI();
                        MessageBox.Show("Employee deleted");
                    }
                }
                else
                {
                    ResetUI();
                }
      
            }
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            int indeks = DataGrid1.SelectedIndex;
            if (indeks < 0)
            {
                MessageBox.Show("Choose employee");
                return;
            }
            UpdateEmployee(true);
            change = 1;
         }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            UpdateEmployee(false);
            ResetUI();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (change == 0)
            {
                InsertEmployee();
               
            }

            if (change == 1)
            {
                EditEmployee();
            }
         
        }
    }
}
