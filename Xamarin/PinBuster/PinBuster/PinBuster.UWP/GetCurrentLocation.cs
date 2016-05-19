using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI.Core;

namespace PinBuster.UWP
{

    public class LocationEventArgs : EventArgs, ILocationEventArgs
    {
        public double lat { get; set; }
        public double lng { get; set; }
    }

    class GetCurrentLocation : IGetCurrentPosition
    {
        public event EventHandler<ILocationEventArgs> locationObtained;
        LocationEventArgs args;
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

        public async void IGetCurrentPosition()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                    var geoLocator = new Geolocator { };
                    geoLocator.ReportInterval = 3000;
                    geoLocator.MovementThreshold = 10;
                    geoLocator.DesiredAccuracy = PositionAccuracy.High;

                    // Subscribe to StatusChanged event to get updates of location status changes

                    geoLocator.PositionChanged += OnPositionChanged;

                    try
                    {
                        // Carry out the operation
                        Geoposition pos = await geoLocator.GetGeopositionAsync();
                        args = new LocationEventArgs();
                        UpdateLocationData(pos);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error: " + ex.Message);
                    }
                    break;

                case GeolocationAccessStatus.Denied:
                    System.Diagnostics.Debug.WriteLine("Error: location permission denied");
                    break;

                case GeolocationAccessStatus.Unspecified:
                    System.Diagnostics.Debug.WriteLine("Error: unspecified");
                    break;
            }

        }

        private async void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            var coreWindow = Windows.ApplicationModel.Core.CoreApplication.MainView;
            await coreWindow.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UpdateLocationData(args.Position);
            });
        }

        private void UpdateLocationData(Geoposition pos)
        {
            if (pos != null)
            {
                args.lat = pos.Coordinate.Point.Position.Latitude;
                args.lng = pos.Coordinate.Point.Position.Longitude;
              
                try
                {
                    locationObtained(this, args);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Error: " + e);
                }
            };
        }

        ~GetCurrentLocation()
        {
        }
    }
}
