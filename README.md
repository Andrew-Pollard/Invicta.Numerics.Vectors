# Invicta.Numerics.Vectors
This library contains double-precision versions of [the single-precision structs contained within the `System.Numerics.Vectors` assembly](https://learn.microsoft.com/en-us/dotnet/api/system.numerics#structs).

## Configuration
This library uses code from the official .NET runtime repository and as such has adopted their [`.editorconfig`](https://github.com/Andrew-Pollard/Invicta.Numerics.Vectors/blob/v10.0/.editorconfig), [`.gitattributes`](https://github.com/Andrew-Pollard/Invicta.Numerics.Vectors/blob/v10.0/.gitattributes) and [`.gitignore`](https://github.com/Andrew-Pollard/Invicta.Numerics.Vectors/blob/v10.0/.gitignore) configuration files. This is done so that the code remains in its original form and can easily be compared to the canonical source code.
> Some parts of these files will not be relevant to the narrow subset of code contained within this library, however that should not have any negative impact on this library and they will not be modified from the canonical versions unless absolutely necessary.

### Code Style
For the reasons described above this repository adopts the .NET runtime repository style guidelines.

## Modifications
This double-precision conversion aims to make as few changes as possible to [the original single-precision source code](https://github.com/dotnet/runtime/tree/v10.0.0/src/libraries/System.Private.CoreLib/src/System/Numerics). The fewer changes that are made the easier it is to review them and the higher the confidence that the library will mirror the behavior of the original.

To this end some "tricks" have been used such as [MSBuild `Using` properties](https://learn.microsoft.com/en-us/dotnet/core/project-sdk/msbuild-props#using) in the `.csproj` files to avoid modifying `using` statements within the code, stubbing/recreating internal dependencies from within the .NET `System` libraries, and commenting out rather than deleting code so that line numbering in the original source and modified source stays the same.

## Extensions
To keep this library a clean, maintainable, one-to-one mapping of `System.Numerics.Vectors` no additional functionality shall be introduced. Any additional functionality will instead be maintained in separate companion libraries following an `Invicta.Numerics.X` naming scheme.

## Testing
To allow the reuse of [the existing .NET repository unit tests](https://github.com/dotnet/runtime/tree/v10.0.0/src/libraries/System.Numerics.Vectors/tests) this library uses xUnit for testing.
> As with the supporting code for the implementation some xUnit dependencies have been stubbed.

## Versioning
The `Invicta.Numerics.Vectors` library uses [semantic versioning as recommended by Microsoft](https://learn.microsoft.com/en-us/nuget/concepts/package-versioning) for NuGet packages, where the version number takes the form [*`major.minor.patch`*](https://semver.org/).

The version number of the library will match the major/minor version from the tag of the original `System.Numerics.Vectors` source code. The patch version of the `Invicta.Numerics.Vectors` library will be used to indicate each subsequent release of the library, for example the first release of the `Invicta.Numerics.Vectors` library adapted from the `v10.0.0` source code will be `v10.0.0`, the second release adapted from the `v10.0.0` source code will be `v10.0.1` and so on.
> This is the pattern that Microsoft have adopted for other libraries released in step with the .NET runtime such as [`Microsoft.Extensions.Hosting`](https://www.nuget.org/packages/Microsoft.Extensions.Hosting#versions-body-tab).

Within this repository each variant of the library will be maintained in a separate branch named after the major/minor version of the .NET libraries from which it is derived e.g. `v10.0`, `v12.0` etc. Each new release of the library will be tagged with its version number, e.g. `v10.0.0`, `v10.0.1`, `v10.0.2` etc.

## Licensing
This library is derived from code from the [.NET runtime repository](https://github.com/dotnet/runtime/tree/v10.0.0) which is licensed under the [MIT license](https://github.com/dotnet/runtime/blob/v10.0.0/LICENSE.TXT), copyright .NET Foundation and Contributors. The modifications to this code are also licensed under the [MIT license](https://opensource.org/license/mit), copyright Andrew Pollard.

SPDX-License-Identifier: MIT. See [LICENSE.txt](https://github.com/Andrew-Pollard/Invicta.Numerics.Vectors/blob/v10.0/LICENSE.txt) and [THIRD-PARTY-NOTICES.txt](https://github.com/Andrew-Pollard/Invicta.Numerics.Vectors/blob/v10.0/THIRD-PARTY-NOTICES.txt) for more details.
