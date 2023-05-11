using Xamarin.Essentials;
using Xamarin.Forms;

namespace Mobile_app
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new TabbedPage1();
        }

        protected override void OnStart()
        {
            try
            {
                var permission_micro = Permissions.CheckStatusAsync<Permissions.Microphone>();
                if (permission_micro.Result != PermissionStatus.Granted)
                {
                    permission_micro = Permissions.RequestAsync<Permissions.Microphone>();
                }

            }
            catch { }
        }

        protected override void OnSleep()
        {
            
        }

        protected override void OnResume()
        {
        }
    }
}
