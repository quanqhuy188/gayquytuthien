namespace GayQuyTuThien.Models
{
    public class ResultModel<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; }
        public string? ErrorMessenger { get; set; }
        public ResultModel()
        {
            IsSuccess = true;
        }
        public ResultModel(T? obj)
        {
            Data = obj;
            IsSuccess = true;
        }
        public ResultModel(bool isSuccess, string? errorMessenger, T? obj)
        {
            IsSuccess = isSuccess;
            ErrorMessenger = errorMessenger;
            Data = obj;
        }
        public ResultModel(bool isSuccess, string? errorMessenger)
        {
            IsSuccess = isSuccess;
            ErrorMessenger = errorMessenger;
        }
        public ResultModel(string? errorMessenger)
        {
            IsSuccess = false;
            ErrorMessenger = errorMessenger;
        }
    }
}
