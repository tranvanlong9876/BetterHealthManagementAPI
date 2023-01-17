using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BetterHealthManagementAPI.BetterHealth2023.Repository.ViewModels.PagingModels
{
    public class PagedResult<T>
    {

        public PagedResult(List<T> items, int totalRecord, int pageIndex, int pageItems) {
            Items = items;
            TotalRecord = totalRecord;
            TotalPage = (int) Math.Ceiling((double) totalRecord / pageItems);
            HasNextPage = pageIndex < TotalPage;
            HasPreviousPage = pageIndex > 1;
        }
        //danh sách sau khi đã phân trang.
        public List<T> Items { get; set; }

        //tổng record nếu không có phân trang
        public int TotalRecord { get; }
        public int TotalPage { get; }
        public bool HasNextPage { get; }
        public bool HasPreviousPage { get; }
    }
}
