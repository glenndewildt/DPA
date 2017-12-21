using DPA_Musicsheets.Managers;
using DPA_Musicsheets.Messages;
using DPA_Musicsheets.LilyPond;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DPA_Musicsheets.Keycommands;
using DPA_Musicsheets.keycommands;

namespace DPA_Musicsheets.ViewModels
{
    public class LilypondViewModel : ViewModelBase
    {
        private FileHandler _fileHandler;
        private LilypondEditor _lilyEditor;

        private HotkeyChainOfResponsibility _hotkeyChain;
        private KeyListener _keyListener;

        private bool _textChangedByLoad = false;
        private DateTime _lastChange;
        private static int MILLISECONDS_BEFORE_CHANGE_HANDLED = 1500;
        private bool _textChangedByUndoRedo;

        public LilypondViewModel(FileHandler fileHandler)
        {
            _fileHandler = fileHandler;

            _fileHandler.LilypondLoaded += (src, e) =>
            {
                _textChangedByLoad = true;
                _lilyEditor.ClearBookmarkHistory();
                _lilyEditor.SetText(e.LilypondText);
                _lilyEditor.AddBookmark(_lilyEditor.SaveStateToMemento());
                _textChangedByLoad = false;
            };

            _hotkeyChain = new HotkeyChainOfResponsibility();
            _keyListener = new KeyListener(_hotkeyChain);
        }

        public ICommand LoadedCommand => new RelayCommand<RoutedEventArgs>(args =>
        {
            TextBox textBox = (TextBox)args.OriginalSource;

            _lilyEditor = new LilypondEditor(_hotkeyChain, textBox);
            _lilyEditor.SetText("Your lilypond text will appear here.");
        });

        public ICommand OnKeyDownCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            _keyListener.KeyDown(e);
        });

        public ICommand OnKeyUpCommand => new RelayCommand<KeyEventArgs>((e) =>
        {
            _keyListener.KeyUp(e);
        });

        public ICommand SelectionChangedCommand => new RelayCommand(() =>
        {
            _lilyEditor.debugBookmarks();
        });
        
        public ICommand TextChangedCommand => new RelayCommand<TextChangedEventArgs>((args) =>
        {
            TextBox textBox = (TextBox)args.OriginalSource;

            if (!_textChangedByLoad)
            {
                _lastChange = DateTime.Now;
                MessengerInstance.Send<CurrentStateMessage>(new CurrentStateMessage() { State = "Rendering..." });

                Task.Delay(MILLISECONDS_BEFORE_CHANGE_HANDLED).ContinueWith((task) =>
                {
                    if ((DateTime.Now - _lastChange).TotalMilliseconds >= MILLISECONDS_BEFORE_CHANGE_HANDLED)
                    {
                        _lilyEditor.SetText(textBox.Text);
                        if (_textChangedByUndoRedo)
                        {
                            _textChangedByUndoRedo = false;
                        } else
                        {
                            _lilyEditor.AddBookmark(_lilyEditor.SaveStateToMemento());
                        }

                        // handles the generation of the graphical notes representation
                        _fileHandler.LoadLilypond(textBox.Text);
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext()); // Request from main thread.
            }
        });

        public RelayCommand UndoCommand => new RelayCommand(() =>
        {
            _lilyEditor.Undo();
            _textChangedByUndoRedo = true;
        }, () => _lilyEditor != null && _lilyEditor.CanUndo());

        public RelayCommand RedoCommand => new RelayCommand(() =>
        {
            _lilyEditor.Redo();
            _textChangedByUndoRedo = true;
        }, () => _lilyEditor != null && _lilyEditor.CanRedo());

        public ICommand SaveAsCommand => new RelayCommand(() =>
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog() { Filter = "Midi|*.mid|Lilypond|*.ly|PDF|*.pdf" };
            if (saveFileDialog.ShowDialog() == true)
            {
                string extension = Path.GetExtension(saveFileDialog.FileName);
                if (extension.EndsWith(".mid"))
                {
                    _fileHandler.SaveToMidi(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".ly"))
                {
                    _fileHandler.SaveToLilypond(saveFileDialog.FileName);
                }
                else if (extension.EndsWith(".pdf"))
                {
                    _fileHandler.SaveToPDF(saveFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show($"Extension {extension} is not supported.");
                }
            }
        });
    }
}
