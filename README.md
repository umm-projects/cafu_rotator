# cafu_rotator

## What

* Rotator is rotation increment logic by observable 

## Requirement

- cafu\_resume_pause
- extra\_unirx
- event\_activator

## Install

```shell
yarn add "umm/cafu_rotator#^1.0.0"
```

## Usage

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

