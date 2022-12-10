using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TochDetector : MonoBehaviour
{
    [SerializeField] private BowlingBall bowlingBall;

    private Vector2 _firstTochPosition;
    private Vector2 _continuosTochPosition;

    private void Start()
    {
        StartBeginTochDetect();
        StartShiftTochBegin();
        StartEndTOchDetect();
        StartDoubleTochDetect();

    }

    // Начать, палец на экране
    private void StartBeginTochDetect()
    {
        Observable
            .EveryUpdate()
            .Where(begin => Input.touchCount > 0)
            .Where(begin => Input.GetTouch(0).phase == TouchPhase.Began)
            .Subscribe(begin =>
            {
                _continuosTochPosition = _firstTochPosition = Input.GetTouch(0).position;

                bowlingBall.StopTorqueBall();
            })
            .AddTo(this);
    }
    // Сдвиг, движение пальца
    private void StartShiftTochBegin()
    {
        Observable
            .EveryUpdate()
            .Where(shift => Input.touchCount > 0)
            .Where(shift => Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(0).phase == TouchPhase.Stationary)
            .Subscribe(shift =>
            {
                _continuosTochPosition = Input.GetTouch(0).position;
                if (_continuosTochPosition == _firstTochPosition) return;

                var direction = _firstTochPosition - _continuosTochPosition;
                direction = Vector2.ClampMagnitude(direction, 3f);

                bowlingBall.TorqueBall(direction.x, direction.y);
            })
            .AddTo(this);
    }
    // Конец, остановка пальца или его отсутствие
    private void StartEndTOchDetect()
    {
        Observable
           .EveryUpdate()
           .Where(end => Input.touchCount > 0)
           .Where(end => Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
           .Subscribe(end =>
           {
               bowlingBall.BreakTorqueBall();
           })
           .AddTo(this);
    }
    // Двойное, нажатие пальца
    private void StartDoubleTochDetect()
    {
        var doubleToch =
            Observable
            .EveryUpdate()
            .Where(doubleBegin => Input.touchCount > 0)
            .Where(doubleBegin => Input.GetTouch(0).phase == TouchPhase.Began);

        doubleToch
            .Buffer(doubleToch.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(doubleBegin => doubleBegin.Count >= 2)
            .Subscribe(doubleBegin =>
            {
                bowlingBall.UseGravityBall();
            })
            .AddTo(this);

    }
}
