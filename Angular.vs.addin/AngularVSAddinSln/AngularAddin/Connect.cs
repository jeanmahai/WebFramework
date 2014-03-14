using System;
using Extensibility;
using EnvDTE;
using EnvDTE80;
namespace AngularAddin
{
    /// <summary>The object for implementing an Add-in.</summary>
    /// <seealso class='IDTExtensibility2' />
    public class Connect : IDTExtensibility2
    {
        /// <summary>
        /// 向output中输出消息
        /// </summary>
        /// <param name="message"></param>
        private void Display(string message)
        {
            var outputWin = this._applicationObject.Windows.Item(EnvDTE.Constants.vsWindowKindOutput).Object as OutputWindow;
            if (outputWin == null) return;
            outputWin.ActivePane.Activate();
            outputWin.ActivePane.OutputString(message);
        }

        /// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
        public Connect()
        {
        }

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;
            _addInInstance = (AddIn)addInInst;

            var docEvt = this._applicationObject.Events.get_DocumentEvents();
            docEvt.DocumentSaved += new _dispDocumentEvents_DocumentSavedEventHandler(docEvt_DocumentSaved);
        }

        void docEvt_DocumentSaved(Document Document)
        {
            if (Document == null) return;

            if (!Document.Name.ToUpper().EndsWith(".AGV.HTM")
                || !Document.Name.ToUpper().EndsWith(".AGV.HTML"))
                return;

            //用模版文件生成angular view 文件
            var tmpl = new AngularViewTmpl();

            var txt = tmpl.TransformText();

            //将生成的文件保存并加载到工程中
            //http://www.c-sharpcorner.com/UploadFile/mgold/AddIns11292005015631AM/AddIns.aspx
            //http://www.cnblogs.com/anderslly/archive/2009/05/30/vs-addin-summary.html

            //获得当前正在编辑的项目
            var curPro = this._applicationObject.Solution.Projects.DTE.ActiveSolutionProjects as Project;
            var curDoc = curPro.DTE.ActiveDocument as Document;
            var docPath = curDoc.FullName;
            //curPro.ProjectItems.Add
        }

        /// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
        /// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
        {

        }

        /// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />		
        public void OnAddInsUpdate(ref Array custom)
        {
        }

        /// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnStartupComplete(ref Array custom)
        {
        }

        /// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
        /// <param term='custom'>Array of parameters that are host application specific.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnBeginShutdown(ref Array custom)
        {
        }

        private DTE2 _applicationObject;
        private AddIn _addInInstance;
    }
}