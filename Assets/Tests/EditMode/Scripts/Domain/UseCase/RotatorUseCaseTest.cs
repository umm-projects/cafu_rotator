using ExtraUniRx;
using NUnit.Framework;
using UnityEngine;

namespace CAFU.Rotator.Domain.UseCase {

    public class RotatorUseCaseTest {

        [Test]
        public void RotationDiffTest() {
            var usecase = new RotatorUseCase.Factory().Create();
            var observer = new TestObserver<float>();
            usecase.RotationDiffAsObservable.Subscribe(observer);
            Assert.AreEqual(0, observer.OnNextCount);

            usecase.OnPressStart(new Vector3(1, 0), Vector3.zero);
            usecase.OnPressing(new Vector3(1, 1), Vector3.zero);
            Assert.AreEqual(1, observer.OnNextCount);
            Assert.AreEqual(Mathf.PI / 4, observer.OnNextValues[0], 0.0000001f);

            usecase.OnPressing(new Vector3(1, Mathf.Sqrt(3)), Vector3.zero);
            Assert.AreEqual(2, observer.OnNextCount);
            Assert.AreEqual(Mathf.PI / 3 - Mathf.PI / 4, observer.OnNextValues[1], 0.0000001f);
        }

    }

}