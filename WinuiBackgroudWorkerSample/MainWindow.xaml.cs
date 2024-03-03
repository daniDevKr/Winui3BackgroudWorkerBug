using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace WinuiBackgroudWorkerSample
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private string TaskName => "ExampleToastTask";

        public MainWindow()
        {
            this.InitializeComponent();
            RegisterTaskBtn.Loaded += RegisterTaskBtn_Loaded;
        }


        private void UpdateLabelText()
        {
            var s = TaskIsRegistered(TaskName) ? "Registered" : "Not registered";
            RegistrationStateTxt.Text = $"{TaskName} task {s}";
        }
        private void RegisterTaskBtn_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterTaskBtn.IsEnabled = !TaskIsRegistered(TaskName);
           UnregisterTaskBtn.IsEnabled = TaskIsRegistered(TaskName);

            UpdateLabelText();
        }

        private void RegisterTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            var builder = new BackgroundTaskBuilder
            {
                Name = TaskName,
                TaskEntryPoint = "Samples.Tasks.ExampleToastTask"
            };
            builder.SetTrigger(new SystemTrigger(SystemTriggerType.TimeZoneChange, false));
            builder.Register();
            RegisterTaskBtn.IsEnabled = false;
            UnregisterTaskBtn.IsEnabled = true;
            UpdateLabelText();
        }

        private void UnregisterTaskBtn_Click(object sender, RoutedEventArgs e)
        {
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == TaskName)
                {
                    task.Value.Unregister(true);
                }
            }

            RegisterTaskBtn.IsEnabled = true;
            UnregisterTaskBtn.IsEnabled = false;
            UpdateLabelText();
        }

        private static bool TaskIsRegistered(string taskName)
        {
            bool result = false;
            foreach (var task in BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == taskName)
                {
                    result = true;
                    break;
                }
            }

            return result;  
        }
    }
}
