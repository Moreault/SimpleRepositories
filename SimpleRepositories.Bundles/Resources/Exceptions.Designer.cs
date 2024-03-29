﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ToolBX.SimpleRepositories.Bundles.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Exceptions {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Exceptions() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ToolBX.SimpleRepositories.Bundles.Resources.Exceptions", typeof(Exceptions).Assembly);
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
        ///   Looks up a localized string similar to Cannot find entity of type {0} with ID &apos;{1}&apos;.
        /// </summary>
        internal static string EntityWithIdNotFound {
            get {
                return ResourceManager.GetString("EntityWithIdNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot find entity of type {0} using predicate.
        /// </summary>
        internal static string EntityWithPredicateNotFound {
            get {
                return ResourceManager.GetString("EntityWithPredicateNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot insert entity of type {0} : ID to be inserted was modified from &apos;{1}&apos; to &apos;{2}&apos;.
        /// </summary>
        internal static string IdWasChangedBeforeInsert {
            get {
                return ResourceManager.GetString("IdWasChangedBeforeInsert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot update entity of type {0} : there is no entity with ID &apos;{1}&apos;.
        /// </summary>
        internal static string NoEntityFoundToUpdate {
            get {
                return ResourceManager.GetString("NoEntityFoundToUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string NoEntityToDelete {
            get {
                return ResourceManager.GetString("NoEntityToDelete", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot insert entities of type {0} : no entity was provided.
        /// </summary>
        internal static string NoEntityToInsert {
            get {
                return ResourceManager.GetString("NoEntityToInsert", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot update entities of type {0} : no entity was provided.
        /// </summary>
        internal static string NoEntityToUpdate {
            get {
                return ResourceManager.GetString("NoEntityToUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot delete entities of type {0} : predicate did not yield any result.
        /// </summary>
        internal static string TryingToDeleteInexistantEntities {
            get {
                return ResourceManager.GetString("TryingToDeleteInexistantEntities", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot delete entities of type {0} : one or more entity in collection was null.
        /// </summary>
        internal static string TryingToDeleteNulls {
            get {
                return ResourceManager.GetString("TryingToDeleteNulls", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot insert entities of type {0} : one or more entity in collection was null.
        /// </summary>
        internal static string TryingToInsertNulls {
            get {
                return ResourceManager.GetString("TryingToInsertNulls", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot update entities of type {0} : one or more entity in collection was null
        ///.
        /// </summary>
        internal static string TryingToUpdateNulls {
            get {
                return ResourceManager.GetString("TryingToUpdateNulls", resourceCulture);
            }
        }
    }
}
