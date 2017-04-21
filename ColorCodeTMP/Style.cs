﻿// Copyright (c) Microsoft Corporation.  All rights reserved.

using ColorCode.Common;

namespace ColorCode
{
    /// <summary>
    /// Defines the styling for a given scope.
    /// </summary>
    public class Style
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Style"/> class.
        /// </summary>
        /// <param name="scopeName">The name of the scope the style defines.</param>
        public Style(string scopeName)
        {
            Guard.ArgNotNullAndNotEmpty(scopeName, "scopeName");

            ScopeName = scopeName;
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        /// <value>The background color.</value>
        public string Background { get; set; }
        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        /// <value>The foreground color.</value>
        public string Foreground { get; set; }
        /// <summary>
        /// Gets or sets the name of the scope the style defines.
        /// </summary>
        /// <value>The name of the scope the style defines.</value>
        public string ScopeName { get; set; }
        /// <summary>
        /// Gets or sets the name of the CSS class.
        /// </summary>
        /// <value>The CSS class name.</value>
        public string CssClassName { get; set; }

        /// <summary>
        /// Gets or sets italic font style.
        /// </summary>
        /// <value>True if italic.</value>
        public bool Italic { get; set; }

        /// <summary>
        /// Gets or sets bold font style.
        /// </summary>
        /// <value>True if bold.</value>
        public bool Bold { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        /// <remarks>
        /// Returns the scope name if specified, or String.Empty otherwise.
        /// </remarks>
        public override string ToString()
        {
            return ScopeName ?? string.Empty;
        }
    }
}