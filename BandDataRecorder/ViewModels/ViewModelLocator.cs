namespace BandDataRecorder.ViewModels
{
    using Croft.Core.Helpers;

    using GalaSoft.MvvmLight.Ioc;

    using Microsoft.Practices.ServiceLocation;

    public class ViewModelLocator
    {
        private static bool servicesRegistered;

        private static bool viewModelsRegistered;

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            RegisterServices();
            RegisterViewModels();
        }

        public MainPageViewModel MainPageViewModel => SimpleIoc.Default.GetInstance<MainPageViewModel>();

        private static void RegisterViewModels()
        {
            if (viewModelsRegistered)
            {
                return;
            }

            viewModelsRegistered = true;

            SimpleIoc.Default.Register<MainPageViewModel>();
        }

        private static void RegisterServices()
        {
            if (servicesRegistered)
            {
                return;
            }

            servicesRegistered = true;

            SimpleIoc.Default.Register<MessageDialogHelper>();
        }
    }
}
