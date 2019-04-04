using System;
using CAFU.Core;
using CAFU.Rotator.Application.Enum;
using ExtraUniRx;
using UniRx;
using UnityEngine;

namespace CAFU.Rotator.Domain.Entity
{
    public interface IRotatorFinishReportEntity : IEntity
    {
        IObservable<Unit> OnFinishedAsObservable();
    }

    public interface IRotatorEntity : IEntity
    {
        float RotationRad { get; set; }
        SubjectProperty<float> RotationDiffRad { get; }
        SubjectProperty<float> RotationSpeed { get; }

        IObservable<int> RotationCountAsObservable(RotateDirection rotateDirection);
    }

    public class RotatorEntity : IRotatorEntity
    {
        /// <summary>
        /// Rotator's relative rotation radian where user put their fingures.
        /// </summary>
        public float RotationRad { get; set; }

        /// <summary>
        /// Rotator's rotate difference on each user input rotation.
        /// </summary>
        public SubjectProperty<float> RotationDiffRad { get; } = new SubjectProperty<float>();

        /// <summary>
        /// Rotator's rotation speed. it's calculated by rotation diff rad & frame time.
        /// </summary>
        public SubjectProperty<float> RotationSpeed { get; } = new SubjectProperty<float>();

        /// <summary>
        /// Rotation Count by rotation rad diff.
        /// </summary>
        /// <param name="rotateDirection">Rotation direction</param>
        /// <returns>The count of rotation which should be more than 0.</returns>
        public IObservable<int> RotationCountAsObservable(RotateDirection rotateDirection)
        {
            var sign = rotateDirection == RotateDirection.Left ? 1 : -1;

            return RotationDiffRad
                    // 回転の集計方向
                    .Select(it => sign * it)
                    // 回転角の総和
                    .Scan((sum, it) => sum + it)
                    // 回転数に変換
                    .Select(it => it / (2 * Mathf.PI))
                    .Select(it => Mathf.FloorToInt(it))
                    // 回転数の変化時点に限定
                    .DistinctUntilChanged()
                    // 条件: 回転値が上昇した && 回転値が振動していない(堺の揺れ防止):
                    // - NG: -1 => -1
                    // - NG: +1 => -1
                    // - NG: -1 => +1 // 境目で振動するのを防ぎたい
                    // - OK: +1 => +1 // 連続上昇しているときはカウント、初回は+1が流れる
                    // 例:
                    // [1,2,1,2,3,2,1] => [  [0,1],  [1,2],  [2,1],  [1,2],  [2,3],   [3,2],   [2,1]]
                    //                 => [     +1,     +1,     -1,     +1,     +1,      -1,      -1]
                    //                 => [[+1,+1],[+1,+1],[+1,-1],[-1,+1],[+1,+1], [+1,-1], [-1,-1]]
                    //                 => [     +1,     +1,      -,      -,     +1,       -,       -]
                    //                 => [     +1,     +1,                     +1,                 ]
                    //                 => [      1,      2,                      3,                 ]
                    .StartWith(0)
                    .Buffer(2, 1)
                    .Select(it => it[1] - it[0])
                    .StartWith(1)
                    .Buffer(2, 1)
                    .Where(it => it[1] > 0 && it[0] >= 0)
                    .Select(it => it[1])
                    .Scan((sum, it) => sum + it)
                ;
        }
    }
}
