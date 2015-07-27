using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;



namespace DMSvStandard
{
	/// <summary>
	/// This class represents text message which is exchenged between application and Transics.
	/// </summary>
	public class TextMessage
    {
        public static int STATUS_TOBESENT = 1;
        public static int STATUS_SENT = 2;
        public static int STATUS_RECEIVED = 3;
        public static int STATUS_DRAFT = 4;
        public static int STATUS_READ = 5;
        public static int STATUS_UNREAD = 6;
        public static int STATUS_TREATED = 7;
        public static int STATUS_NONTREATED = 8;

		public static int MSG_OUTBOX = 1;
		public static int MSG_INBOX = 2;
		public static int MSG_NEW = 4;
		public static int MSG_REPLAY = 5;

		private bool checkedItem = false;

		public bool isCheckedItem()
		{	return checkedItem;
		}

		public void setCheckedItem(bool _checked)
		{ 
			checkedItem = _checked;
		}


        public TextMessage()
        {
            id = ApplicationData.randomGenerator.Next();
        }

        private String message;

        public String Message
        {
            get { return message; }
            set { message = value; }
        }

        private String sender;

        public String Sender
        {
            get { return sender; }
            set { sender = value; }
        }

        private DateTime actionDate;

        public DateTime ActionDate
        {
            get { return actionDate; }
            set { actionDate = value; }
        }

        private int status;

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public String getStatusValue()
        {
            if (status == STATUS_DRAFT)
                return ApplicationData.Instance.getTranslator().translateMessage("messagestatus.draft");
            else if (status == STATUS_NONTREATED)
                return ApplicationData.Instance.getTranslator().translateMessage("messagestatus.nontreated");
            else if (status == STATUS_READ)
                return ApplicationData.Instance.getTranslator().translateMessage("messagestatus.read");
            else if (status == STATUS_RECEIVED)
                return ApplicationData.Instance.getTranslator().translateMessage("messagestatus.received");
            else if (status == STATUS_SENT)
                return ApplicationData.Instance.getTranslator().translateMessage("messagestatus.sent");
            else if (status == STATUS_TOBESENT)
                return ApplicationData.Instance.getTranslator().translateMessage("messagestatus.tobesent");
            else if (status == STATUS_TREATED)
                return ApplicationData.Instance.getTranslator().translateMessage("messagestatus.treated");
            else if (status == STATUS_UNREAD)
                return ApplicationData.Instance.getTranslator().translateMessage("messagestatus.unread");
            else return "";
        }

        /*private bool isRead;

        public bool IsRead
        {
            get { return isRead; }
            set { isRead = value; }
        }*/


        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private DateTime arrivalDate;

        public DateTime ArrivalDate
        {
            get { return arrivalDate; }
            set { arrivalDate = value; }
        }


    }
}
