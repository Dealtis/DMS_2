using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace DMSvStandard 
{
    class MenuCode
    {
        private String strCodeMenu;
        private String strBarcodeMenu;

        public MenuCode()
        {
            strCodeMenu = "";
            strBarcodeMenu = "";
        }

        public String getCodeMenu() { return strCodeMenu; }
        public void setGodeMenu(String _codeMenu) { strCodeMenu = _codeMenu; }

        public String getBarcodeMenu() { return strBarcodeMenu; }
        public void setBarcodeMenu(String _BarcodeMenu) { strBarcodeMenu = _BarcodeMenu; }
    }
}
