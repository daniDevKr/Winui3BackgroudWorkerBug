using System.Diagnostics;

using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace Samples.Tasks
{
    public sealed class ExampleToastTask: IBackgroundTask
    {

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            Debug.WriteLine("Background " + taskInstance.Task.Name + " Starting...");

            // Perform the background task.
            SendToast();
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
    }
}
