using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace CH9.MVVM
{
    public class ViewResolver:IViewResolver
    {
        private static bool _uiAutomationIDSupport;

        public static void Initialise()
        {
            ViewLocator.NameTransformer.AddRule
              (
                  @"(?<nsbefore>([A-Za-z_]\w*\.)*)?(?<nsvm>ViewModels\.)(?<nsafter>[A-Za-z_]\w*\.)*(?<basename>[A-Za-z_]\w*)(?<suffix>ViewModel$)",
                  @"${nsbefore}Views.PropertyEditors.${nsafter}${basename}View",
                  @"(([A-Za-z_]\w*\.)*)?ViewModels\.([A-Za-z_]\w*\.)*[A-Za-z_]\w*ViewModel$"
              );

            ViewLocator.NameTransformer.AddRule
                (
                    @"(?<nsbefore>([A-Za-z_]\w*\.)*)?(?<nsvm>ViewModels\.)(?<nsafter>[A-Za-z_]\w*\.)*(?<basename>[A-Za-z_]\w*)(?<suffix>ViewModel$)",
                    @"${nsbefore}Views.PropertyEditors.${basename}View",
                    @"(([A-Za-z_]\w*\.)*)?ViewModels\.([A-Za-z_]\w*\.)*[A-Za-z_]\w*ViewModel$"
                );
        }

        public static IViewResolver View => new ViewResolver();
    }

    public interface IViewResolver
    {
    }
}
