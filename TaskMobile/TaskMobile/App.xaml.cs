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
            NavigationService.NavigateAsync("LoginPage");
        }

        /// <summary>
        /// When adding new views you must register them views in this method 
        /// </summary>
        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<Views._Detail>("DetailPage");
            Container.RegisterTypeForNavigation<Views.Login>("LoginPage");
            Container.RegisterTypeForNavigation<Views.Tasks.AssignedToExecuted>("AssignedToExecuted");
            Container.RegisterTypeForNavigation<Views.Tasks.Assigned >("AssignedTasks");
            Container.RegisterTypeForNavigation<Views.Tasks.Executed>("ExecutedTask");
            Container.RegisterTypeForNavigation<Views.Tasks.Canceled>("CanceledTask");
            Container.RegisterTypeForNavigation<Views.Tasks.Rejected>("RejectedTask");
            Container.RegisterTypeForNavigation<Views.Tasks.QueryExecuted>("QueryExecuted");
            Container.RegisterTypeForNavigation<Views.Tasks.ExecutedToFinish>("ExecutedToFinish");
            Container.RegisterTypeForNavigation<Views.Tasks.Finished>("Finished");
            Container.RegisterTypeForNavigation<Views.Tasks.QueryFinished>("QueryFinished");
            Container.RegisterTypeForNavigation<Views.Tasks.FinishDetails>("FinishDetails");
            Container.RegisterTypeForNavigation<Views.Tasks.QueryRejected>("QueryRejected");
            Container.RegisterTypeForNavigation<Views.Tasks.RejectedDetail>("RejectedDetail");
            Container.RegisterTypeForNavigation<Views.Vehicle.Change>("ChangeVehicle");
        }
    }
}
