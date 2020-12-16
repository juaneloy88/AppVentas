package md59f375f2279bef1a65cf506e3a3602dfe;


public class Utilerias
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("Base.Utilerias, VentasCCA.Android", Utilerias.class, __md_methods);
	}


	public Utilerias ()
	{
		super ();
		if (getClass () == Utilerias.class)
			mono.android.TypeManager.Activate ("Base.Utilerias, VentasCCA.Android", "", this, new java.lang.Object[] {  });
	}

	private java.util.ArrayList refList;
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
