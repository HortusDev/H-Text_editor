using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using Text_editor.Commands;
using Text_editor.Models;
using System.IO;
using System.Windows;

namespace Text_editor.ViewModels
{
    internal class EditorViewModel: INotifyPropertyChanged
    {
        private Document1 _document;

         public string Content
         {
            get => _document.Content;
            set
            {
                if (_document.Content != value)
                {
                    _document.Content = value;
                    OnPropertyChanged(nameof(Content));
                }
            }
         }

        public string FilePath
        {
            get => _document.FilePath;
            set
            {
                if (_document.FilePath != value)
                {
                    _document.FilePath = value;
                    OnPropertyChanged(nameof(FilePath));
                    OnFilePathChanged();
                }
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand OpenCommand { get; }

        public EditorViewModel()
        { 
            _document = new Document1();
            SaveCommand = new RelayCommand(Save, CanSave);
            OpenCommand = new RelayCommand(Open);
        }

        private bool CanSave()
        {
            //return !string.IsNullOrEmpty(Content);
            var canSave = !string.IsNullOrEmpty(FilePath);
            return canSave;
        }
        private void Save()
        {

            try
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    SaveAs();
                }
                else
                {
                    _document.Save();
                    MessageBox.Show("File saved");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);  
            }
               
        }

        private void SaveAs()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = "txt"
            };

            if (saveFileDialog.ShowDialog() == true)
            { 
               FilePath = saveFileDialog.FileName;
               _document.Save();
               MessageBox.Show("File saved");
            }
        }

        private void Open() 
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };
            if (openFileDialog.ShowDialog() == true)
            {
                try
                { 
                  FilePath = openFileDialog.FileName;
                  Content = File.ReadAllText(FilePath); //need to create File
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show($"Error opening file: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                }
            }
            

        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnFilePathChanged()
        {        
            (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }


}
