
using System.Text.RegularExpressions;

Console.WriteLine(CleanupAppLockName("My App!@# Name--With--Bad--Chars///one!!!!111"));


string CleanupAppLockName(string appLocId) {
        var badCharacters = new Regex(@"[^\w\d\-]");
        var doubleHyphen = new Regex("--+");
        var result = appLocId;
        result = result.Trim().Trim('-');
        result = badCharacters.Replace(result, "-");
        result = doubleHyphen.Replace(result, "-");
        return result;

}