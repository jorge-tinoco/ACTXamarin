using Xamarin.Forms;
using TaskMobile.DB;
using Prism.Unity;

namespace TaskMobile
{
    public partial class App : PrismApplication
    {
        public static MasterDetailPage MasterD { get; set; }
        public static EmployeeREPO EmployeeRepo;


        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
        }

        protected override void OnInitialized()
        {
            InitializeComponent();
            NavigationService.NavigateAsync("MainPage");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<Views.Tasks.AssignedToExecuted>("AssignedToExecuted");
            Container.RegisterTypeForNavigation<Views._Detail>("DetailPage");
            Container.RegisterTypeForNavigation<Views.Tasks.Assigned , TaskMobile.ViewModels.TaskViewModel>("AssignedTasks");
        }
    }
}
