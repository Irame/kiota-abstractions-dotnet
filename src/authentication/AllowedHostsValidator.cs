﻿// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Kiota.Abstractions.Authentication
{
    /// <summary>
    /// Validator for handling allowed hosts for authentication
    /// </summary>
    public class AllowedHostsValidator
    {
        private HashSet<string> _allowedHosts;

        /// <summary>
        /// The <see cref="AllowedHostsValidator"/> constructor
        /// </summary>
        /// <param name="validHosts"> Collection of valid Hosts</param>
        public AllowedHostsValidator(IEnumerable<string>? validHosts = null)
        {
            validHosts ??= Array.Empty<string>();
            ValidateHosts(validHosts);
            _allowedHosts = new HashSet<string>(validHosts, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets/Sets the collection of allowed hosts for the configurator
        /// </summary>
        public IEnumerable<string> AllowedHosts
        {
            get => _allowedHosts.AsEnumerable();
            set
            {
                if(value is null) throw new ArgumentNullException(nameof(value));
                ValidateHosts(value);
                _allowedHosts = new HashSet<string>(value.Where(x => !string.IsNullOrEmpty(x)), StringComparer.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Validates that the given Uri is valid
        /// </summary>
        /// <param name="uri">The <see cref="Uri"/> to validate</param>
        /// <returns>
        /// true - if the host is in the <see cref="AllowedHosts"/>. If <see cref="AllowedHosts"/> is empty, it will return true for all urls.
        /// false - if the <see cref="AllowedHosts"/> is not empty and the host is not in the list
        /// </returns>
        public bool IsUrlHostValid(Uri uri)
        {
            return _allowedHosts.Count == 0 || _allowedHosts.Contains(uri.Host);
        }

        private static void ValidateHosts(IEnumerable<string> hostsToValidate)
        {
            if(hostsToValidate is null) 
                throw new ArgumentNullException(nameof(hostsToValidate));

            if (hostsToValidate.Any(static host => host.StartsWith("http://", StringComparison.OrdinalIgnoreCase) 
                                                || host.StartsWith("https://", StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("host should not contain http or https prefix");
            }
        }
    }
}
