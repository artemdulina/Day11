using System;

namespace Entities
{
    public class Book : IEquatable<Book>, IComparable<Book>, IComparable
    {
        public string Author { get; }
        public string Title { get; }
        public int Pages { get; }
        public int Year { get; }

        public Book() : this("default", "default", 0, 0)
        {

        }

        public Book(string author, string title, int pages, int year)
        {
            if (pages <= 0)
            {
                throw new ArgumentException("pages should be greater than 0");
            }
            Author = author;
            Title = title;
            Pages = pages;
            Year = year;
        }

        public bool Equals(Book other)
        {
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (Author == other.Author && Title == other.Title)
            {
                return true;
            }
            return false;
        }

        public int CompareTo(Book other)
        {
            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            if (Pages > other.Pages)
                return Pages - other.Pages;
            if (Pages < other.Pages)
                return Pages - other.Pages;
            return 0;

        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            Book book = (Book)obj;
            return Equals(book);
        }

        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (obj.GetType() != GetType())
            {
                throw new ArgumentException("Argument must be Book");
            }
            Book book = (Book)obj;
            return CompareTo(book);
        }

        public override string ToString()
        {
            return $"Title: {Title}, Author: {Author}, Number of pages: {Pages}, The year of publishing: {Year}";
        }
    }
}
