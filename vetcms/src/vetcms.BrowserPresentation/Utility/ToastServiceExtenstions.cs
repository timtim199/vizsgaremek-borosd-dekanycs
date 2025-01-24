using Microsoft.FluentUI.AspNetCore.Components;

namespace vetcms.BrowserPresentation.Services
{
    internal static class ToastServiceExtenstions
    {
        public static string ShowIndeterminateProgressToast(this IToastService toastService, string title, string? description, int closeAfter = int.MaxValue)
        {
            Console.WriteLine("ToastBuilderService.ShowIndeterminateProgressToast");
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
            toastService.ShowProgressToast(toastOptions);
            return toastId;
        }

        public static Task<T> ShowIndeterminateProgressToast<T>(this IToastService toastService, Task<T> Until, string title, string? description, int closeAfter = int.MaxValue)
        {
            string toastId = toastService.ShowIndeterminateProgressToast(title, description, closeAfter);
            Until.ContinueWith((task) =>
            {
                toastService.CloseToast(toastId);
            });

            return Until;
        }

        public static Task ShowIndeterminateProgressToast(this IToastService toastService, Task Until, string title, string? description, int closeAfter = int.MaxValue)
        {
            string toastId = toastService.ShowIndeterminateProgressToast(title, description, closeAfter);
            Until.ContinueWith((task) =>
            {
                toastService.CloseToast(toastId);
            });

            return Until;
        }
    }

    
}