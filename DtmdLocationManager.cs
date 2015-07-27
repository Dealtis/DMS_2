using System;
using System.Linq;
using System.Collections.Generic;
using Android.Locations;
using Android.OS;

namespace DMSvStandard
{
	/// <summary>
	///  This class is GPS listener and used for retreiving GPS possition changes.
	/// </summary>
	class MyGPSListener:Java.Lang.Object, GpsStatus.IListener {

		private DtmdLocationManager _DtmdLocationManager=null;

		public MyGPSListener(DtmdLocationManager _location)
		{
			_DtmdLocationManager = _location;
		}
		public void OnGpsStatusChanged(Android.Locations.GpsEvent evnt)
		{
			switch (evnt) {
			case GpsEvent.SatelliteStatus:
				if (_DtmdLocationManager.getLocationManager() != null)
				{
					if((SystemClock.ElapsedRealtime() - _DtmdLocationManager.getLastLocationMillis()) < _DtmdLocationManager.getLocationUpdateInterval()*2)
					{
						_DtmdLocationManager.setLocationEnabled (true);
					} 
					else
					{
						_DtmdLocationManager.setLocationEnabled (false);
					}
				}
				break;
			case GpsEvent.FirstFix:
				_DtmdLocationManager.setLocationEnabled (true);
				break;
			case GpsEvent.Started:
				break;
			case GpsEvent.Stopped:
				break;
			}



		}
	}

	/// <summary>
	///  This class reponsible for starting GPS possition listening depending on available provider.
	/// </summary>
	public class DtmdLocationManager:Java.Lang.Object, ILocationListener
	{
		int locationType = 0; // 0 - GPS, 1 - Network
		Location _currentLocation = null;
		LocationManager _locationManager = null;
		String _locationProvider="";
		int locationUpdateInterval=0;
		bool locationEnabled = true;
		long mLastLocationMillis=0;

		public DtmdLocationManager (int _type)
		{
			locationType = _type;
		}

		public int getLocationUpdateInterval()
		{
			return locationUpdateInterval;
		}

		public long getLastLocationMillis()
		{
			return mLastLocationMillis;
		}

		public void setLocationEnabled(bool _enabled)
		{
			locationEnabled = _enabled;
		}

		public bool isLocationEnabled()
		{
			return locationEnabled;
		}

		public LocationManager getLocationManager()
		{
			return _locationManager;
		}

		public Location getCurrentLocation()
		{	

			ApplicationData.GPS = ""+ (Convert.ToString(_currentLocation.Latitude)).Replace(',','.')+","+(Convert.ToString(_currentLocation.Longitude)).Replace(',','.')+"";
			return _currentLocation;
		}

		public void OnLocationChanged(Location location)
		{
			if (location == null)
				return;

			_currentLocation = location;
			mLastLocationMillis = SystemClock.ElapsedRealtime ();
		}

		public void OnProviderDisabled(string provider) 
		{
			setLocationEnabled (false);
		}

		public void OnProviderEnabled(string provider) 
		{
			setLocationEnabled (true);
		}



		public void OnStatusChanged(string provider, Availability status, Bundle extras)
		{

		}

		public void startLocation(int _interval)
		{
			locationUpdateInterval = _interval;

			if (_locationManager != null)
				_locationManager.RemoveUpdates (this);

			_locationManager = (LocationManager) MainActivity.getContext().GetSystemService(MainActivity.LocationService);

			Criteria criteriaForLocationService = new Criteria ();
			if (locationType == 0) {
				criteriaForLocationService.Accuracy = Accuracy.Fine;
			} else {
				criteriaForLocationService.Accuracy = Accuracy.Coarse;
			}


			_locationProvider = _locationManager.GetBestProvider(criteriaForLocationService, true);

			if (locationType == 0) {
				_locationManager.AddGpsStatusListener (new MyGPSListener (this));
			}

			_locationManager.RequestLocationUpdates(_locationProvider, locationUpdateInterval, 0, this);
		}

		public void stopLocation()
		{
			if (_locationManager != null)
				_locationManager.RemoveUpdates (this);

			_currentLocation = null;
			setLocationEnabled (false);
		}
	}
}

