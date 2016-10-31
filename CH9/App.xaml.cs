using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;

namespace CH9
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{
        //    CultureInfo culture = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
        //    var dtf = culture.DateTimeFormat;
        //    dtf.ShortTimePattern = Microsoft.Win32.Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\International", "sShortTime", "hh:mm tt") as string;
        //    CultureInfo.DefaultThreadCurrentUICulture = culture;

        //    Thread.CurrentThread.CurrentUICulture = culture;

        //    var xmlLang = FixXmlLanguage(culture);

        //    FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(xmlLang));
        //    // ReSharper disable AccessToStaticMemberViaDerivedType
        //    FrameworkContentElement.LanguageProperty.OverrideMetadata(typeof(Run), new FrameworkPropertyMetadata(xmlLang));
        //    // ReSharper restore AccessToStaticMemberViaDerivedType   

        //    ShutdownMode = ShutdownMode.OnMainWindowClose;
        //    base.OnStartup(e);
        //}


        private static XmlLanguage FixXmlLanguage(CultureInfo culture)
        {
            var xmlLang = XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag);

            var specificCultureField = xmlLang.GetType().GetField("_specificCulture", BindingFlags.Instance | BindingFlags.NonPublic);
            var equivalentCultureField = xmlLang.GetType().GetField("_equivalentCulture", BindingFlags.Instance | BindingFlags.NonPublic);
            if (specificCultureField != null && equivalentCultureField != null)
            {
                specificCultureField.SetValue(xmlLang, culture);
                equivalentCultureField.SetValue(xmlLang, culture);
            }

            return xmlLang;
        }

    }
}
