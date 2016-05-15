using System;
using Android.Content;
using PinBuster.Droid;
using Android.Locations;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using Android.Util;

[assembly: Xamarin.Forms.Dependency(typeof(GetCurrentLocation))]
namespace PinBuster.Droid
{
    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }


    class GetCurrentLocation : Java.Lang.Object, IGetCurrentPosition, ILocationListener
    {
        LocationManager lm;
        public void OnProviderDisabled(string provider) { }
        public void OnProviderEnabled(string provider) { }
        public void OnStatusChanged(string provider, Availability status, Android.OS.Bundle extras) { }
        public void OnLocationChanged(Location location)
        {
            if (location != null)
            {
                LocationEventArgs args = new LocationEventArgs();
                args.lat = location.Latitude;
                args.lng = location.Longitude;
                locationObtained(this, args);
            };
        }
        
        //---an EventHandler delegate that is called when a location is obtained
        public event EventHandler<ILocationEventArgs> locationObtained;

        //---custom event accessor that is invoked when client subscribes to the event
        event EventHandler<ILocationEventArgs> IGetCurrentPosition.locationObtained
        {
            add
            {
                locationObtained += value;
            }
            remove
            {
                locationObtained -= value;
            }
        }

        //method to call to start getting location
        public void IGetCurrentPosition()
        {
            string locationProvider;

            lm = (LocationManager) Forms.Context.GetSystemService(Context.LocationService);

            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = lm.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                locationProvider = string.Empty;
            }

            lm.RequestLocationUpdates(locationProvider, 2000, 1, this);
        }
       
        ~GetCurrentLocation()
        {
            lm.RemoveUpdates(this);
        }
    }
}