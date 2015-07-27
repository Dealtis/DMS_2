using System;
using System.Collections.Generic;
using System.Text;

namespace DMSvStandard
{
    class ThreadStartWraper
    {
        private DealtisMessage Msg;

        public ThreadStartWraper(DealtisMessage _msg)
        {
            Msg = _msg;
        }   


        public void WorkerThreadFunction()
		{
            RequestProcessor th;

            th = new RequestProcessor(Msg);

            ServerData.Instance.getRunningThreads().Add(th);

            th.Run();
            

            //ApplicationData.Instance.getRunningThreads().Remove(th);
        }

    }
}
