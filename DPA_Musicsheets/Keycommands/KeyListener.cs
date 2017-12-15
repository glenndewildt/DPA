using DPA_Musicsheets.Keycommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DPA_Musicsheets.keycommands
{
    public class KeyListener
    {
        // the design of the keylistener is that we'll get keyevents from WPF
        // each time a keydown event is received, a timer is reset (LastKeypressTimestamp)
        // if the key was pressed quickly enough after the first one, we'll consider it to be part of the current sequence
        // if the timer runs out, the sequence will be reset and passed onto the keyhandler-chain-of-command

        // overall, the design idea is that we use timers to compose a sequence of keys
        // and pass this "sequence" on to the chain of responsibility to run the right action.

        // we don't want people to chord commands
        const int MAX_SEQUENCE_LENGTH = 4;
        // {Keys.Z}, {Keys.LeftCtrl}, {Keys.Alt} etc aren't sequences, they're keys.
        const int MIN_SEQUENCE_LENGTH = 2;

        List<Key> currentKeySequence = new List<Key>();

        private HotkeyChainOfResponsibility _hotkeyChain;

        public KeyListener(HotkeyChainOfResponsibility keyHandler)
        {
            _hotkeyChain = keyHandler;
        }

        public void FireSequence()
        {
            // pass the sequence on to the chain of command
            // do(currentKeySequence);
            if (currentKeySequence.Count() >= MIN_SEQUENCE_LENGTH)
            {
                KeySequence seq = new KeySequence("generated sequence", currentKeySequence);
                seq.LogKeysequence();
                _hotkeyChain.Handle(seq);
            }

            // clean up
            currentKeySequence = new List<Key>();
        }

        /// <summary>
        /// Adds a key to the current sequence
        /// Will fire the sequence if it's too long (MAX_SEQUENCE_LENGTH)
        /// </summary>
        /// <param name="e"></param>
        internal void KeyDown(KeyEventArgs e)
        {
            if (!e.IsRepeat && currentKeySequence.Count <= MAX_SEQUENCE_LENGTH)
            {
                if (e.Key == Key.System)
                {
                    currentKeySequence.Add(e.SystemKey);
                } else
                {
                    currentKeySequence.Add(e.Key);
                }
            }
        }

        // probably don't need this one
        internal void KeyUp(KeyEventArgs e)
        {
            FireSequence();
        }
    }
}
