using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpyMinusMinus {
    class WindowManager {

        private VirtualWindow targetWindow;
        private NamedPipeServer pipeMessageListener;
        private MessageLogForm messageForm;
        private PropertiesForm propertiesForm;


        public WindowManager(VirtualWindow window) {
            targetWindow = window;

            InitPropertiesForm();
            //InitMessageLog();
            
        }


        private void InitPropertiesForm() {
            propertiesForm = new PropertiesForm(targetWindow);
            propertiesForm.Show();
        }


        private void InitMessageLog() {
            Hook();
            messageForm = new MessageLogForm(targetWindow);
            pipeMessageListener = new NamedPipeServer(messageForm);
            messageForm.Show();
        }


        private void Hook() {
            IntPtr listener = IntPtr.Zero;
            int hook = HookWrapper.Hook(targetWindow.handle, listener);
        }
     

    }
}
