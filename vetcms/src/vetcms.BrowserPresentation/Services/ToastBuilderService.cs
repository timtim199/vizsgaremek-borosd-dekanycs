using Microsoft.FluentUI.AspNetCore.Components;

namespace vetcms.BrowserPresentation.Services
{
    internal class ToastBuilderService(IToastService ToastService)
    {
        public string ShowIndeterminateProgressToast(string title, string? description, int closeAfter = int.MaxValue)
        {
            string toastId = Guid.NewGuid().ToString();
            ToastParameters<ProgressToastContent> toastOptions = new()
            {
                Id = toastId,
                Intent = ToastIntent.Progress,
                Title = title,
                Timeout = closeAfter,
                Content = new ProgressToastContent()
                {
                    Details = description,
                },
            };
            ToastService.ShowProgressToast(toastOptions);
            return toastId;
        }

        public void CloseToast(string toastId)
        {
            ToastService.CloseToast(toastId);
        }
    }

    
}