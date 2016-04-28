using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBuster
{

    public interface IGetCurrentPosition
    {
       void IGetCurrentPosition();
       event EventHandler<ILocationEventArgs> locationObtained;
    }

    public interface ILocationEventArgs
    {
        double lat { get; set; }
        double lng { get; set; }
    }
}
