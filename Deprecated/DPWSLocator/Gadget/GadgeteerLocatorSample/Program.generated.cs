//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18047
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GadgeteerLocatorSample {
    using Gadgeteer;
    using GTM = Gadgeteer.Modules;
    
    
    public partial class Program : Gadgeteer.Program {
        
        private Gadgeteer.Modules.GHIElectronics.Display_TE35 display_TE35;
        
        private Gadgeteer.Modules.GHIElectronics.Ethernet_J11D ethernet_J11D;
        
        private Gadgeteer.Modules.GHIElectronics.UsbClientDP usbClientDP;
        
        public static void Main() {
            // Important to initialize the Mainboard first
            Program.Mainboard = new GHIElectronics.Gadgeteer.FEZSpider();
            Program p = new Program();
            p.InitializeModules();
            p.ProgramStarted();
            // Starts Dispatcher
            p.Run();
        }
        
        private void InitializeModules() {
            this.usbClientDP = new GTM.GHIElectronics.UsbClientDP(1);
            this.ethernet_J11D = new GTM.GHIElectronics.Ethernet_J11D(7);
            this.display_TE35 = new GTM.GHIElectronics.Display_TE35(14, 13, 12, 10);
        }
    }
}