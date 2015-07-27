using System;
using System.Collections.Generic;
using System.Text;

namespace DMSvStandard
{
	/// <summary>
	///  This class represents a message which is exchenged between applicatio and server.
	/// </summary>
    class DealtisMessage
    {
        private String userCode;
        private DateTime messageDate;
        private DateTime timeout;
        private String agancyCode;
        private String actionType;
        private String actionCode;
        private String comments;
        private int actionPriority;
        private int actionStatus;
        private int connectionType;
        private Dictionary<String, String> parameters;

        public static int CONNECTION_ANY = 0;
        public static int CONNECTION_USB = 1;
        public static int CONNECTION_WIFI = 2;
        public static int CONNECTION_GPRS = 3;

        public static int STATUS_NEW = 0;
        public static int STATUS_QUEUED = 1;
        public static int STATUS_EXECUTED = 2;
        public static int STATUS_FAILD = 3;
        public static int STATUS_INPORGRESS = 7;

        public static String ACTION_UPLOAD = "UPLOAD";
        public static String ACTION_DOWNLOAD = "DOWNLOAD";

        public static String ACTION_INBOX = "INBOX";
        public static String ACTION_OUTBOX = "OUTBOX";
        public static String ACTION_SETREAD = "SETREAD";
        public static String ACTION_SENDMSG = "SENDMSG";
        public static String ACTION_SENDPOSSITION = "GPSPOSSITION";
        public static String ACTION_LOADACTIVITIES = "LOADACTIVITIES";
        public static String ACTION_STARTACTIVITY = "STARTACTIVITY";
        public static String ACTION_STOPACTIVITY = "STOPACTIVITY";
        public static String ACTION_LOADTRIP = "LOADTRIP";
        public static String ACTION_STARTTRIP = "STARTTRIP";
        public static String ACTION_STOPTRIP = "STOPTRIP";


        public static String MSG_SEPARATOR = "#X%";
        
        public DealtisMessage()
        {
            messageDate = DateTime.Now;      
            userCode = "";            
            agancyCode = "";
            actionType = "";
            actionCode = "";
            actionPriority = 0;
            connectionType = 0;
            actionStatus = 0;
            comments = "";
            parameters = new Dictionary<String, String>();
        }



        public String getUserCode()
        { return userCode; }

        public void setUserCode(String userCode)
        { this.userCode = userCode; }

        public DateTime getMessageDate()
        { return messageDate; }

        public void setMessageDate(DateTime messageDate)
        { this.messageDate = messageDate; }

        public DateTime getTimeout()
        { return timeout; }

        public void setTimeout(DateTime timeout)
        { this.timeout = timeout; }

        public String getAgancyCode()
        { return agancyCode; }

        public void setAgancyCode(String agancyCode)
        { this.agancyCode = agancyCode; }

        public String getActionType()
        { return actionType; }

        public void setActionType(String actionType)
        { this.actionType = actionType; }

        public String getActionCode()
        { return actionCode; }

        public void setActionCode(String actionCode)
        { this.actionCode = actionCode; }

        public int getActionPriority()
        { return actionPriority; }

        public void setActionPriority(int actionPriority)
        { this.actionPriority = actionPriority; }

        public int getConnectionType()
        { return connectionType; }

        public void setConnectionType(int connectionType)
        { this.connectionType = connectionType; }

        public int getActionStatus()
        { return actionStatus; }

        public void setActionStatus(int actionStatus)
        { this.actionStatus = actionStatus; }

        public String getComments()
        { return comments; }

        public void setComments(String comments)
        { this.comments = comments; }

        public Dictionary<String, String> getParameters()
        { return parameters; }

        public void setParameters(Dictionary<String, String> parameters)
        { this.parameters = parameters; }



        public String toXML()
        {
            String xml = "";

            xml += "<DTM_MESSAGE>";
            xml += "<USER>" + getUserCode() + "</USER>";
            xml += "<DATE_TIME>"+ getMessageDate().ToString("G") + "</DATE_TIME>";
            xml += "<AGENCY_CODE>" + getAgancyCode() + "</AGENCY_CODE>";
            xml += "<ACTION_TYPE>" + getActionType() + "</ACTION_TYPE>";
            xml += "<ACTION_CODE>" + getActionCode() + "</ACTION_CODE>";
            xml += "<PRIORITY>" + getActionPriority().ToString() + "</PRIORITY>";
            xml += "<CONNECTION>" + getConnectionType().ToString() + "</CONNECTION>";
            xml += "<STATUS>" + getActionStatus().ToString() + "</STATUS>";
            xml += "<TIMEOUT>" + getTimeout().ToString("G") + "</TIMEOUT>";
            xml += "<COMMENTS>" + getComments() + "</COMMENTS>";
            xml += "<PARAMETERS>";
            foreach (String key in getParameters().Keys)
            {
                xml += "<PARAMETER name=\"" + key + "\">" + getParameters()[key] + "</PARAMETER>";
            }            
            xml += "</PARAMETERS>";
            xml += "</DTM_MESSAGE>";

            return xml;
        
        }

    }
}
