package md57c2c6652a1b0daeb54fdd6abad2db287;


public class MyGPSListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		android.location.GpsStatus.Listener
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onGpsStatusChanged:(I)V:GetOnGpsStatusChanged_IHandler:Android.Locations.GpsStatus/IListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n" +
			"";
		mono.android.Runtime.register ("DMSvStandard.MyGPSListener, DMSvStandard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", MyGPSListener.class, __md_methods);
	}


	public MyGPSListener () throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyGPSListener.class)
			mono.android.TypeManager.Activate ("DMSvStandard.MyGPSListener, DMSvStandard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public MyGPSListener (md57c2c6652a1b0daeb54fdd6abad2db287.DtmdLocationManager p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == MyGPSListener.class)
			mono.android.TypeManager.Activate ("DMSvStandard.MyGPSListener, DMSvStandard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "DMSvStandard.DtmdLocationManager, DMSvStandard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", this, new java.lang.Object[] { p0 });
	}


	public void onGpsStatusChanged (int p0)
	{
		n_onGpsStatusChanged (p0);
	}

	private native void n_onGpsStatusChanged (int p0);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
