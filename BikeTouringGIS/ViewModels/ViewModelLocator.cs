using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace BikeTouringGIS.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainScreenViewModel>();
        }

        public MainScreenViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainScreenViewModel>();
            }
        }
    }
}