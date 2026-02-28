
namespace Essence.Tests
{
    [TestFixture]
    public class Vector2Tests
    {
        #region Constructors

        [Test]
        public void Constructor_SetsValuesCorrectly()
        {
            var v = new Vector2(6f, 7f);

            Assert.That(v.x, Is.EqualTo(6f));
            Assert.That(v.y, Is.EqualTo(7f));
        }

        [Test]
        public void Constructor_InitializesToZero()
        {
            var v = new Vector2();

            Assert.That(v.x, Is.EqualTo(0f));
            Assert.That(v.y, Is.EqualTo(0f));
        }

        #endregion

        #region Indexer

        [Test]
        public void Indexer_Get_ReturnsCorrectComponent()
        {
            var v = new Vector2(6f, 7f);

            Assert.That(v[0], Is.EqualTo(6f));
            Assert.That(v[1], Is.EqualTo(7f));
        }

        [Test]
        public void Indexer_Set_SetsCorrectComponent()
        {
            var v = new Vector2();

            v[0] = 6f;
            v[1] = 7f;

            Assert.That(v.x, Is.EqualTo(6f));
            Assert.That(v.y, Is.EqualTo(7f));
        }

        #endregion
    }
}
