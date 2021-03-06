﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DTEModels.MVVM_Tools;

namespace DTEModels.Models
{
    public class FaultInjectionModel : ViewModelBase
    {
        bool duplicateMessage;

        public bool DuplicateMessage
        {
            get { return duplicateMessage; }
            set { duplicateMessage = value;
            OnPropertyChanged("DuplicateMessage");
            }
        }
        bool loseMessage;

        public bool LoseMessage
        {
            get { return loseMessage; }
            set { loseMessage = value;
            OnPropertyChanged("LoseMessage");
            }
        }
        bool corruptMessage;

        public bool CorruptMessage
        {
            get { return corruptMessage; }
            set { corruptMessage = value;
            OnPropertyChanged("CorruptMessage");
            }
        }
        bool delayMessage;

        public bool DelayMessage
        {
            get { return delayMessage; }
            set { delayMessage = value;
            OnPropertyChanged("DelayMessage");
            }
        }
        bool reverseOrderMessage;

        public bool ReverseOrderMessage
        {
            get { return reverseOrderMessage; }
            set { reverseOrderMessage = value;
            OnPropertyChanged("ReverseOrderMessage");
            }
        }
        string previousMessage = null;

        public string PreviousMessage
        {
            get { return previousMessage; }
            set { previousMessage = value;
            OnPropertyChanged("PreviousMessage");
            }
        }
        int delay_ms = 0;

        public int Delay_ms
        {
            get { return delay_ms; }
            set { delay_ms = value;
            OnPropertyChanged("Delay_ms");
            }
        }
        bool disable_process;

        public bool Disable_process
        {
            get { return disable_process; }
            set { disable_process = value;
            OnPropertyChanged("Disable_process");
            }
        }

        private string OutOfOrderMessage(string msg)
        {
            if(previousMessage == null)
            {
                previousMessage = msg;
                return null;
                
            }
            return msg;
        }

        private string DelayTheMessage(string msg)
        {
            Thread.Sleep(delay_ms);
            return msg;
        }

        private string CorruptTheMessage(string msg)
        {
            byte[] bytedata = Encoding.ASCII.GetBytes(msg);
            int counter = 0;
            foreach(byte data in bytedata)
            {
                bytedata[counter] ^= 0x55;
                counter++;
            }
            return Encoding.ASCII.GetString(bytedata, 0, bytedata.Length);
        }
    
        public string applyFaults(string msg)
        {
            string tmpMsg = msg;
            if(Disable_process || LoseMessage)
            {
                return null;
            }
            if (ReverseOrderMessage)
            {
                tmpMsg = OutOfOrderMessage(tmpMsg);
                if (tmpMsg == null)
                    return null;
            }
            if(CorruptMessage)
            {
                tmpMsg = CorruptTheMessage(tmpMsg);
            }
            if(DelayMessage)
            {
                DelayTheMessage(tmpMsg);
            }


            return tmpMsg;
        }
    
    }
}
