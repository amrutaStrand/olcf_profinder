//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Agilent.OpenLab.ProfinderApplication.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class CustomResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal CustomResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Agilent.OpenLab.ProfinderApplication.Properties.CustomResources", typeof(CustomResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Connection Failure.
        /// </summary>
        internal static string AuthenticationFailedCaption {
            get {
                return ResourceManager.GetString("AuthenticationFailedCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to authenticate. Please check your project privileges with your administrator..
        /// </summary>
        internal static string AuthenticationFailedMessage {
            get {
                return ResourceManager.GetString("AuthenticationFailedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Experiment Setup.
        /// </summary>
        internal static string ExperimentSetupPhaseCaption {
            get {
                return ResourceManager.GetString("ExperimentSetupPhaseCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Feature Extraction.
        /// </summary>
        internal static string MFEPhaseCaption {
            get {
                return ResourceManager.GetString("MFEPhaseCaption", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Results.
        /// </summary>
        internal static string ResultsPhaseCaption {
            get {
                return ResourceManager.GetString("ResultsPhaseCaption", resourceCulture);
            }
        }
    }
}
