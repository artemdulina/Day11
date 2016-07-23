using System;
using System.Collections.Generic;

namespace Entities
{
    public class SortByAuthor : IComparer<Book>
    {
        public int Compare(Book x, Book y) => string.Compare(x.Author, y.Author, StringComparison.Ordinal);
    }

    public class SortByYear : IComparer<Book>
    {
        public int Compare(Book x, Book y) => x.Year.CompareTo(y.Year);
    }

    public class SortBytitle : IComparer<Book>
    {
        public int Compare(Book x, Book y) => string.Compare(x.Title, y.Title, StringComparison.Ordinal);
    }
}
