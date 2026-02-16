Use `gen-tests.fs.fish` to generate .... `Tests.fs`
Main idea is to generate big test file so we can see how (and if) it affects compile time.

To see compile time with nullable=enable one can call 
`dotnet msbuild -v d -t:Rebuild`

To see it without nullable 
`dotnet msbuild -v d -t:Rebuild /p:Nullability=disable`


`Nullability` is a *custom* prop that is used in `*.fsproj` inside condition to turn nullability on or off.
`-v d` is optional - it will cause custom messages to be displayed.