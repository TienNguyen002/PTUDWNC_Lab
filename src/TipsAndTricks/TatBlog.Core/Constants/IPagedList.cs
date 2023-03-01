using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TatBlog.Core.Constants
{
    public interface IPagedList
    {      
        int PageCount { get; }          //Tổng số trang (số tập con)

        int TotalItemCount { get; }     //Tổng số phần tử trả về từ truy vấn

        int PageIndex { get; }          //Chỉ số trang hiện tại (bắt đầu từ 0)

        int PageNumber { get; }         //Vị trí của trang hiện tại (bắt đầu từ 1)

        int PageSize { get; }           //Số lượng phần tử tối đa trên 1 trang

        bool HasPreviousPage { get; }   //Kiểm tra có trang trước hay không
        
        bool HasNextPage { get; }       //Kiểm tra có trang tiếp theo hay không

        bool IsFirstPage { get; }       //Trang hiện tại có phải là trang đầu tiên không

        bool IsLastPage { get; }        //Trang hiện tại có phải là trang cuối cùng không

        int FirstItemIndex { get; }     //Thứ tự của phần tử đầu trang trong truy vấn (bắt đầu từ 1)

        int LastItemIndex { get; }      //Thứ tự của phần tử cuối trang trong truy vấn (bắt đầu từ 1)
    }

    public interface IPagedList<out T>: IPagedList, IEnumerable<T>
    { 
        T this[int index] { get; }      //Lấy phần tử tại vị trí index (bắt đầu từ 0)

        int Count { get; }              //Đếm số lượng phần tử chứa trong trang
    }
}
