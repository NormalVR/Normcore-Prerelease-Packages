namespace Normcore.Services
{
    /// <summary>
    /// A struct that represents a page of results for a query.
    /// </summary>
    /// <typeparam name="T">The type of data stored in the page.</typeparam>
    public readonly struct Page<T>
    {
        /// <summary>
        /// The items in the page.
        /// </summary>
        public readonly T[] Items;

        /// <summary>
        /// The page info used to retrieve the items.
        /// </summary>
        public readonly PageInfo CurrentPage;
        
        /// <summary>
        /// The page info used to retrieve the next items. When <see langword="null"/>, there are no more pages.
        /// </summary>
        public readonly PageInfo? NextPage;
        
        internal Page(T[] items, PageInfo currentPage, PageInfo? nextPage)
        {
            Items = items;
            CurrentPage = currentPage;
            NextPage = nextPage;
        }
    }
}
