using DPA_Musicsheets.Commands;
using DPA_Musicsheets.Keycommands;

namespace DPA_Musicsheets.LilyPond
{
    internal class OpenFileHandler : HotkeyHandler
    {
        private KeySequence _pattern = KeysequenceDefinitions.CTRL_O;
        private ICommand_mb _cmd;

        public OpenFileHandler(ICommand_mb cmd)
        {
            _cmd = cmd;
        }

        public override void Handle(KeySequence keys)
        {
            if (CanHandle(keys))
            {
                _cmd.Execute();
            }
            else
            {
                Successor?.Handle(keys);
            }
        }

        protected override bool CanHandle(KeySequence keys)
        {
            return keys.Equals(_pattern);
        }
    }
}