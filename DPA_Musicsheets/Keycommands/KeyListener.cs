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
    class KeyListener
    {
        // the design of the keylistener is that we'll get keyevents from WPF
        // each time a keydown event is received, a timer is reset (LastKeypressTimestamp)
        // if the key was pressed quickly enough after the first one, we'll consider it to be part of the current sequence
        // if the timer runs out, the sequence will be reset and passed onto the keyhandler-chain-of-command

        // overall, the design idea is that we use timers to compose a sequence of keys
        // and pass this "sequence" on to the chain of command to run the right action.

        const int SEQUENCE_TIME_CUTOFF_MS = 450; // in milliseconds

        private bool KeyHasBeenPressedOnTime = false;

        // automatically fire if a key sequence becomes of length 4
        const int MAX_SEQUENCE_LENGTH = 4;
        // {Keys.Z}, {Keys.LeftCtrl}, {Keys.Alt} etc aren't sequences, they're keys.
        const int MIN_SEQUENCE_LENGTH = 2;

        List<Key> currentKeySequence = new List<Key>();

        private System.Windows.Forms.Timer timer;

        public KeyListener()
        {
            ResetTimer();
        }

        /// <summary>
        /// The timer will fire every 250ms
        /// if a key has been pressed within this 250ms, it will do nothing
        /// if nothing has been pressed, it will fire the current sequence of keys
        /// </summary>
        private void ResetTimer()
        {
            timer = new System.Windows.Forms.Timer();
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = SEQUENCE_TIME_CUTOFF_MS;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (KeyHasBeenPressedOnTime)
            {
                FireSequence();
            }
        }

        public void FireSequence()
        {
            // pass the sequence on to the chain of command
            // do(currentKeySequence);
            if (currentKeySequence.Count() >= MIN_SEQUENCE_LENGTH)
            {
                KeySequence seq = new KeySequence("generated sequence", currentKeySequence);
            }

            // clean up
            currentKeySequence = new List<Key>();
            KeyHasBeenPressedOnTime = false;
        }

        /// <summary>
        /// Adds a key to the current sequence
        /// Will fire the sequence if it's too long (MAX_SEQUENCE_LENGTH)
        /// </summary>
        /// <param name="e"></param>
        internal void KeyDown(KeyEventArgs e)
        {
            if (currentKeySequence.Count >= MAX_SEQUENCE_LENGTH)
            {
                FireSequence();
                return;
            }

            if (!e.IsRepeat)
            {
                currentKeySequence.Add(e.Key);
            }
            KeyHasBeenPressedOnTime = true;
        }

        // probably don't need this one
        internal void KeyUp(KeyEventArgs e)
        {
            // just ignore these events
        }
    }
}
