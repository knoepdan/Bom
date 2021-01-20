﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Knomes.Localization
{
    public interface ITextService
    {
        /// <summary>
        /// Translate value in the corresponding language
        /// </summary>
        /// <param name="code">key</param>
        /// <param name="fallbackValue">Fallback if no value was found</param>
        /// <param name="args">param objects</param>
        /// <returns></returns>
        string Localize(string code, string fallbackValue, params object[] args);

        /// <summary>
        /// Marker for text that don't need translation
        /// </summary>
        /// <param name="textValue"></param>
        string Fixed(string textValue, params object[] args);

        /// <summary>
        /// Text that don't need translation
        /// </summary>
        /// <param name="textValue"></param>
        string Todo(string code, string fallbackValue, params object[] args);
    }
}
