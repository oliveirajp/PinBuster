using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using PinBuster.Data;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;
using System.Diagnostics;
using PinBuster.Pages;

namespace PinBuster.ViewModels
{
    public class MapViewModel : ViewModelBase
    {
        public static readonly Position NullPosition = new Position(0, 0);

        public ObservableCollection<PinBuster.Models.Pin> Pins = new ObservableCollection<PinBuster.Models.Pin>();

        public MapViewModel()
        {
            Task.Run(() => LoadPins());
        }

        public async Task LoadPins()
        {
            var pins = await App.pinsManager.FetchPins();
            if (pins != null)
                try
                {
                    List<PinBuster.Models.Pin> myList = new List<PinBuster.Models.Pin>(Pins);
                    if (myList.Count > 0)
                        foreach (var x in myList)
                        {
                            if (!pins.Contains(x))
                                Pins.Remove(x);

                        }
                    if (pins.Count > 0)
                        foreach (var pin in pins)
                        {
                            if (pin.Categoria == "Secret")
                            {
                                if (pin.Visivel == 1)
                                {
                                    if (!Pins.Contains(pin))
                                        Pins.Add(pin);
                                }
                            }
                            else if (!Pins.Contains(pin))
                                Pins.Add(pin);

                            //await Task.Delay(500);
                        }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(@"@@@@@@@@@@@@@@@@@@@ERROR {0}", ex.Message);
                }
           
        }
    }
}
