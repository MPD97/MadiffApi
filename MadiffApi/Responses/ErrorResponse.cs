﻿namespace MadiffApi.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string TraceId { get; set; }
        public IDictionary<string, string[]> Errors { get; set; }
    }
}
