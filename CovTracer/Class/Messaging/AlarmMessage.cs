using CovTracer.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace CovTracer.Class.Messaging
{
    public class AlarmMessage : ViewModelBase
    {
        #region Constant Variables
        private string _statusRunning = "Running";
        private string _statusStopped = "Stopped";
        private string _statusIdle = "Idle";
        #endregion

        #region Constructor
        public AlarmMessage()
        {
            Status = _statusIdle;
        }
        #endregion

        #region Properties
        private int _timeVal;
        public int TimeVal
        {
            get
            {
                return _timeVal;
            }
            set
            {
                _timeVal = value;
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        private int _ctr;
        public int Ctr
        {
            get
            {
                return _ctr;
            }
            set
            {
                SetProperty(ref _ctr, value);
            }
        }
        #endregion

        #region Methods
        public void IncrementCounter()
        {
            Ctr++;
        }
        public void RunTimer()
        {
            Status = _statusRunning;
            Ctr = 0;
            Timer timer = new Timer(TimeVal * 1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Ctr++;
        }
        #endregion


    }
}
