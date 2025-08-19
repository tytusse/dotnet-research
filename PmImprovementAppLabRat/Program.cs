using System.Globalization;

PrintAge("John", new DateTime(1960, 03, 11), "ar-SA"); // ar-SA -> arabic, Saudi Arabia

void PrintAge(string name, DateTime birthdate, string cultureCode) {
    var culture = CultureInfo.GetCultureInfo(cultureCode);

    FormattableString formattableString = $"{name} was born on {birthdate:d}";
    var text = formattableString.ToString(culture);
 
    Console.WriteLine(text);
}