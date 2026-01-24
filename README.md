# Invicta.Numerics.Vectors
This library contains double-precision copies of [the single-precision structs contained within the `System.Numerics.Vectors` assembly](https://learn.microsoft.com/en-us/dotnet/api/system.numerics#structs).

## Design Philosophy

### Modifications
This double-precision conversion aims to make as few changes as possible to [the original single-precision source code](https://github.com/dotnet/runtime/tree/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics). The fewer changes that are made the easier it is the review them and the higher the confidence that the library will mirror the behavior of the original.

To this end some "tricks" have been used such as [MSBuild `Using` properties](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#using) in the `.csproj` files to avoid modifying `using` statements within the code, stubbing/recreating internal dependencies from within the .NET `System` libraries, and commenting out rather than deleting code so that line numbering in the original source and modified source stays the same.

### Extensions
To keep this library a clean, maintainable, one-to-one mapping of `System.Numerics.Vectors` no additional functionality shall be introduced. Any additional functionality will instead be maintained in separate companion libraries following an `Invicta.Numerics.X` naming scheme.

### Testing
To allow the reuse of [the existing .NET repository unit tests](https://github.com/dotnet/runtime/tree/v10.0.0/src/libraries/System.Numerics.Vectors/tests) this library uses xUnit for testing.
> As with the supporting code for the implementation some xUnit dependencies have been stubbed.

### Configuration
This library uses code from the official .NET runtime repository and as such has adopted their [`.editorconfig`](.editorconfig), [`.gitattributes`](.gitattributes) and [`.gitignore`](.gitignore) configuration files. This is done so that the code remains in its original form and can easily be compared to the canonical source code.
> Some parts of these files will not be relevant to the narrow subset of code contained within this library, however that should not have any negative impact on this library and they will not be modified from the canonical versions unless absolutely necessary.

#### Code Style
For the reasons described above this repository adopts the .NET runtime repository style guidelines.

### Versioning
The `Invicta.Numerics.Vectors` library uses [semantic versioning as recommended by Microsoft](https://learn.microsoft.com/en-us/nuget/concepts/package-versioning) for NuGet packages, where the version number takes the form [*`major.minor.patch`*](https://semver.org/).

The version number of the library will match the major/minor version from the tag of the original `System.Numerics.Vectors` source code. The patch version of the `Invicta.Numerics.Vectors` library will be used to indicate each subsequent release of the library, for example the first release of the `Invicta.Numerics.Vectors` library adapted from the `v10.0.0` source code will be `v10.0.0`, the second release adapted from the `v10.0.0` source code will be `v10.0.1` and so on.
> This is the pattern that Microsoft have adopted for other libraries released in step with the .NET runtime such as [`Microsoft.Extensions.Hosting`](https://www.nuget.org/packages/Microsoft.Extensions.Hosting#versions-body-tab).

Within this repository each variant of the library will be maintained in a separate branch named after the major/minor version of the .NET libraries from which it is derived e.g. `v10.0`, `v12.0` etc. Each new release of the library will be tagged with its version number, e.g. `v10.0.0`, `v10.0.1`, `v10.0.2` etc.

### Licensing
This library is derived from code from the [.NET runtime repository](https://github.com/dotnet/runtime/tree/v10.0.0) which is licensed under the [MIT license](https://github.com/dotnet/runtime/blob/v10.0.0/LICENSE.TXT), copyright .NET Foundation and Contributors. The modifications to this code are also licensed under the [MIT license](https://opensource.org/license/mit), copyright Andrew Pollard.

SPDX-License-Identifier: MIT. See [LICENSE.txt](LICENSE.txt) and [THIRD-PARTY-NOTICES.txt](THIRD-PARTY-NOTICES.txt) for more details.

## Steps to Create the Library

### Acquire the `System.Numerics.Vectors` Source Code
Gather the source files for the desired mathematical constructs. As of `v10.0.0` these are:
- `System.Numerics.Matrix3x2`
	- [`Matrix3x2.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Matrix3x2.cs)
	- [`Matrix3x2.Impl.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Matrix3x2.Impl.cs)
- `System.Numerics.Matrix4x4`
	- [`Matrix4x4.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Matrix4x4.cs)
	- [`Matrix4x4.Impl.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Matrix4x4.Impl.cs)
- `System.Numerics.Plane`
	- [`Plane.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Plane.cs)
- `System.Numerics.Quaternion`
	- [`Quaternion.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Quaternion.cs)
	- [`Quaternion.Extensions.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Quaternion.Extensions.cs)
- `System.Numerics.Vector2`
	- [`Vector2.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Vector2.cs)
	- [`Vector2.Extensions.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Vector2.Extensions.cs)
- `System.Numerics.Vector3`
	- [`Vector3.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Vector3.cs)
	- [`Vector3.Extensions.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Vector3.Extensions.cs)
- `System.Numerics.Vector4`
	- [`Vector4.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Vector4.cs)
	- [`Vector4.Extensions.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics/Vector4.Extensions.cs)

### Acquire Supporting Source Code
Gather the source files for the required supporting code - as of `v10.0.0` the following have been introduced:

- `System.Runtime.CompilerServices.IntrinsicAttribute`
	- [`IntrinsicAttribute.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/IntrinsicAttribute.cs)
- `System.ThrowHelper`
	- [`ThrowHelper.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/ThrowHelper.cs)
- `System.Runtime.Intrinsics.Vector`
	- [`Vector128.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Runtime/Intrinsics/Vector128.cs)

### Acquire the `System.Numerics.Vectors.Tests` Source Code
Gather the unit test source code - as of `v10.0.0` the following have been introduced:
- `System.Numerics.MathHelper`
	- [`MathHelper.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Numerics.Vectors/tests/MathHelper.cs)
- `System.Numerics.Tests.Matrix3x2Tests`
	- [`Matrix3x2Tests.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Numerics.Vectors/tests/Matrix3x2Tests.cs)
- `System.Numerics.Tests.Matrix4x4Tests`
	- [`Matrix4x4Tests.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Numerics.Vectors/tests/Matrix4x4Tests.cs)
- `System.Numerics.Tests.PlaneTests`
	- [`PlaneTests.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Numerics.Vectors/tests/PlaneTests.cs)
- `System.Numerics.Tests.QuaternionTests`
	- [`QuaternionTests.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Numerics.Vectors/tests/QuaternionTests.cs)
- `System.Numerics.Tests.Vector2Tests`
	- [`Vector2Tests.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Numerics.Vectors/tests/Vector2Tests.cs)
- `System.Numerics.Tests.Vector3Tests`
	- [`Vector3Tests.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Numerics.Vectors/tests/Vector3Tests.cs)
- `System.Numerics.Tests.Vector4Tests`
	- [`Vector4Tests.cs`](https://github.com/dotnet/runtime/blob/v10.0.0/src/libraries/System.Numerics.Vectors/tests/Vector4Tests.cs)

### Modify the Source Code
The modifications required to adapt the original source code will be different for each version, however in broad strokes they are as follows:

- Change the namespaces of the classes/structs from `System.*`/`Microsoft.*`/etc. to `Invicta.*`.

- Add a `D` suffix to the names of the `Invicta.Numerics` structures, e.g. `System.Numerics.Vector4` would become `Invicta.Numerics.Vector4D`.

- Modify any single-precision extension methods for converting `System.Numerics` structures to/from `System.Runtime.Intrinsics` structures to be double-precision, e.g. [`System.Runtime.Intrinsics.Vector128.AsVector128(Vector4)`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector128.asvector128#system-runtime-intrinsics-vector128-asvector128(system-numerics-vector4)) would become `Invicta.Runtime.Intrinsics.Vector256.AsVector256(Vector4D)` and [`System.Runtime.Intrinsics.Vector128.AsVector4(Vector128<Single>)`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector128.asvector4#system-runtime-intrinsics-vector128-asvector4(system-runtime-intrinsics-vector128((system-single)))) would become `Invicta.Runtime.Intrinsics.Vector256.AsVector4D(Vector256<Double>)`.

- Rename each file to match its new contents e.g. `Vector4.Extensions.cs` becomes `Vector4D.Extensions.cs`, `Vector128.cs` becomes `Vector256.cs` etc.

- Replace references to `float` with `double`.

- Replace [float literals with double literals](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types#real-literals).
	> [Find and replace within Visual Studio supports Regular Expressions](https://stackoverflow.com/questions/43577528) - a find expression of `(\d)f` matched with a replace expression of `$1d` will work in most cases.

- Replace references to [`System.MathF`](https://learn.microsoft.com/en-us/dotnet/api/system.mathf) with [`System.Math`](https://learn.microsoft.com/en-us/dotnet/api/system.math).

- Replace references to [`System.Runtime.Intrinsics.Vector128`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector128) with [`System.Runtime.Intrinsics.Vector256`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector256).

- Replace references to [`System.Runtime.Intrinsics.Vector128<float>`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector128-1) with [`System.Runtime.Intrinsics.Vector256<double>`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.intrinsics.vector256-1).

- Replace references to single precision types from `System.Numerics` with double precision types from `Invicta.Numerics`.

- Where necessary comment out unused code.
	> In general there is no harm in leaving unused code as-is and it means it is easier to compare to the original source as line numbers remain the same, however if that unused code is inconvenient e.g. it introduces additional dependencies, cannot be built etc. then comment it out.
