using Microsoft.AspNetCore.Mvc;

namespace FlapKap.Models
{
    public class SaveResultWithSavedObject<T>
    {
        public bool IsSuccess { get; set; }
        public T SavedObject { get; set; }
        public string Message { get; set; }

        public SaveResultWithSavedObject(bool isSuccess, T savedObject = default, string message = "")
        {
            IsSuccess = isSuccess;
            SavedObject = savedObject;
            Message = message;
        }
    }
}
