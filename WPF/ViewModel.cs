using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Scanner_lib;

using Microsoft.WindowsAPICodePack.Dialogs;
using WPF.DirectoryTreeMapper;


namespace WPF
{
    public class ViewModel : INotifyPropertyChanged
    {


        
        private bool _isSearchEnabled = false;
        public bool IsSearchEnabled
        {
            get
            {
                return _isSearchEnabled;
            }
            set
            {
                _isSearchEnabled = value;
                OnPropertyChanged("IsStartEnable");
                OnPropertyChanged("IsSearchEnabled");
            }
        }

        private bool _isFileChosen = false;
        public bool IsFileChosen
        {
            get
            {
                return _isFileChosen;
            }
            set
            {
                _isFileChosen = value;
                OnPropertyChanged("IsStartEnable");
                OnPropertyChanged("IsFileChosen");
            }
        }
        
        public bool IsStartEnable
        {
            get
            {
                return IsFileChosen && !IsSearchEnabled;
            }
        }

        private string? _filePath;
        public string FilePath
        {
            get
            {
                return _filePath ?? "";
            }
            set
            {
                _filePath = value;
                OnPropertyChanged("FilePath");
            }
        }


        
        private ObservableCollection<DirectoryTreeView> _treeNodes;
        public ObservableCollection<DirectoryTreeView> TreeViewList
        {
            get
            {
                return _treeNodes;
            }
            set { 
                _treeNodes = value; 
            }
        }

        public DirectoryTree TreeResult
        {
            set
            {
                if (value != null)
                {
                    _treeNodes = new ObservableCollection<DirectoryTreeView>();
                    _treeNodes.Add(value.ToTreeViewNode());
                    OnPropertyChanged("TreeViewList");
                }
            }
        }
        
        
        
        
        private RelayCommand _startScan;
        public RelayCommand StartScan
        {
            get
            {
                return _startScan ??= new RelayCommand(obj =>
                {
                    
                    Task.Run(() =>
                    {
                        IsSearchEnabled = true;
                        var result= DirectoryScanner.StartScan(FilePath);
                        
                        IsSearchEnabled = false;

                        TreeResult = result;
                    });
                });
            }
        }
        
        private RelayCommand _stopScan;
        public RelayCommand StopScan
        {
            get
            {
                return _stopScan ??= new RelayCommand(obj =>
                {
                    DirectoryScanner.StopScan();
                });
            }
        }
        
        
        
        
        
        
        private RelayCommand _chooseFile;
        public RelayCommand ChooseFile
        {
            get
            {
                return _chooseFile = new RelayCommand(obj =>
                {
                    using  (var dialog = new CommonOpenFileDialog())
                    {
                        dialog.IsFolderPicker = true;
                        dialog.Multiselect = false;
                        
                        if( dialog.ShowDialog() == CommonFileDialogResult.Ok)
                        {
                            FilePath = dialog.FileName;
                            IsFileChosen = true;
                        }
                    }
                     
                    
                });
            }
        }
        
        
        
        
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
    }
}