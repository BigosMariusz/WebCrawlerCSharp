using Xunit;

namespace WebCrawler.Tests
{
    public class LinkServiceTests
    {
        [Theory]
        [InlineData("http://site.example.com", "http://site.example.com/")]
        [InlineData("http://site.example.com/page1/", "http://site.example.com/page1/")]
        [InlineData("http://site.example.com/page1", "http://site.example.com/page1/")]
        [InlineData("http://site.example.com/page1.html", "http://site.example.com/")]
        [InlineData("http://wp.pl", "http://wp.pl/")]
        public void ShouldReturnEalierPath(string input, string expected)
        {
            // Arrange
            
            // Act
            string actual = LinkService.GetEalierPath(input);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldReturnDomainOrSubdomain()
        {
            // Arrange
            string expected = "site.example.com";

            // Act
            string actual = LinkService.GetDomainOrSubdomain("http://site.example.com");

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("/static.html", false)]
        [InlineData("players/id/450.html", false)]
        public void ShouldReturnIsFullPath(string input, bool expected)
        {
            // Arrange

            // Act
            bool actual = LinkService.IsFullPath(input);

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
