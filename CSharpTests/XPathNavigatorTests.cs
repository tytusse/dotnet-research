using System.Xml;
using System.Xml.XPath;
using Xunit.Abstractions;

namespace CSharpTests;

public class XPathNavigatorTests(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void DeepXPathWorks()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <root>
                            <child>
                                <id>1</id>
                            </child>
                            <child>
                                <id>2</id>
                                <attributes>
                                    <attribute>
                                        <barbaz>bar</barbaz>
                                    </attribute>
                                    <attribute>
                                        <origin>Foo</origin>
                                    </attribute>
                                    <attribute>
                                        <barbaz>adad</barbaz>
                                    </attribute>
                                    <attribute>
                                        <origin>Foo2</origin>
                                    </attribute>
                                </attributes>
                            </child>
                        </root>
                        """));
        var navigator = xml.CreateNavigator();
        var children = 
            navigator?.Select("/root/child").OfType<XPathNavigator>()
                .Select(child =>
                {
                    var id = child.SelectSingleNode("id")?.Value;
                    Assert.NotNull(id);
                    var origin =
                        child
                            .Select("attributes/attribute/origin").OfType<XPathNavigator>()
                            .Select(x => x.Value)
                            .FirstOrDefault();
                    return new {id, origin};
                })
                .ToList();
        Assert.NotNull(children);
        Assert.Collection(children,
            child =>
            {
                Assert.Equal("1", child.id);
                Assert.Null(child.origin);
            },
            child =>
            {
                Assert.Equal("2", child.id);
                Assert.Equal("Foo", child.origin);
            });
    }

    [Fact]
    public void NullsWorkButRequireTricks()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <attributes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                            <attribute>
                                <barbaz>bar</barbaz>
                            </attribute>
                            <attribute>
                                <origin xsi:nil="true" />
                            </attribute>
                            <attribute>
                                <origin>Foo</origin>
                            </attribute>
                            <attribute>
                                <barbaz>adad</barbaz>
                            </attribute>
                            <attribute>
                                <origin>Foo2</origin>
                            </attribute>
                        </attributes>
                        """));
        var origin =
            xml.CreateNavigator()
                ?.Select("attributes/attribute/origin").OfType<XPathNavigator>()
                .Where(x => 
                    x.GetAttribute("nil", "http://www.w3.org/2001/XMLSchema-instance")
                    is not "true")
                .Select(x => x.Value)
                .FirstOrDefault();
        Assert.Equal("Foo", origin);
    }
    
    [Fact]
    public void SkippingEmptyStrings()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <attributes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                            <attribute>
                                <barbaz>bar</barbaz>
                            </attribute>
                            <attribute>
                                <origin>   </origin>
                            </attribute>
                            <attribute>
                                <origin/>
                            </attribute>
                            <attribute>
                                <origin>Foo</origin>
                            </attribute>
                            <attribute>
                                <barbaz>adad</barbaz>
                            </attribute>
                            <attribute>
                                <origin>Foo2</origin>
                            </attribute>
                        </attributes>
                        """));
        var origin =
            xml.CreateNavigator()
                ?.Select("attributes/attribute/origin").OfType<XPathNavigator>()
                .Select(x => x.Value.Trim())
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
        Assert.Equal("Foo", origin);
    }

    [Fact]
    public void XPathNavToString()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <attributes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                            <attribute>
                                <barbaz>bar</barbaz>
                            </attribute>
                        </attributes>
                        """));
        var someXpathNavigator =
            xml.CreateNavigator()
                ?.Select("attributes/attribute").OfType<XPathNavigator>().FirstOrDefault();
        
        testOutputHelper.WriteLine($"{someXpathNavigator}");
    }

    [Fact]
    public void CanSpecifyRoot()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <attributes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                            <attribute>
                                <barbaz>bar</barbaz>
                            </attribute>
                        </attributes>
                        """));
        
        var navigator = xml.CreateNavigator()
            ?.Select("/attributes/attribute/barbaz").OfType<XPathNavigator>().FirstOrDefault();
        
        Assert.NotNull(navigator);
        Assert.Equal("bar", navigator.Value);
    }

    [Fact]
    public void NodeNotFoundBehavior_Collection()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <attributes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                            <attribute>
                                <barbaz>bar</barbaz>
                            </attribute>
                        </attributes>
                        """));
        
        var navigator = xml.CreateNavigator()
            ?.Select("blabla").OfType<XPathNavigator>().FirstOrDefault();
        Assert.Null(navigator);
    }
    
    [Fact]
    public void NodeNotFoundBehavior_SingleNode()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <attributes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                            <attribute>
                                <barbaz>bar</barbaz>
                            </attribute>
                        </attributes>
                        """));
        
        var navigator = xml.CreateNavigator()?.SelectSingleNode("blabla");
        Assert.Null(navigator);
    }
    
    [Fact]
    public void CanHaveNestedNavigators()
    {
        var xml = new XmlDocument().Do(xml =>
            xml.LoadXml("""
                        <main>
                            <attributes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
                                <attribute>
                                    <origin>Foo</origin>
                                </attribute>
                                <attribute>
                                    <origin>Foo2</origin>
                                </attribute>
                            </attributes>
                            <origins>
                                <origin>
                                    <name>Foo</name>
                                    <color>Red</color>
                                </origin>
                                <origin>
                                    <name>Foo2</name>
                                    <color>Blue</color>
                                </origin>
                            </origins>
                        </main>
                        """));
        var xPathNavigator = xml.CreateNavigator();
        var origins =
            xPathNavigator
                ?.Select("main/attributes/attribute/origin").OfType<XPathNavigator>()
            ?? Array.Empty<XPathNavigator>();

        foreach (var origin in origins) {
            testOutputHelper.WriteLine(origin.Value);
            var theOrigin = xPathNavigator?.SelectSingleNode($"main/origins/origin[name/text()='{origin.Value}']");
            Assert.NotNull(theOrigin);
            testOutputHelper.WriteLine($"theOrigin: {theOrigin.InnerXml}");
        }

    }
}