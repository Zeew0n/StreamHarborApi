using System;
using System.Collections.Generic;
using System.Text;

namespace WorkFlowTaskManager.Application.DTO.User.Response
{
    public class UserLoginResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
