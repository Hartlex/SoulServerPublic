﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MasterServer.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MasterServer.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Initializing ClientManager....
        /// </summary>
        internal static string ClientManager_Initialize_Load {
            get {
                return ResourceManager.GetString("ClientManager_Initialize_Load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ClientManager Initialized!.
        /// </summary>
        internal static string ClientManager_Initialize_Success {
            get {
                return ResourceManager.GetString("ClientManager_Initialize_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client wasnt found so no need to remove it!.
        /// </summary>
        internal static string ClientManager_RemoveClient_Error {
            get {
                return ResourceManager.GetString("ClientManager_RemoveClient_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Try to remove Client with id: .
        /// </summary>
        internal static string ClientManager_RemoveClient_Load_ {
            get {
                return ResourceManager.GetString("ClientManager_RemoveClient_Load#", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully removed Client with id: .
        /// </summary>
        internal static string ClientManager_RemoveClient_Success {
            get {
                return ResourceManager.GetString("ClientManager_RemoveClient_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Clearing all Client Connections.
        /// </summary>
        internal static string ClientManager_Shutdown_Clear {
            get {
                return ResourceManager.GetString("ClientManager_Shutdown_Clear", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to New Client with id: {0} succesfully added!.
        /// </summary>
        internal static string ClientManager_UpdateOrAddClient_ClientAddSuccess {
            get {
                return ResourceManager.GetString("ClientManager_UpdateOrAddClient_ClientAddSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Try to Add or Update Client with id: .
        /// </summary>
        internal static string ClientManager_UpdateOrAddClient_Load {
            get {
                return ResourceManager.GetString("ClientManager_UpdateOrAddClient_Load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Client with id: {0} succesfully updated!.
        /// </summary>
        internal static string ClientManager_UpdateOrAddClient_UpdateSuccess {
            get {
                return ResourceManager.GetString("ClientManager_UpdateOrAddClient_UpdateSuccess", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Try to load DatabaseConfig....
        /// </summary>
        internal static string DatabaseHelper_LoadDatabaseConfig_Load {
            get {
                return ResourceManager.GetString("DatabaseHelper_LoadDatabaseConfig_Load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DatabaseConfig successfully loaded....
        /// </summary>
        internal static string DatabaseHelper_LoadDatabaseConfig_Success {
            get {
                return ResourceManager.GetString("DatabaseHelper_LoadDatabaseConfig_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Try to access Database....
        /// </summary>
        internal static string DatabaseHelper_TestDbConnection_Load {
            get {
                return ResourceManager.GetString("DatabaseHelper_TestDbConnection_Load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully accessed Database.
        /// </summary>
        internal static string DatabaseHelper_TestDbConnection_Success {
            get {
                return ResourceManager.GetString("DatabaseHelper_TestDbConnection_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing Database Connection....
        /// </summary>
        internal static string DatabaseManager_Initialize_Load {
            get {
                return ResourceManager.GetString("DatabaseManager_Initialize_Load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DatabaseConnection successfully initialized!.
        /// </summary>
        internal static string DatabaseManager_Initialize_Success {
            get {
                return ResourceManager.GetString("DatabaseManager_Initialize_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing Network....
        /// </summary>
        internal static string NetworkConfig_InitialiseNetwork_Load {
            get {
                return ResourceManager.GetString("NetworkConfig_InitialiseNetwork_Load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Network successfully initialized!.
        /// </summary>
        internal static string NetworkConfig_InitialiseNetwork_Success {
            get {
                return ResourceManager.GetString("NetworkConfig_InitialiseNetwork_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully loaded NetworkConfig!.
        /// </summary>
        internal static string NetworkConfig_InitialiseNetwork_Successfully_loaded_NetworkConfig_ {
            get {
                return ResourceManager.GetString("NetworkConfig_InitialiseNetwork_Successfully_loaded_NetworkConfig_", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Try to load NetworkConfig....
        /// </summary>
        internal static string NetworkConfig_InitialiseNetwork_Try_to_load_NetworkConfig___ {
            get {
                return ResourceManager.GetString("NetworkConfig_InitialiseNetwork_Try_to_load_NetworkConfig___", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stopped listening on .
        /// </summary>
        internal static string NetworkConfig_StopListening_Stop {
            get {
                return ResourceManager.GetString("NetworkConfig_StopListening_Stop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing PacketParser....
        /// </summary>
        internal static string PacketParser_Initialize_Load {
            get {
                return ResourceManager.GetString("PacketParser_Initialize_Load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully initialized PacketParser!.
        /// </summary>
        internal static string PacketParser_Initialize_Success {
            get {
                return ResourceManager.GetString("PacketParser_Initialize_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unloaded all PacketProcessors!.
        /// </summary>
        internal static string PacketParser_Unload_Success {
            get {
                return ResourceManager.GetString("PacketParser_Unload_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing PacketProcessor....
        /// </summary>
        internal static string PacketProcessor_Initialize_Load {
            get {
                return ResourceManager.GetString("PacketProcessor_Initialize_Load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Successfully initialized PacketProcessor!.
        /// </summary>
        internal static string PacketProcessor_Initialize_Success {
            get {
                return ResourceManager.GetString("PacketProcessor_Initialize_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} successfully loaded.
        /// </summary>
        internal static string PacketProcessor_InitializeCategories__load {
            get {
                return ResourceManager.GetString("PacketProcessor_InitializeCategories__load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing Servers and Channels....
        /// </summary>
        internal static string Server_InitializeServerAndChannels_Load {
            get {
                return ResourceManager.GetString("Server_InitializeServerAndChannels_Load", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Starting Server....
        /// </summary>
        internal static string Server_Start_Start {
            get {
                return ResourceManager.GetString("Server_Start_Start", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server successfully started!.
        /// </summary>
        internal static string Server_Start_Success {
            get {
                return ResourceManager.GetString("Server_Start_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Stoping Server....
        /// </summary>
        internal static string Server_Stop_Stop {
            get {
                return ResourceManager.GetString("Server_Stop_Stop", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Server successfully stopped!.
        /// </summary>
        internal static string Server_Stop_Success {
            get {
                return ResourceManager.GetString("Server_Stop_Success", resourceCulture);
            }
        }
    }
}
