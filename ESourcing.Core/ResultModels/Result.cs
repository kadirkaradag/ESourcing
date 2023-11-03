namespace ESourcing.Core.ResultModels
{
    public class Result<T> : IResult // generic bi result yapmamızın sebebi sadece uzak sunucudan gelen datayı değil isSuccess, message, total count gibi değerleri de dönebilmek.
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public int TotalCount { get; set; }

        public Result(bool isSuccess,string message) : this(isSuccess, message, default(T))
        {

        }

        public Result(bool isSuccess, string message, T data) : this(isSuccess, message,data,0)
        {
            
        }

        public Result(bool isSuccess,string message,T data,int totalCount)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
            TotalCount = totalCount;
        }


    }
}
