# cafu_rotator

## What

* Rotator is rotation increment logic by observable 

## for CAFU v3

### Requirement

- extra\_unirx
- event\_activator

### Install

```shell
yarn add "umm/cafu_rotator#^2.0.0"
```

### Usage

Attach RotatorInput on your rotation input object.
Attach RotatorRenderer on your rotation rendering object.

If you want receive rotation speed or count, implement receiver interface

```csharp
public class RotatorReceiver : MonoBehaviour, IRotatorRotationSpeedReceiver, IRotatorRotationCountReceiver
{
    public void ReceiveRotationSpeed(float rotationSpeed)
    {
        ...
    }

    public void ReceiveRotationCount(int rotationCount)
    {
        ...
    }

    public void ReceiveTotalRotationCount(int totalRotationCount)
    {
        ...
    }
}
```

And implement finish reporter interface

```csharp
public class RotatorFinishReportEntity : IRotatorFinishReportEntity
{
    public IObservable<Unit> OnFinishedAsObservable()
    {
        // ex)
        return Observable
            .Timer(TimeSpan.FromSeconds(5.0f))
            .AsUnitObservable();
    }
}
```

## for CAFU v2

### Requirement

- cafu\_resume_pause
- extra\_unirx
- event\_activator

### Install

```shell
yarn add "umm/cafu_rotator#^1.0.0"
```

### Usage

Implement preseneter

```csharp
public class MyPresenter : IRotatorPresenter, IResumePausePresenter
{
    public IRotatorUseCase RotatorUseCase { get; private set; }
}
```

Attach RotatorInput on your rotation input object.
Attach RotatorRenderer on your rotation rendering object.
Attach RotatorCountText on your rotation count text.

## License

Copyright (c) 2018 Takuma Maruyama

Released under the MIT license, see [LICENSE.txt](LICENSE.txt)

