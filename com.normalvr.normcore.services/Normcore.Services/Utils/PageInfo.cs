using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Normcore.Services
{
    /// <summary>
    /// A struct used to request a page of results.
    /// </summary>
    /// <remarks>
    /// To ensure consistent performance, for some APIs the Normcore Services backend limits the number of
    /// items that can be returned in a single response. So, to retrieve all items in a list (ex. all available
    /// lobbies), clients need to retrieve them in fixed-size batches, known as pages. The <see cref="ByCursor"/>
    /// and <see cref="ByOffset"/> methods can be used to control how the pages are retrieved by the client.
    /// </remarks>
    public readonly struct PageInfo
    {
        const int DefaultCountPerPage = 25;
        
        // TODO: make public when cursor pagination is supported.
        /// <summary>
        /// The continuation token used to retrieve the next page, or <see langword="null"/> if not using cursor pagination.
        /// </summary>
        internal readonly string ContinuationToken;

        /// <summary>
        /// The offset of the first item in the page, or <see langword="null"/> if not using offset pagination.
        /// </summary>
        public readonly int? Offset;
        
        /// <summary>
        /// The number of items to retrieve per page.
        /// </summary>
        public readonly int Count;
        
        /// <summary>
        /// The number of the page, indexed from 0.
        /// </summary>
        public readonly int PageNumber;
        
        /// <summary>
        /// Gets the default page info used if one is not supplied to a paged request.
        /// </summary>
        internal static PageInfo Default()
        {
            // TODO: switch to cursor pagination once cursor pagination is supported. 
            return ByOffset(0);
        }
        
        // TODO: make public when cursor pagination is supported.
        /// <summary>
        /// Creates a page request using cursor pagination.
        /// </summary>
        /// <remarks>
        /// Cursor pagination is generally preferred over offset pagination. Items may be inserted or removed while
        /// iterating the pages, without items being skipped or duplicated in the resulting pages.
        /// </remarks>
        /// <param name="count">The number of items to retrieve per page. Must be non-negative.</param>
        /// <returns>A page request using cursor pagination.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="count"/> is invalid.</exception>
        /// <seealso cref="ByOffset"/>
        internal static PageInfo ByCursor(int count = DefaultCountPerPage)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must not be negative.");
            }

            return new PageInfo(null, count, 0);
        }

        /// <summary>
        /// Creates a page request using offset pagination.
        /// </summary>
        /// <remarks>
        /// Offset pagination is useful when needing to skip past a large number of items. However, items may
        /// be inserted or removed while iterating the pages, causing items to be skipped or duplicated in the
        /// resulting pages. To avoid this, consider using cursor pagination instead.
        /// </remarks>
        /// <param name="offset">The offset of the first item in the page. Must be non-negative.</param>
        /// <param name="count">The number of items to retrieve per page. Must be non-negative.</param>
        /// <returns>A page request using offset pagination.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="offset"/> or <paramref name="count"/> is invalid.</exception>
        /// <seealso cref="ByCursor"/>
        public static PageInfo ByOffset(int offset, int count = DefaultCountPerPage)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset), offset, "Must not be negative.");
            }
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), count, "Must not be negative.");
            }

            return new PageInfo(offset, count, 0);
        }
        
        internal PageInfo(int offset, int count, int pageNumber)
        {
            ContinuationToken = null;
            Offset = offset;
            Count = count;
            PageNumber = pageNumber;
        }
        
        internal PageInfo(string continuationToken, int count, int pageNumber)
        {
            ContinuationToken = continuationToken;
            Offset = null;
            Count = count;
            PageNumber = pageNumber;
        }
        
        internal PageInfo? NextPage(string responseContinuationToken)
        {
            if (Offset.HasValue)
            {
                return new PageInfo(Offset.Value + Count, Count, PageNumber + 1);
            }
            if (!string.IsNullOrEmpty(responseContinuationToken))
            {
                return new PageInfo(responseContinuationToken, Count, PageNumber + 1);
            }

            return null;
        }
        
        /// <summary>
        /// Iterates over all pages of items.
        /// </summary>
        /// <param name="fetchPage">An async function that takes in a page, and returns the items in the page.</param>
        /// <typeparam name="T">The type of the items in the page.</typeparam>
        /// <returns>An async enumerable that iterates over the pages of items.</returns>
        internal static async IAsyncEnumerable<ReadOnlyMemory<T>> IterateByPage<T>(
            Func<PageInfo?, ValueTask<Page<T>>> fetchPage
        )
        {
            PageInfo? page = Default();
            
            do
            {
                var result = await fetchPage(page);
                var items = result.Items;

                if (items.Length == 0)
                {
                    break;
                }
                
                yield return items;

                page = result.NextPage;
            }
            while (page != null);
        }
        
        /// <summary>
        /// Iterates over all items.
        /// </summary>
        /// <param name="fetchPage">An async function that takes in a page, and returns the items in the page.</param>
        /// <typeparam name="T">The type of the items in the page.</typeparam>
        /// <returns>An async enumerable that iterates over the items.</returns>
        internal static async IAsyncEnumerable<T> IterateByItem<T>(
            Func<PageInfo?, ValueTask<Page<T>>> fetchPage
        )
        {
            await foreach (var items in IterateByPage(fetchPage))
            {
                for (var i = 0; i < items.Length; i++)
                {
                    yield return items.Span[i];
                }
            }
        }
    }
}
