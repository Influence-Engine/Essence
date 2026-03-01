
namespace Essence.Tests
{
    [TestFixture]
    public class TimeTests
    {
        [Test]
        public void InitializeState()
        {
            Time.Reset();

            Assert.That(Time.time, Is.EqualTo(0f));
            Assert.That(Time.timeAsDouble, Is.EqualTo(0f));

            Assert.That(Time.unscaledTime, Is.EqualTo(0f));

            Assert.That(Time.deltaTime, Is.EqualTo(0f));
            Assert.That(Time.deltaTimeAsDouble, Is.EqualTo(0f));

            Assert.That(Time.frameCount, Is.EqualTo(0f));
            Assert.That(Time.fixedFrameCount, Is.EqualTo(0f));

            Assert.That(Time.timeScale, Is.EqualTo(1f));
        }

        [Test]
        public void SingleUpdate_Float()
        {
            Time.Reset();

            float deltaTime = 1f / 60f;
            Time.Update(deltaTime);

            Assert.That(Time.deltaTime, Is.EqualTo(deltaTime));
            Assert.That(Time.unscaledDeltaTime, Is.EqualTo(deltaTime));
            Assert.That(Time.time, Is.EqualTo(deltaTime));
        }

        [Test]
        public void SingleUpdate_Double()
        {
            Time.Reset();

            double deltaTime = 1.0 / 60.0;
            Time.Update(deltaTime);

            Assert.That(Time.deltaTimeAsDouble, Is.EqualTo(deltaTime));
            Assert.That(Time.unscaledDeltaTimeAsDouble, Is.EqualTo(deltaTime));
            Assert.That(Time.timeAsDouble, Is.EqualTo(deltaTime));
        }

        [Test]
        public void TimeScale_Zero()
        {
            Time.Reset();
            Time.timeScale = 0f;

            float deltaTime = 1f / 60f;
            Time.Update(deltaTime);

            Assert.That(Time.deltaTime, Is.EqualTo(0f));
            Assert.That(Time.unscaledDeltaTime, Is.EqualTo(deltaTime));

            Assert.That(Time.time, Is.EqualTo(0f));
            Assert.That(Time.unscaledTime, Is.EqualTo(deltaTime));
        }

        [Test]
        public void TimeScale_Two()
        {
            Time.Reset();
            Time.timeScale = 2f;

            float deltaTime = 1f / 60f;
            Time.Update(deltaTime);

            Assert.That(Time.deltaTime, Is.EqualTo(deltaTime * 2));
            Assert.That(Time.unscaledDeltaTime, Is.EqualTo(deltaTime));

            Assert.That(Time.time, Is.EqualTo(deltaTime * 2));
            Assert.That(Time.unscaledTime, Is.EqualTo(deltaTime));
        }

        [Test]
        public void TimeScale_Ignore_Unscaled()
        {
            Time.Reset();
            Time.timeScale = 5;

            float deltaTime = 1f / 60f;
            Time.Update(deltaTime);
            Time.Update(deltaTime);

            Assert.That(Time.unscaledDeltaTime, Is.EqualTo(deltaTime));
            Assert.That(Time.unscaledTime, Is.EqualTo(deltaTime * 2));
        }


        [Test]
        public void FrameCount_Increment()
        {
            Time.Reset();

            float deltaTime = 1f / 60f;
            Time.Update(deltaTime);
            Time.Update(deltaTime);
            Time.Update(deltaTime);

            Assert.That(Time.frameCount, Is.EqualTo(3));
        }

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
