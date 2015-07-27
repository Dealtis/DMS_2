using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace DMSvStandard
{ 
	/// <summary>
	/// This class represents a trip object retreived from Transics.
	/// </summary>
    class Trip
    {
        public static int STATE_START = 1;
        public static int STATE_STOP = 2;
        public static int STATE_STARTING = 3;
        public static int STATE_STOPING = 4;
        public static int STATE_BUSY = 5;
        public static int STATE_ERROR = 6;
        public static int STATE_NONSTARTED = 7;

        

        private String tripID;

        public String TripID
        {
            get { return tripID; }
            set { tripID = value; }
        }

        private int tripStartActivityID;

        public int TripStartActivityID
        {
            get { return tripStartActivityID; }
            set { tripStartActivityID = value; }
        }

        private int tripEndActivityID;

        public int TripEndActivityID
        {
            get { return tripEndActivityID; }
            set { tripEndActivityID = value; }
        }

        private int tripState;

        public int TripState
        {
            get { return tripState; }
            set { tripState = value; }
        }

        private String tripName;

        public String TripName
        {
            get { return tripName; }
            set { tripName = value; }
        }

        private DateTime tripExecutionDate;

        public DateTime TripExecutionDate
        {
            get { return tripExecutionDate; }
            set { tripExecutionDate = value; }
        }

        private int tripActionResult;

        public int TripActionResult
        {
            get { return tripActionResult; }
            set { tripActionResult = value; }
        }

        
    }
}
