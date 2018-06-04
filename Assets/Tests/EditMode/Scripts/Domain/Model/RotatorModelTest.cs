using ExtraUniRx;
using NUnit.Framework;
using UnityEngine;

namespace CAFU.Rotator.Domain.Model {

    public class RotatorModelTest {

        [Test]
        public void TotalRotationCountTest() {
            var model = new RotatorModel();

            // default
            {
                var observer = new TestObserver<int>();
                model.GetTotalRotationCount(false).Subscribe(observer);

                model.RotationDiffRad.Value = Mathf.PI;
                Assert.AreEqual(0, observer.OnNextValues.Count);

                model.RotationDiffRad.Value = Mathf.PI;
                Assert.AreEqual(1, observer.OnNextCount);
                Assert.AreEqual(1, observer.OnNextValues[0]);

                model.RotationDiffRad.Value = 2 * Mathf.PI;
                Assert.AreEqual(2, observer.OnNextCount);
                Assert.AreEqual(2, observer.OnNextValues[1]);

                model.RotationDiffRad.Value = -2 * Mathf.PI;
                Assert.AreEqual(2, observer.OnNextCount);

                model.RotationDiffRad.Value = 2 * Mathf.PI;
                Assert.AreEqual(2, observer.OnNextCount);

                model.RotationDiffRad.Value = 2 * Mathf.PI;
                Assert.AreEqual(3, observer.OnNextCount);
                Assert.AreEqual(3, observer.OnNextValues[2]);

                model.RotationDiffRad.Value = 2 * Mathf.PI;
                Assert.AreEqual(4, observer.OnNextCount);
                Assert.AreEqual(4, observer.OnNextValues[3]);
            }

            // reverse
            {
                var observer = new TestObserver<int>();
                model.GetTotalRotationCount(true).Subscribe(observer);

                model.RotationDiffRad.Value = - Mathf.PI;
                Assert.AreEqual(0, observer.OnNextValues.Count);
                
                model.RotationDiffRad.Value = - Mathf.PI;
                Assert.AreEqual(1, observer.OnNextValues.Count);
                Assert.AreEqual(1, observer.OnNextValues[0]);
            }
        }

    }

}