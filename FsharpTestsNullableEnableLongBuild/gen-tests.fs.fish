#!/usr/bin/env fish

set numOfTests 2000
set fileName Tests.fs

set header 'module Tests
open System
open FsharpTestsNullableEnableLongBuild.ExampleExtensions
open Xunit

type Fixture() ='

function mk_template -a num 
    echo "
    [<Fact>]
    member _.``My test ($num)`` () =
        let z = \"adad\".NullableEcho()
        let q = (null:string|null).NullableEcho()
        Assert.True(true)
    "
end

echo $header > $fileName
for i in (seq 1 $numOfTests) 
    mk_template $i >> $fileName
end