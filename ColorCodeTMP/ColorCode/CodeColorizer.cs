// Copyright (c) Microsoft Corporation.  All rights reserved.

using ColorCode.Compilation;
using ColorCode.Lexing;

namespace ColorCode
{
    /// <summary>
    /// Lexes source code.
    /// </summary>
    public class CodeColorizer
    {
        public struct SourceAndScope
        {
            public string sourceCode;
            public Scope scope;

            public SourceAndScope(string sourceCode, Scope scope)
            {
                this.sourceCode = sourceCode;
                this.scope = scope;
            }
        }

        /// <summary>
        /// Lex source code using the specified language.
        /// </summary>
        /// <param name="sourceCode">The source code to colorize.</param>
        /// <param name="language">The language to use to colorize the source code.</param>
        /// <returns>The lexed source code.</returns>
        public SourceAndScope[] Lex(string sourceCode, ILanguage language)
        {
            LanguageLexer languageLexer = new LanguageLexer(new LanguageCompiler(Languages.CompiledLanguages),
                Languages.LanguageRepository);

            return languageLexer.Lex(sourceCode, language);
        }
    }
}