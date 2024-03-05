using System.Diagnostics;
using System.Threading.Tasks;

using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Samples.Tasks
{
    public sealed class ExampleToastTask: IBackgroundTask
    {
        private BackgroundTaskDeferral? _taskDeferral;
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            taskInstance.Canceled += TaskInstance_Canceled;
            Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");
            _taskDeferral = taskInstance.GetDeferral();


            for (int i = 0; i < 30; i++)//simulate long operation
            {
                await Task.Delay(1000);
                Debug.WriteLine($"Runnig time {i} sec.");

            }


          
            SendToast();//show example toast

            Debug.WriteLine("Background " + taskInstance.Task.Name + " Completed.");
            _taskDeferral.Complete();
        }

        private static void SendToast()
        {
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText02);
            XmlNodeList textElements = toastXml.GetElementsByTagName("text");
            textElements[0].AppendChild(toastXml.CreateTextNode("A toast example"));
            textElements[1].AppendChild(toastXml.CreateTextNode("Hello world by task!"));
            ToastNotification notification = new(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(notification);
        }

        private void TaskInstance_Canceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            if (_taskDeferral != null)
            {
                _taskDeferral.Complete();
                Debug.WriteLine("Background task terminated");
            }
        }
    }
}
