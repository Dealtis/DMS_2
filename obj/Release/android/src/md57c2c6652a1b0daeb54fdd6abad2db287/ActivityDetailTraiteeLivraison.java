package md57c2c6652a1b0daeb54fdd6abad2db287;


public class ActivityDetailTraiteeLivraison
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("DMSvStandard.ActivityDetailTraiteeLivraison, DMSvStandard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", ActivityDetailTraiteeLivraison.class, __md_methods);
	}


	public ActivityDetailTraiteeLivraison () throws java.lang.Throwable
	{
		super ();
		if (getClass () == ActivityDetailTraiteeLivraison.class)
			mono.android.TypeManager.Activate ("DMSvStandard.ActivityDetailTraiteeLivraison, DMSvStandard, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

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
