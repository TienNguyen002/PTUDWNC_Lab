using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Constants
{
    public interface IPagingParams
    {  
        int PageSize { get; set; }          //Số mẫu tin trên một trang
        
        int PageNumber { get; set; }        //Số trang tính bắt đầu từ 1
        
        string SortColumn { get; set; }     //Tên cột muốn sắp xếp
        
        string SortOrder { get; set; }      //Thứ tự sắp xếp: tăng hay giảm
    }
}
