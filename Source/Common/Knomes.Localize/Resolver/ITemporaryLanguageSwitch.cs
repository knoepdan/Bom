using System;

namespace Knomes.Localize.Resolver
{

    /// <summary>
    /// Interface for temporary language switches -> built on IDisposable interface and to be used in a using statement
    /// </summary>
    /// <remarks>
    /// Usage: 
    /// using(ITemporaryLanguageSwitch switch = resolver.GetTemporaryLanguageSwitch("en)){
    ///     TextService.Localize("someCode", ".....);
    /// }
    /// </remarks>
    public interface ITemporaryLanguageSwitch : IDisposable
    {
    }
}
