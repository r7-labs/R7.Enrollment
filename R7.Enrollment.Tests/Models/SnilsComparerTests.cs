using R7.Enrollment.Models;
using Xunit;

namespace R7.Enrollment.Tests
{
    public class SnilsComparerTests
    {
        [Fact]
        public void SnilsNotNullAndEqualsTest ()
        {
            var snilsComparer = new SnilsComparer ();

            Assert.False (snilsComparer.SnilsNotNullAndEquals (null, null));
            Assert.False (snilsComparer.SnilsNotNullAndEquals ("111-111-111-11", null));
            Assert.False (snilsComparer.SnilsNotNullAndEquals (null, "111-111-111-11"));
            Assert.False (snilsComparer.SnilsNotNullAndEquals ("", ""));
            Assert.False (snilsComparer.SnilsNotNullAndEquals ("111-111-111-11", "111-111-111-112"));

            Assert.True (snilsComparer.SnilsNotNullAndEquals ("111-111-111-11", "111-111-111-11"));
            Assert.True (snilsComparer.SnilsNotNullAndEquals ("111111-11111", "111111111-11"));
            Assert.True (snilsComparer.SnilsNotNullAndEquals ("111-111-111-11", "11111111111"));
        }
    }
}
