using System;
using System.Collections.Generic;
using System.Text;

namespace Maquina.BusinessDomain.RulesEngine.Model
{
    public class MetamodelModel
    {
        public List<ScriptTypeModel> ScriptTypes { get; set; }

        public List<MethodModel> Conditions { get; set; }

        public List<MethodModel> Actions { get; set; }

        public List<EnumerationModel> Enumerations { get; set; }
    }
}
