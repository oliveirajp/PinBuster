using System;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using PinBuster.ViewModels;

namespace PinBuster
{
	public class Locator
	{
		/// <summary>
		/// Register all the used ViewModels, Services et. al. witht the IoC Container
		/// </summary>
		public Locator()
		{
			ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

			// ViewModels
			SimpleIoc.Default.Register<MapViewModel>();
		}

		/// <summary>
		/// Gets the Main property.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
			"CA1822:MarkMembersAsStatic",
			Justification = "This non-static member is needed for data binding purposes.")]
		public MapViewModel Map
		{
			get
			{
				return ServiceLocator.Current.GetInstance<MapViewModel>();
			}
		}
	}
}
