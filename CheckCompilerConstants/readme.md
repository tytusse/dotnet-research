- run `dotnet run -p:TestBuild=true --project .\CheckCompilerConstants\CheckCompilerConstants.csproj` 
  to observe 'TESTBUILD' constant being defined.
- to observe lack of 'TESTBUILD' constant.
  - run `dotnet run -p:TestBuild=false --project .\CheckCompilerConstants\CheckCompilerConstants.csproj`
  - OR run `dotnet run --project .\CheckCompilerConstants\CheckCompilerConstants.csproj`
  