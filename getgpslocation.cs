using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Widget;

namespace DMSvStandard
{
    
	class getgpslocation: Activity, ILocationListener
    {
        static readonly string LogTag = "GetLocation";
        TextView _addressText;
        Location _currentLocation;
        LocationManager _locationManager;
		private static getgpslocation instance;
        string _locationProvider;
        TextView _locationText;

        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                _locationText.Text = "Unable to determine your location.";
            }
            else
            {
                _locationText.Text = String.Format("{0},{1}", _currentLocation.Latitude, _currentLocation.Longitude);
				_locationText.Text = ApplicationData.GPS;
            }
        }

		public static getgpslocation Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new getgpslocation();
				}
				return instance;
			}
		}

        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {
            Log.Debug(LogTag, "{0}, {1}", provider, status);
        }

        

		public void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
                                                  {
                                                      Accuracy = Accuracy.Fine
                                                  };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = String.Empty;
            }
            Log.Debug(LogTag, "Using " + _locationProvider + ".");
        }

        async void AddressButton_OnClick(object sender, EventArgs eventArgs)
        {
            if (_currentLocation == null)
            {
                _addressText.Text = "Can't determine the current address.";
                return;
            }

            Geocoder geocoder = new Geocoder(this);
            IList<Address> addressList = geocoder.GetFromLocation(_currentLocation.Latitude, _currentLocation.Longitude, 10);

            Address address = addressList.FirstOrDefault();
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.Append(address.GetAddressLine(i))
                                 .AppendLine(",");
                }
                _addressText.Text = deviceAddress.ToString();
            }
            else
            {
                _addressText.Text = "Unable to determine the address.";
            }
        }
    }
}
