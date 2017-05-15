using Refactoring.CodeTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Refactoring.CodeTest
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        //TODO: handle error
        public bool IsFailoverModeEnabled()
        {
            return ConfigurationManager.AppSettings["IsFailoverModeEnabled"] == "true" || ConfigurationManager.AppSettings["IsFailoverModeEnabled"] == "True";
        }
    }
}
