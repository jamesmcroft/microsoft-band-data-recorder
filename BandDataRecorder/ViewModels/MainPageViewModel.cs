// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainPageViewModel.cs" company="James Croft">
//   Copyright (c) 2015 James Croft.
// </copyright>
// <summary>
//   The MainPage.xaml view model.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace BandDataRecorder.ViewModels
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Windows.System;
    using Windows.System.Profile;
    using Windows.UI.Popups;

    using Croft.Core.Diagnostics;
    using Croft.Core.Helpers;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Microsoft.Band;

    /// <summary>
    /// The MainPage.xaml view model.
    /// </summary>
    public class MainPageViewModel : ViewModelBase
    {
        private IBandClient _bandClient;

        private bool _isBandConnected;

        private MessageDialogHelper _dialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPageViewModel"/> class.
        /// </summary>
        /// <param name="dialog">
        /// The dialog.
        /// </param>
        public MainPageViewModel(MessageDialogHelper dialog)
        {
            if (dialog == null)
            {
                throw new ArgumentNullException(nameof(dialog));
            }

            this._dialog = dialog;

            this.ConnectToBandCommand = new RelayCommand(async () => await this.ConnectToBandAsync());
        }

        /// <summary>
        /// Gets the command responsible for connecting to the Microsoft Band.
        /// </summary>
        public ICommand ConnectToBandCommand { get; }

        private async Task ConnectToBandAsync()
        {
            try
            {
                var availableBands = await BandClientManager.Instance.GetBandsAsync();
                var band = availableBands.FirstOrDefault();

                if (band == null)
                {
                    await
                        this._dialog.ShowAsync(
                            "A Microsoft Band is not connected to this device. Please check your Bluetooth connection and try again.",
                            new UICommand(
                                "Check settings",
                                async command => { await Launcher.LaunchUriAsync(new Uri("ms-settings:bluetooth")); }));

                    return;
                }

                this._bandClient = await BandClientManager.Instance.ConnectAsync(band);
                if (this._bandClient == null)
                {
                    return;
                }

                this.IsBandConnected = true;
            }
            catch (BandException be)
            {
                Logger.Log.Error(be.Message);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a Microsoft Band is connected.
        /// </summary>
        public bool IsBandConnected
        {
            get
            {
                return this._isBandConnected;
            }
            set
            {
                this.Set(() => this.IsBandConnected, ref this._isBandConnected, value);
            }
        }
    }
}