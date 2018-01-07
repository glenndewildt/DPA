using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Messages;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using PSAMWPFControlLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DPA_Musicsheets.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _fileName;
        public string FileName
        {
            get
            {
                return _fileName;
            }
            set
            {
                _fileName = value;
                RaisePropertyChanged(() => FileName);
            }
        }

        private string _currentState;
        public string CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; RaisePropertyChanged(() => CurrentState); }
        }

        private FileHandler _fileHandler;

        public MainViewModel(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            FileName = @"Files/Alle-eendjes-zwemmen-in-het-water.mid";

            MessengerInstance.Register<CurrentStateMessage>(this, (message) => CurrentState = message.State);

            _fileHandler.FileOpened += OnFileOpened;
        }

        private void OnFileOpened(object sender, FileOpenedEventArgs e)
        {
            FileName = e.fileName;
        }

        public void LoadFile()
        {
            _fileHandler.OpenFile(FileName);
        }

        public ICommand OpenFileCommand => new RelayCommand(() =>
        {
            _fileHandler.ShowOpenFileDialog();
        });

        public ICommand LoadCommand => new RelayCommand(LoadFile);

        public ICommand OnLostFocusCommand => new RelayCommand(() =>
        {
            Console.WriteLine("Maingrid Lost focus");
        });

        public ICommand OnWindowClosingCommand => new RelayCommand(() =>
        {
            ViewModelLocator.Cleanup();
        });
    }
}
