using System.Collections.Generic;
using System.Linq;
using SharedKernel.Application.Common.Models;
using Shouldly;
using Xunit;

namespace SharedKernel.UnitTests.Application.Common.Models;

public class PagedListTests
{
    public PagedListTests() { }

    [Fact]
    public void pagedlist_returns_accurate_data_for_standard_pagination()
    {
        const int pageNumber = 2;
        const int pageSize = 2;
        var source = new List<int> { 1, 2, 3, 4, 5 }.AsQueryable();

        var list = PagedList<int>.Create(source, pageNumber, pageSize);
        
        list.TotalCount.ShouldBe(5);
        list.PageSize.ShouldBe(2);
        list.PageNumber.ShouldBe(2);
        list.CurrentPageSize.ShouldBe(2);
        list.CurrentStartIndex.ShouldBe(3);
        list.CurrentEndIndex.ShouldBe(4);
        list.TotalPages.ShouldBe(3);
    }
    
    [Fact]
    public void pagedlist_returns_accurate_data_with_last_record()
    {
        const int pageNumber = 3;
        const int pageSize = 2;
        var source = new List<int> { 1, 2, 3, 4, 5 }.AsQueryable();

        var list = PagedList<int>.Create(source, pageNumber, pageSize);
        list.TotalCount.ShouldBe(5);
        list.PageSize.ShouldBe(2);
        list.PageNumber.ShouldBe(3);
        list.CurrentPageSize.ShouldBe(1);
        list.CurrentStartIndex.ShouldBe(5);
        list.CurrentEndIndex.ShouldBe(5);
        list.TotalPages.ShouldBe(3);
    }
}