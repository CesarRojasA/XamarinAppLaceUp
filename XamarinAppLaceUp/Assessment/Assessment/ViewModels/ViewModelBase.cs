using Prism.Mvvm;
using Prism.Navigation;

namespace Assessment.ViewModels
{
    public class ViewModelBase : BindableBase, IInitialize, INavigationAware, IDestructible
    {
        #region Properties

        protected INavigationService NavigationService { get; private set; }

        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        #endregion

        #region Constructors

        public ViewModelBase(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #endregion

        #region IInitialize Implementation

        public virtual void Initialize(INavigationParameters parameters)
        {
        }

        #endregion

        #region INavigationAware Implementation

        public virtual void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public virtual void OnNavigatedTo(INavigationParameters parameters)
        {
        }

        #endregion

        #region IDestructible Implementation

        public virtual void Destroy()
        {
        }

        #endregion
    }
}
