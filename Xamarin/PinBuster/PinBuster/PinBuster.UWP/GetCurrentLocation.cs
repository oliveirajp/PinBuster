using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

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
            var geoLocator = new Geolocator();
            geoLocator.DesiredAccuracy = PositionAccuracy.High;
            Geoposition pos = await geoLocator.GetGeopositionAsync();

            if (pos != null)
            {
                LocationEventArgs args = new LocationEventArgs();
                args.lat = pos.Coordinate.Point.Position.Latitude;
                args.lng = pos.Coordinate.Point.Position.Longitude;
                locationObtained(this, args);
            };

        }
    }
}
