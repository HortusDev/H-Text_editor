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
using System.Xml.Linq;

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
                    
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
         }

        public string FilePath
        {
            get => _document.FilePath;
            private set
            {
                if (_document.FilePath != value)
                {
                    _document.FilePath = value;
                    OnPropertyChanged(nameof(FilePath));
                    (SaveCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }
        public string FileName
        {
            get
            {
                return string.IsNullOrEmpty(FilePath)
                      ?"No file opened"
                      :Path.GetFileName(FilePath);
            }
        }
        public ICommand SaveCommand { get; }
        public ICommand OpenCommand { get; }
        public ICommand NewCommand { get; }
        public ICommand SaveAsCommand { get; }

        public EditorViewModel()
        { 
            _document = new Document1();
            NewCommand = new RelayCommand(NewFile);
            SaveCommand = new RelayCommand(SaveFile, CanSaveFile);
            OpenCommand = new RelayCommand(OpenFile);
            SaveAsCommand = new RelayCommand(SaveFileAs);

        }

        private void NewFile()
        {
            if (!string.IsNullOrEmpty(_document.Content))
            {
                var result = MessageBox.Show("Do you want to save changes to the current file?", "Save Changes", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Cancel)
                {
                    return;
                }
                else if (result == MessageBoxResult.Yes)
                {
                    SaveFile();
                }
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = "txt",
                Title = "Create New File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                _document.CreateNew(saveFileDialog.FileName);
                OnPropertyChanged(nameof(Content));
                OnPropertyChanged(nameof(FilePath));
                MessageBox.Show($"New file created at: {_document.FilePath}", "File Created", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            UpdateFilePath(Path.GetFileName(FilePath));
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                Title = "Open File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    _document.Load(openFileDialog.FileName);
                    OnPropertyChanged(nameof(Content));
                    OnPropertyChanged(nameof(FilePath));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            UpdateFilePath(Path.GetFileName(FilePath));
        }

        private void SaveFile()
        {
            try
            {
                _document.Save();
                MessageBox.Show("File saved successfully.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (InvalidOperationException)
            {
                SaveFileAs();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveFileAs()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                DefaultExt = "txt",
                Title = "Save File As"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                FilePath = saveFileDialog.FileName;
                SaveFile();
            }
        }

        private bool CanSaveFile()
        {
            //return !string.IsNullOrEmpty(Content);
            var canSave = !string.IsNullOrEmpty(FilePath);
            return canSave;
        }

        private void UpdateFilePath(string newPath)
        {
            if (_document.FilePath != newPath)
            {
                _document.FilePath = newPath;
                OnPropertyChanged(nameof(FilePath));
                OnPropertyChanged(nameof(FileName));
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
